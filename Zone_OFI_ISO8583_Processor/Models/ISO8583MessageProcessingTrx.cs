using Trx.Messaging;
using Zone_OFI_ISO8583_Processor.Utilities;
using Trx.Messaging.Iso8583;
using Trx.Communication.Channels.Sinks.Framing;
using Trx.Communication.Channels.Sinks;
using Trx.Communication.Channels;
using Trx.Coordination.TupleSpace;
using Trx.Communication.Channels.Tcp;
using Microsoft.Data.Sqlite;

namespace Zone_OFI_ISO8583_Processor.Models
{
    public class ISO8583MessageProcessingTrx
    {
        private const int Field2PAN = 2;
        private const int Field3ProcCode = 3;
        private const int Field4TransactionAmount = 4;
        private const int Field7TransDateTime = 7;
        private const int Field11Trace = 11;
        private const int Field12LocalTransDate = 12;
        private const int Field13LocalTranstime = 13;
        private const int Field18MarchantCategoryCode = 18;
        private const int Field24Nii = 24;
        private const int Field32AcquirerInstitutionCode = 32;
        private const int Field37RetrivalReferenceNumber = 37;
        private const int Field41TerminalCode = 41;
        private const int Field42MerchantCode = 42;
        private const int Field49TransactionCurrencyCode = 49;
        private const int Field52PINBlock = 52;
        private const int Field70NetworkMgtCode = 70;
        private const int Field102AccountIdentification1 = 102;
        private const int Field123additionalData = 123;

        private readonly TcpClientChannel _client;
        private readonly VolatileStanSequencer _sequencer;

        private readonly string _terminalCode;

        private int _expiredRequests;
        private int _requestsCnt;

        public int RequestsCount
        {
            get { return _requestsCnt; }
        }

        public int ExpiredRequests
        {
            get { return _expiredRequests; }
        }
        

        public async void SendEchoMessage()
        {
            // Build echo test message.
            var echoMsg = CreateEchoMessage();
            var response = await SendMessage(echoMsg);
            Console.WriteLine(response);
            Console.WriteLine("============Done============");
        }

        public async void SendKeyRequestMessage(SqliteConnection connection)
        {
            string clearZPK = string.Empty, generatedKCV = string.Empty, valKCV = string.Empty;
            var kv = SQLite.FetchKeyVaultData(connection); 

            Iso8583Message keyExchangeMsg = await CreateKeyExchangeMessage();
            var response = await SendMessage(keyExchangeMsg);
            var encryptedZPK = response[53]?.Value.ToString();

            var result = ZPKSessionKeyHandler.DecryptZPKSessionKey(kv.ZMK, encryptedZPK);
            clearZPK = result.clearZPK;
            valKCV = result.valKcv;
            generatedKCV = ZPKSessionKeyHandler.GenerateKCV(clearZPK);

            if (valKCV.Equals(generatedKCV))
            {
                var values = new KeyVault
                {
                    ZPK = clearZPK,
                    ZPK_Date = DateTime.Now
                };
                SQLite.UpdateZPK(connection, values);
            }
            else
                Console.WriteLine("=======Error: KCV validation failed =======");

            Console.WriteLine("==========Done=========");
        }

        public async void SendFinancialMessage(string pin, string pan, SqliteConnection connection)
        {
            string clearZPK = string.Empty, generatedKCV = string.Empty, valKCV = string.Empty;

            var sendFinMessage = await CreateFinancialMessage(pin,pan,connection);

            var response = await SendMessage(sendFinMessage);

            if (response != null)
            {
                var receivedKey = response[53]?.Value.ToString();

                var db = SQLite.FetchKeyVaultData(connection);

                var result = ZPKSessionKeyHandler.DecryptZPKSessionKey(db.ZMK, receivedKey);
                clearZPK = result.clearZPK;
                valKCV = result.valKcv;
                generatedKCV = ZPKSessionKeyHandler.GenerateKCV(clearZPK);

                if (valKCV.Equals(generatedKCV))
                {
                    var values = new KeyVault
                    {
                        ZPK = clearZPK,
                        ZPK_Date = DateTime.Now
                    };
                    SQLite.UpdateZPK(connection, values);
                }
                else
                {
                    Console.WriteLine("=============Error: KCV validation failed =============");
                }
                Console.WriteLine("==========Done=========");
            }
        }

        private Iso8583Message CreateEchoMessage()
        {
            var echoMsg = new Iso8583Message(0800);
            echoMsg.Fields.Add(Field7TransDateTime, "0905091101");
            echoMsg.Fields.Add(Field11Trace, "642796");
            echoMsg.Fields.Add(Field70NetworkMgtCode, "301");
            return echoMsg;
        }
        private async Task<Iso8583Message> CreateKeyExchangeMessage()
        {
            var keyExchangeMsg = new Iso8583Message(0800);
            keyExchangeMsg.Fields.Add(Field7TransDateTime, "0905091101");
            keyExchangeMsg.Fields.Add(Field11Trace, "642796");
            keyExchangeMsg.Fields.Add(Field32AcquirerInstitutionCode, "040");
            keyExchangeMsg.Fields.Add(Field70NetworkMgtCode, "101");
            return keyExchangeMsg;
        }

        public async Task<Iso8583Message> CreateFinancialMessage(string pin, string pan, SqliteConnection connection)
        {
            var finMessage = new Iso8583Message(0200);
            finMessage.Fields.Add(Field2PAN, "5559405048128222");
            finMessage.Fields.Add(Field3ProcCode, "0905091101");
            finMessage.Fields.Add(Field4TransactionAmount, "000000001000");
            finMessage.Fields.Add(Field7TransDateTime, "0905091101");
            finMessage.Fields.Add(Field11Trace, "642795");
            finMessage.Fields.Add(Field12LocalTransDate, "0905");
            finMessage.Fields.Add(Field13LocalTranstime, "091101");
            finMessage.Fields.Add(Field18MarchantCategoryCode, "5411");
            finMessage.Fields.Add(Field32AcquirerInstitutionCode, "4008");
            finMessage.Fields.Add(Field37RetrivalReferenceNumber, "451298");
            finMessage.Fields.Add(Field41TerminalCode, "20351254");
            finMessage.Fields.Add(Field42MerchantCode, "0905091101");
            finMessage.Fields.Add(Field49TransactionCurrencyCode, "566");
            finMessage.Fields.Add(Field52PINBlock, await GenerateEncryptedPinBlock(pin, pan, connection));
            finMessage.Fields.Add(Field102AccountIdentification1, "12345678901234567890");
            finMessage.Fields.Add(Field123additionalData, "91010151134");

            return finMessage;
        }
        
        private Pipeline CreatePipeline()
        {
            var pipeline = new Pipeline();
            pipeline.Push(new ReconnectionSink());
            pipeline.Push(new NboFrameLengthSink(2) { IncludeHeaderLength = false, MaxFrameLength = 1024 });
            pipeline.Push(
                new MessageFormatterSink(new Iso8583MessageFormatter((@"Formatters\Iso8583Ascii1987.xml"))));
            return pipeline;
        }

        public async Task<Message> SendMessage(Iso8583Message msg)
        {
            var pipeline = CreatePipeline();
            var ts = new TupleSpace<ReceiveDescriptor>();

            var _client = new TcpClientChannel(pipeline, ts, new FieldsMessagesIdentifier(new[] { 11 }))
            {
                RemotePort = 12000,
                RemoteInterface = "52.234.156.59",
                Name = "Merchant"
            };

            _client.Connect();

            await Task.Delay(5000);

            SendRequestHandlerCtrl sndCtrl = _client.SendExpectingResponse(msg, 1000);
            sndCtrl.WaitCompletion(); // Wait send completion.
            if (!sndCtrl.Successful)
            {
                Console.WriteLine(string.Format("Merchant: unsuccessful request # {0} ({1}.",
                    "10000", sndCtrl.Message));
                if (sndCtrl.Error != null)
                    Console.WriteLine(sndCtrl.Error);
            }
            sndCtrl.Request.WaitResponse();
            Message response = (Message)sndCtrl.Request.ReceivedMessage;
            _client.Close();

            return response;
        }

        public async Task<string> GenerateEncryptedPinBlock(string pin, string pan, SqliteConnection connection)
        {
            var keyVault = SQLite.FetchKeyVaultData(connection);
            string clearPinBlock = Iso8583PinBlockGenerator.GenerateIso0PinBlock(pin, pan);

            byte[] clearPinBlockBytes = TripleDESHelper.HexStringToByteArray(clearPinBlock);
            byte[] zpkBytes = TripleDESHelper.HexStringToByteArray(keyVault.ZPK);

            string encryptedPINBlock = TripleDESHelper.ByteArrayToHexString(TripleDESHelper.TripleDesEncrypt(clearPinBlockBytes, zpkBytes));
            return encryptedPINBlock;
        }
    }
}
