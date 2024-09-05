using BIM_ISO8583.NET;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Zone_OFI_ISO8583_Processor.Utilities;

namespace Zone_OFI_ISO8583_Processor.Models
{
    public class ISO8583Processing
    {
        public static void SendEchoMessage()
        {
            #region Build Message Body
            string MTI = "0800"; // Echo message MTI
            string TransDate = DateTime.Now.ToString("MMddHHmmss");//TransactionDate
            string STAN = "000001"; // STAN
            string NetworkMgtCode = "301"; // Network management code (301 for echo)
            
            ISO8583 iso8583 = new ISO8583();
            
            string[] DE = new string[130];
            DE[7] = TransDate;
            DE[11] = STAN;
            DE[70] = NetworkMgtCode;
            
            string newIsoMessage = iso8583.Build(DE, MTI);

            Console.WriteLine($"ISO Request Message -> {newIsoMessage}");
            #endregion
            
            var response = TcpHelper.SocketSendClientRequest(newIsoMessage);
            
            Console.WriteLine(response);

            if(string.IsNullOrEmpty(response))
                return;

            var isoResponse = Unpack(response);
            if (isoResponse[39] == Response.SUCCESSFUL)
                Console.WriteLine("Echo Successful");
        }

        public static void SendKeyRequestMessage(SqliteConnection connection)
        {
            string clearZPK = string.Empty, generatedKCV = string.Empty,valKCV = string.Empty;

            #region Build Message Body
            string MTI = "0800"; // Echo message MTI
            string TransDate = DateTime.Now.ToString("MMddHHmmss");//TransactionDate
            string STAN = "000002"; // STAN
            string NetworkMgtCode = "101"; // Network management code (101 for Key Exchange)
            
            ISO8583 iso8583 = new ISO8583();
            
            string[] DE = new string[130];
            DE[7] = TransDate;
            DE[11] = STAN;
            DE[70] = NetworkMgtCode;
            
            string newIsoMessage = iso8583.Build(DE, MTI);

            Console.WriteLine($"ISO Key Exchange Request Message -> {newIsoMessage}");
            #endregion

            var response = TcpHelper.SocketSendClientRequest(newIsoMessage);

            Console.WriteLine($"ISO 8583 Key Exchange Response Message -> {response}");

            if (string.IsNullOrEmpty(response))
                return;

            var isoResponse = Unpack(response);

            if (isoResponse[39] == Response.SUCCESSFUL)
            {
                Console.WriteLine("Key Exchange Successful");
                Console.WriteLine("Initiating ZPK Decryption");
            }
            else
            {
                Console.WriteLine("Key exchange failed");
                return;
            }

           
        }

        public static void SendFinancialMessage(string pin, string pan, SqliteConnection connection)
        {
            #region Build Message Body
            string MTI = "0200"; // Field 01  MTI 
            string PAN = pan;// Field 02 
            string processingCode = "1234567890123456";// Field 03 
            string transactionAmount = "000000001000";// Field 04 
            string transmissionDate = DateTime.Now.ToString("MMddHHmmss");//TransactionDate
            string STAN = "000002"; // STAN
            string localTransactionTime = DateTime.Now.ToString("HHmmss");//TransactionDate
            string localTransactionDate = DateTime.Now.ToString("MMdd");//TransactionDate
            string marchantCategoryCode = "5411";
            string acquirerInstitutionId = "12345678";
            string retrivalReferenceNumber = "123456789012";
            string cardAcceptorTerminalId = "ATM12345";
            string cardAcceptorIdCode = "M123456789012345";
            string transactionCurrencyCode = "560";
            string PINBlock = GenerateEncryptedPinBlock(pin, pan, connection);
            string accountIdentification1 = "12345678901234567890";
            string additionalData = "91010151134"; 

            ISO8583 iso8583 = new ISO8583();

            string[] DE = new string[130];
            DE[2] = PAN;
            DE[3] = processingCode;
            DE[4] = transactionAmount;
            DE[7] = transmissionDate;
            DE[11] = STAN;
            DE[12] = localTransactionTime;
            DE[13] = localTransactionDate;
            DE[18] = marchantCategoryCode;
            DE[32] = acquirerInstitutionId;
            DE[37] = retrivalReferenceNumber;
            DE[41] = cardAcceptorTerminalId;
            DE[42] = cardAcceptorIdCode;
            DE[49] = transactionCurrencyCode;
            DE[52] = PINBlock;
            DE[102] = accountIdentification1;
            DE[123] = additionalData;

            string newIsoMessage = iso8583.Build(DE, MTI);
            Console.WriteLine($"ISO Financial Message -> {newIsoMessage}");
            #endregion
            
            var response = TcpHelper.SocketSendClientRequest(newIsoMessage);
            Console.WriteLine("Financial Message Sent, Response Received: -> " + response);


            if (string.IsNullOrEmpty(response))
                return;

            var isoResponse = Unpack(response);

            if (isoResponse[39] == Response.SUCCESSFUL)
            {
                Console.WriteLine("Transaction Message Successful");
            }
            else
            {
                Console.WriteLine("Transaction failed");
                return;
            }

            var journalDto = JournalDto.FromArray(isoResponse);

            if (journalDto == null)
            {
                Console.WriteLine("Error occurred while parsing server response");
                return;
            }

            PostJournal(journalDto);
            Console.WriteLine("==========Done=========");
        }

        public static void SendReversalMessage(string pin, string pan, SqliteConnection connection)
        {
            #region Build Message Body
            string MTI = "0420"; // Field 01  MTI 
            string PAN = pan;// Field 02 
            string processingCode = "200000";// Field 03 
            string transactionAmount = "000000001000";// Field 04 
            string transmissionDate = DateTime.Now.ToString("MMddHHmmss");//TransactionDate
            string STAN = "123456"; // STAN
            string localTransactionTime = DateTime.Now.ToString("HHmmss");//TransactionDate
            string localTransactionDate = DateTime.Now.ToString("MMdd");//TransactionDate
            string acquirerInstitutionId = "12345678";
            string retrivalReferenceNumber = "123456789012";
            string authorizationIdResponse = "123456"; // Field38
            string responseCode = "00"; //Field39
            string cardAcceptorTerminalId = "ATM12345";
            string cardAcceptorIdCode = "M123456789012345";
            string transactionCurrencyCode = "560";
            string originalDataElement = "02001234561234567890123456000050000916173030123456";
            string replacementAmount = "000000000000";

            ISO8583 iso8583 = new ISO8583();

            string[] DE = new string[130];
            DE[2] = PAN; //----
            DE[3] = processingCode;
            DE[4] = transactionAmount;
            DE[7] = transmissionDate;
            DE[11] = STAN;
            DE[12] = localTransactionTime;
            DE[13] = localTransactionDate;
            DE[32] = acquirerInstitutionId;
            DE[37] = retrivalReferenceNumber;
            DE[38] = authorizationIdResponse;
            DE[39] = responseCode;
            DE[41] = cardAcceptorTerminalId;
            DE[42] = cardAcceptorIdCode;
            DE[49] = transactionCurrencyCode;
            DE[90] = originalDataElement;
            DE[95] = replacementAmount;

            string newIsoMessage = iso8583.Build(DE, MTI);
            Console.WriteLine($"ISO Reversal Message -> {newIsoMessage}");
            #endregion

            var response = TcpHelper.SocketSendClientRequest(newIsoMessage);
            Console.WriteLine("Financial Message Sent, Response Received: " + response);


            if (string.IsNullOrEmpty(response))
                return;

            var isoResponse = Unpack(response);

            if (isoResponse[39] == Response.SUCCESSFUL)
            {
                Console.WriteLine("Reversal Message Successful");
            }
            else
            {
                Console.WriteLine("Transaction failed");
                return;
            }

            var journalDto = JournalDto.FromArray(isoResponse);

            if (journalDto == null)
            {
                Console.WriteLine("Error while parsing server response");
                return;
            }

            PostJournal(journalDto);
            Console.WriteLine("==========Done=========");
        }
        
        public static string GenerateEncryptedPinBlock(string pin, string pan, SqliteConnection connection)
        {
            var keyVault = SQLite.FetchKeyVaultData(connection);
            string clearPinBlock =Iso8583PinBlockGenerator.GenerateIso0PinBlock(pin, pan);

            byte[] clearPinBlockBytes = TripleDESHelper.HexStringToByteArray(clearPinBlock);
            byte[] zpkBytes = TripleDESHelper.HexStringToByteArray(keyVault.ZPK);

            string encryptedPINBlock = TripleDESHelper.ByteArrayToHexString(TripleDESHelper.TripleDesEncrypt(clearPinBlockBytes, zpkBytes));
            return encryptedPINBlock;
        }
        
        private static string[] Unpack(string resData)
        {
            ISO8583 iso8583 = new ISO8583();
            string[] DE;
            DE = iso8583.Parse(resData);
            return DE;
        }

        public static async void PostJournal(JournalDto journalDto)
        {
            var appSettings = ZNConnection.GetDetiails();
            var serviceProvider = new ServiceCollection()
                .AddHttpClient<ApiClient>()// This is expected to register the ApiClient with HttpClientFactory with AP_Key
                .Services
                .BuildServiceProvider();

            var apiClient = serviceProvider.GetRequiredService<ApiClient>();

            try
            {
                var fullApiUrl = $"http://{appSettings.PushJournalHostAddress}:{appSettings.PushJournalHostPort}{appSettings.PushJournalEndPoint}";
                var result = apiClient.PostAsync(fullApiUrl, journalDto).Result;
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
