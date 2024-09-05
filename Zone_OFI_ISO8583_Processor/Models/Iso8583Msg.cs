using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Net.Sockets;
using System.Text;

namespace Zone_OFI_ISO8583_Processor.Models
{
    public class Iso8583Msg
    {
        private const int MAX_LEN_ACCT_INQ = 478;

        private string m_mti { get; set; }

        // public List<Iso8583Field> Iso8583flds;
        public Dictionary<string, Iso8583Field> Iso8583flds;

        public string AccountIden1
        {
            get=> Iso8583flds["102"].Value;
            set => Iso8583flds["102"].Value = value;
        }

        public string AccountIden2
        {
            get => Iso8583flds["103"].Value;
            set => Iso8583flds["103"].Value = value; 
        }

        public string AcquirerInstId
        {
            get => Iso8583flds["32"].Value;
            set => Iso8583flds["32"].Value = Convert.ToString(value);
        }

        public string AccountBalance
        {
            get => Iso8583flds["54"].Value;
            set => Iso8583flds["54"].Value = Convert.ToString(value);
        }

        public string AdditionalData
        {
            get => Iso8583flds["48"].Value;
            set => Iso8583flds["48"].Value = Convert.ToString(value);
        }
        public string AdditionalRspData
        {
            get => Iso8583flds["44"].Value;
            set => Iso8583flds["44"].Value = Convert.ToString(value);
        }

        //public AmountFees AmtFees
        //{
        //    get
        //    {
        //        return new AmountFees(Iso8583flds["46"].Value);
        //    }
        //    set
        //    {
        //        Iso8583flds["46"].Value = value.ToString();
        //    }
        //}

        public string ApprovalCode
        {
            get => Iso8583flds["38"].Value;
            set => Iso8583flds["38"].Value = value;
        }

        public string CardAcceptorId
        {
            get=> Iso8583flds["42"].Value;
            set=> Iso8583flds["42"].Value = Convert.ToString(value);
        }

        public string CardAcceptorNameLoc
        {
            get => Iso8583flds["43"].Value;
            set => Iso8583flds["43"].Value = Convert.ToString(value);
        }

        public string CardAcceptorTerminalId
        {
            get => Iso8583flds["41"].Value;
            set => Iso8583flds["41"].Value = Convert.ToString(value);
        }

        public int CardSeqNo
        {
            get => Convert.ToInt32(Iso8583flds["23"].Value);
            set => Iso8583flds["23"].Value = Convert.ToString(value);
        }

        public DateTime DateCapture
        {
            get => DateTime.ParseExact(Iso8583flds["17"].Value, "yyyyMMdd", null);
            set => Iso8583flds["17"].Value = value.ToString("yyyyMMdd");
        }

        public DateTime DateExp
        {
            get => DateTime.ParseExact(Iso8583flds["15"].Value, "MMyy", CultureInfo.InvariantCulture);
            set => Iso8583flds["15"].Value = value.ToString("MMyy");
        }

        public DateTime DateStl
        {
            get => DateTime.ParseExact(Iso8583flds["14"].Value, "yyMM", CultureInfo.InvariantCulture);
            set => Iso8583flds["14"].Value = value.ToString("yyMM");
        }

        public string ExtendedPAN
        {
            get => Iso8583flds["34"].Value;
            set => Iso8583flds["34"].Value = Convert.ToString(value);
        }

        public string PrimaryAccountNo
        {
            get => Iso8583flds["2"].Value;
            set => Iso8583flds["2"].Value = Convert.ToString(value);
        }

        public string Field_125
        {
            get => Iso8583flds["125"].Value;
            set => Iso8583flds["125"].Value = value;
        }

        public string Field_126
        {
            get => Iso8583flds["126"].Value;
            set => Iso8583flds["126"].Value = value;
        }

        //public string Field_127
        //{
        //    get
        //    {
        //        return this.Iso8583flds["127"].Value;
        //    }
        //    set
        //    {
        //        this.Iso8583flds["127"].Value = value;
        //    }
        //}

        public string Field_93
        {
            get => Iso8583flds["93"].Value;
            set => Iso8583flds["93"].Value = value;
        }

        public string Field_94
        {
            get => Iso8583flds["94"].Value;
            set => Iso8583flds["94"].Value = value;
        }

        public string FowarderInstCode
        {
            get => Iso8583flds["33"].Value;
            set => Iso8583flds["33"].Value = Convert.ToString(value);
        }

        public FUNCTION_CODE FunctionCode
        {
            get => (FUNCTION_CODE)Enum.Parse(typeof(FUNCTION_CODE), Iso8583flds["24"].Value);
            set => Iso8583flds["24"].Value = ((int)value).ToString();
        }

        public DateTime LocalDateTime
        {
            get => DateTime.ParseExact(Iso8583flds["13"].Value, "MMyy", CultureInfo.InvariantCulture);
            set => Iso8583flds["13"].Value = value.ToString("MMyy");
        }

        public string LocalTxnDateTime
        {
            get => Iso8583flds["12"].Value;
            set => Iso8583flds["12"].Value = value;
        }

        public int MerchantType
        {
            get => int.Parse(Iso8583flds["18"].Value);
            set => Iso8583flds["18"].Value = value.ToString();
        }

        public string MessageType { get; set; }

        public string NetworkMgmtCode
        {
            get => Iso8583flds["70"].Value;
            set => Iso8583flds["70"].Value = value;
        }
        /// <summary>
        /// altered during working for reversal
        /// </summary>
        //public decimal OriginalAmounts
        //{
        //    get
        //    {
        //        return Convert.ToDecimal(this.Iso8583flds["30"].Value);
        //    }
        //    set
        //    {
        //        //this.Iso8583flds["30"].Value = value.ToString();
        //        Iso8583Field item = this.Iso8583flds["30"];
        //        double num = double.Parse(value.ToString());
        //        item.Value = num.ToString();
        //    }
        //}

        public string OriginalAmounts
        {
            get => Iso8583flds["30"].Value;
            set => Iso8583flds["30"].Value = value;
        }

        public string OriginalDataElements
        {
            get => Iso8583flds["56"].Value;
            set => Iso8583flds["56"].Value = value;
        }

        //public AmountFees OriginalFees
        //{
        //    get
        //    {
        //        return new AmountFees(Iso8583flds["66"].Value);
        //    }
        //    set
        //    {
        //        Iso8583flds["66"].Value = value.ToString();
        //    }
        //}

        public int PosCondCode
        {
            get => int.Parse(Iso8583flds["25"].Value);
            set => Iso8583flds["25"].Value = value.ToString();
        }

        public string PosDataCode
        {
            get => Iso8583flds["123"].Value;
            set => Iso8583flds["123"].Value = value;
        }

        public int PosEntryMode
        {
            get => int.Parse(Iso8583flds["22"].Value);
            set => Iso8583flds["22"].Value = value.ToString();
        }

        //public string PosEntryMode23
        //{
        //    get
        //    {
        //        return this.Iso8583flds["23"].Value;
        //    }
        //    set
        //    {
        //        this.Iso8583flds["23"].Value = Convert.ToString(value);
        //    }
        //}

        public string PosEntryMode40
        {
            get => Iso8583flds["40"].Value;
            set => Iso8583flds["40"].Value = value;
        }


        public string PosPinCapCode
        {
            get => Iso8583flds["26"].Value;
            set => Iso8583flds["26"].Value = value;
        }


        public PROCESS_CODE ProcessCode
        {
            get => (PROCESS_CODE)Enum.Parse(typeof(PROCESS_CODE), Iso8583flds["3"].Value);
            set => Iso8583flds["3"].Value = ((int)value).ToString();
        }

        public string ReplacementAmount
        {
            get => Iso8583flds["95"].Value;
            set => Iso8583flds["95"].Value = value;
        }

        public string ResponseCode
        {
            get => Iso8583flds["39"].Value;
            set => Iso8583flds["39"].Value = value;
        }

        public long RetrievalRefNo
        {
            get => long.Parse(Iso8583flds["37"].Value);
            set => Iso8583flds["37"].Value = value.ToString();
        }

        public decimal SettlementAmount
        {
            get => decimal.Parse(Iso8583flds["5"].Value);
            set => Iso8583flds["5"].Value = value.ToString();
        }

        public decimal SettlementFee
        {
            get => decimal.Parse(Iso8583flds["29"].Value);
            set => Iso8583flds["29"].Value = value.ToString();
        }

        public decimal SettlementProcFee
        {
            get => decimal.Parse(Iso8583flds["31"].Value);
            set => Iso8583flds["31"].Value = value.ToString();
        }

        public string StlCurrencyCode
        {
            get => Iso8583flds["50"].Value;
            set => Iso8583flds["50"].Value = value;
        }

        public string TerminalType
        {
            get => Iso8583flds["124"].Value;
            set => Iso8583flds["124"].Value = value;
        }

        public long TraceAuditNo
        {
            get => long.Parse(Iso8583flds["11"].Value);
            set => Iso8583flds["11"].Value = value.ToString("000000");
        }

        public string Track2Data
        {
            get => Iso8583flds["35"].Value;
            set => Iso8583flds["35"].Value = value;
        }

        public string ChannelId
        {
            get => Iso8583flds["126"].Value;
            set => Iso8583flds["126"].Value = value;
        }
        
        public string TransactionAmount
        {
            get => Iso8583flds["4"].Value;
            set => Iso8583flds["4"].Value = value;
        }
        
        public string TransactionFee
        {
            get => Iso8583flds["28"].Value;
            set => Iso8583flds["28"].Value = value;
        }
        /// <summary>
        ///  check this property so it is not connect24 dll limiting factor
        /// </summary>
        public string TransmissionDateTime
        {
            get => Iso8583flds["7"].Value;
            set => Iso8583flds["7"].Value = value; // Assuming the format "MMhhmmss" is handled elsewhere
        }

        public string TransportDataCode
        {
            get => Iso8583flds["59"].Value;
            set => Iso8583flds["59"].Value = value;
        }

        public string TxnCurrencyCode
        {
            get => Iso8583flds["49"].Value;
            set => Iso8583flds["49"].Value = value;
        }

        public string Field_95
        {
            get => Iso8583flds["95"].Value;
            set => Iso8583flds["95"].Value = value;
        }

        public string Field_90
        {
            get => Iso8583flds["90"].Value;
            set => Iso8583flds["90"].Value = value;
        }

        public string Field_127002
        {
            get => Iso8583flds["127.002"].Value;
            set => Iso8583flds["127.002"].Value = value;
        }

        public string Field_127003
        {
            get => Iso8583flds["127.003"].Value;
            set => Iso8583flds["127.003"].Value = value;
        }

        public string Field_127006
        {
            get => Iso8583flds["127.006"].Value;
            set => Iso8583flds["127.006"].Value = value;
        }

        public string Field_127011
        {
            get => Iso8583flds["127.011"].Value;
            set => Iso8583flds["127.011"].Value = value;
        }

        public string Field_127020
        {
            get => Iso8583flds["127.020"].Value;
            set => Iso8583flds["127.020"].Value = value;
        }

        public string Field_127026
        {
            get => Iso8583flds["127.026"].Value;
            set => Iso8583flds["127.026"].Value = value;
        }

        public string Field_127012
        {
            get => Iso8583flds["127.012"].Value;
            set => Iso8583flds["127.012"].Value = value;
        }

        public string Field_127022
        {
            get => Iso8583flds["127.022"].Value;
            set => Iso8583flds["127.022"].Value = value;
        }

        public string Field_127042
        {
            get => Iso8583flds["127.042"].Value;
            set => Iso8583flds["127.042"].Value = value;
        }
        //127.025.IccData

        public string Field_127_025_IccData
        {
            get => Iso8583flds["127.025.IccData"].Value;
            set => Iso8583flds["127.025.IccData"].Value = value;
        }

        public void InitiateIsoFields()
        {
            Iso8583flds = new Dictionary<string, Iso8583Field>(128);

            for (int i = 0; i < 128; i++)
            {
                Iso8583flds.Add(i.ToString(), null!);
                if (i == 127)
                {
                    for (int j = 0; i < 128; i++)
                    {
                        string jString = j.ToString();
                        if (jString.Length == 1)
                        {
                            jString = "00" + jString;
                        }
                        if (jString.Length == 2)
                        {
                            jString = "0" + jString;
                        }

                        Iso8583flds.Add("127." + jString, null);
                    }

                }
            }

            Iso8583flds.Add("127.025.IccData", null);


            Iso8583flds["2"] = new Iso8583Field("Primary AccountNumber No", "LLVAR", 19);
            Iso8583flds["3"] = new Iso8583Field("Process Code", "N", 6);
            Iso8583flds["4"] = new Iso8583Field("Txn Amount", "N", 12);
            Iso8583flds["5"] = new Iso8583Field("Stl Amount", "N", 16);
            Iso8583flds["7"] = new Iso8583Field("Transmission Date Time", "N", 10);
            Iso8583flds["11"] = new Iso8583Field("Trace Number", "N", 6);
            Iso8583flds["12"] = new Iso8583Field("Transmission Date Time", "N", 6);
            Iso8583flds["13"] = new Iso8583Field("Txn Local Date", "N", 4);
            Iso8583flds["14"] = new Iso8583Field("Stl Date", "N", 4);
            Iso8583flds["15"] = new Iso8583Field("Expiry Date", "N", 4);
            Iso8583flds["17"] = new Iso8583Field("Date Capture", "DTS", 8);
            Iso8583flds["18"] = new Iso8583Field("Merchant Type", "N", 4);
            Iso8583flds["22"] = new Iso8583Field("Pos Entry Mode", "N", 3);
            Iso8583flds["23"] = new Iso8583Field("Field 23", "N", 3);
            Iso8583flds["24"] = new Iso8583Field("Function Code", "N", 3);
            Iso8583flds["25"] = new Iso8583Field("Settlement Amount", "N", 2);
            Iso8583flds["26"] = new Iso8583Field("Pos Pin Cap Code", "N", 2);
            Iso8583flds["28"] = new Iso8583Field("Txn Fee", "X+N", 9);
            Iso8583flds["29"] = new Iso8583Field("Stl Fee", "AMT", 8);
            Iso8583flds["30"] = new Iso8583Field("Original Amounts", "X+N", 9);
            Iso8583flds["31"] = new Iso8583Field("Stl Processing Fee", "AMT", 8);
            Iso8583flds["32"] = new Iso8583Field("Acquiring Inst ID", "LLVAR", 11);
            Iso8583flds["33"] = new Iso8583Field("Fowarder Inst ID", "LLVAR", 11);
            Iso8583flds["34"] = new Iso8583Field("Extended PAN", "LLVAR", 28);
            Iso8583flds["35"] = new Iso8583Field("Track 2", "LLVAR", 37);
            Iso8583flds["37"] = new Iso8583Field("Retrieval Ref No", "ANP", 12);
            Iso8583flds["38"] = new Iso8583Field("Approval Code", "AN", 6);
            Iso8583flds["39"] = new Iso8583Field("Response Code", "AN", 2);
            Iso8583flds["40"] = new Iso8583Field("Field 40", "AN", 3);
            Iso8583flds["41"] = new Iso8583Field("Terminal ID", "ANS", 8);
            Iso8583flds["42"] = new Iso8583Field("Card Acceptor ID", "ANS", 15);
            Iso8583flds["43"] = new Iso8583Field("Card Acceptor Name", "ANS", 40);
            Iso8583flds["46"] = new Iso8583Field("Amount Fees", "FEE", 300);
            Iso8583flds["48"] = new Iso8583Field("Additional Data", "LLLVAR", 999);
            Iso8583flds["49"] = new Iso8583Field("Txn Ccy Code", "A/N", 3);
            Iso8583flds["50"] = new Iso8583Field("Stl Ccy Code", "LLLVAR", 3);
            Iso8583flds["54"] = new Iso8583Field("AccountNumber Balance Amounts", "LLLVAR", 120);
            Iso8583flds["56"] = new Iso8583Field("Original Data Elements", "LLLVAR", 4);
            Iso8583flds["59"] = new Iso8583Field("Transport data", "LLLVAR", 500);
            Iso8583flds["66"] = new Iso8583Field("Original Amount Fees", "FEE", 300);
            Iso8583flds["70"] = new Iso8583Field("Network Code", "N", 3);
            Iso8583flds["90"] = new Iso8583Field("Field 90", "N", 42);
            Iso8583flds["93"] = new Iso8583Field("Field 93", "LLVAR", 11);
            Iso8583flds["94"] = new Iso8583Field("Field 94", "LLVAR", 11);
            Iso8583flds["95"] = new Iso8583Field("Field 95", "AN*", 42);
            Iso8583flds["102"] = new Iso8583Field("AccountNumber Iden 1", "LLVAR", 28); //account ID
            Iso8583flds["103"] = new Iso8583Field("AccountNumber Iden 2", "LLVAR", 28);
            Iso8583flds["123"] = new Iso8583Field("POS Data Code", "LLLVAR", 15);
            Iso8583flds["124"] = new Iso8583Field("Terminal Type", "AN", 3);
            Iso8583flds["125"] = new Iso8583Field("Field 125", "LLLVAR", 999);
            Iso8583flds["126"] = new Iso8583Field("Field 126", "LLLVAR", 999);
            // this.Iso8583flds["127"] = new Iso8583Field("Field 127", "ANS", 999);
            Iso8583flds["127.002"] = new Iso8583Field("Field 127.002", "ANS", 10);
            Iso8583flds["127.003"] = new Iso8583Field("Field 127.003", "ANS*", 48);
            Iso8583flds["127.020"] = new Iso8583Field("Field 127.020", "N", 8);
            Iso8583flds["127.006"] = new Iso8583Field("Field 127.006", "AN", 2);
            Iso8583flds["127.011"] = new Iso8583Field("Field 127.011", "ANS", 10);
            Iso8583flds["127.026"] = new Iso8583Field("Field 127.026", "ANS", 9);
            Iso8583flds["127.012"] = new Iso8583Field("Field 127.012", "ANS", 3);
            Iso8583flds["127.022"] = new Iso8583Field("Field 127.022", "ANS", 14);
            Iso8583flds["127.042"] = new Iso8583Field("Field 127.026", "N", 20);
            Iso8583flds["127.025.IccData"] = new Iso8583Field("127.025.IccData", "ANS", 1202);



        }

        public Iso8583Msg()
        {
            InitiateIsoFields();
        }
        
        public static decimal ConvertToISOAmount(decimal dec_amt)
        {
            return dec_amt * new decimal(100);
        }

        private void Decode(byte[] iso_msg)
        {
            int num;
            int i;
            const int num1 = 128;
            try
            {
                if (iso_msg[0] * 256 + iso_msg[1] < 22)
                {
                    throw new InvalidOperationException("Message Decode Failed: Insufficient length");
                }

                byte[] numArray = new byte[16];
                Array.Copy(iso_msg, 6, numArray, 0, 16);

                BitArray availableFields = GetAvailableFields(numArray);
                string str = Encoding.ASCII.GetString(iso_msg, 22, iso_msg.Length - 22);
                int num2 = 0;

                for (i = 0; i < num1 - 1; i++)
                {
                    if (availableFields.Get(i))
                    {
                        num = i + 1;
                        string numString = num.ToString();

                        if (Iso8583flds.TryGetValue(numString, out var field) && field != null)
                        {
                            Console.WriteLine($"Decoding field {num} with format: {field.Format}");
                            string format = field.Format;

                            if (!string.IsNullOrEmpty(format))
                            {
                                int length = field.Length; // Initialize length with the predefined field length

                                switch (format)
                                {
                                    case "LLVAR":
                                        length = int.Parse(str.Substring(num2, 2));
                                        num2 += 2;
                                        break;

                                    case "LLLVAR":
                                        length = int.Parse(str.Substring(num2, 3));
                                        num2 += 3;
                                        break;

                                    case "FEE":
                                        length = int.Parse(str.Substring(num2, 3));
                                        num2 += 3;
                                        break;
                                }

                                field.Value = str.Substring(num2, length);
                                num2 += length;
                            }
                            else
                            {
                                Console.WriteLine($"Warning: Unrecognized format for field {numString}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or log it, with more context
                throw new Exception($"Error decoding ISO message: {ex.Message}", ex);
            }
        }
        private BitArray GetAvailableFields(byte[] bitmap)
        {
            var stringBuilder = new StringBuilder();
            int num = 128;

            // Convert each byte to a binary string and append to the StringBuilder
            foreach (byte b in bitmap)
            {
                stringBuilder.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            string str = stringBuilder.ToString();
            Console.WriteLine($"String of available fields: {str}");

            BitArray bitArrays = new BitArray(num);
            for (int j = 0; j < num; j++)
            {
                bitArrays.Set(j, str[j] == '1');
            }

            return bitArrays;
        }

        private byte[] Encode(Iso8583Msg MsgToEncode)
        {
            Iso8583Field item;
            int length;
            string value = "";
            string str = "";

            BitArray bitArrays = new BitArray(128, false);
            bitArrays.Set(0, true); // Primary bitmap
            bitArrays.Set(1, true); // Secondary bitmap (if used)

            try
            {
                for (int i = 2; i < 128; i++)
                {
                    string iString = i.ToString();
                    item = MsgToEncode.Iso8583flds[iString];

                    if (item != null && !string.IsNullOrEmpty(item.Value))
                    {
                        bitArrays.Set(i, true);

                        if (item.Value.Length > item.Length)
                        {
                            item.Value = item.Value.Substring(0, item.Length);
                        }

                        string format = item.Format;
                        switch (format)
                        {
                            case "LLVAR":
                                length = item.Value.Length;
                                value = length.ToString().PadLeft(2, '0') + item.Value;
                                break;

                            case "LLLVAR":
                            case "FEE":
                                length = item.Value.Length;
                                value = length.ToString().PadLeft(3, '0') + item.Value;
                                break;

                            case "AN":
                            case "ANS":
                            case "X+N":
                                value = item.Value.PadRight(item.Length, ' ');
                                break;

                            case "N":
                            case "Z":
                            case "ANP":
                            case "A/N":
                            case "AN*":
                            case "ANS*":
                                value = item.Value.PadLeft(item.Length, '0');
                                break;

                            case "DTL":
                            case "DTS":
                                value = item.Value;
                                break;

                            default:
                                throw new Exception($"Unsupported format: {format}");
                        }

                        str += value;
                    }
                }

                Console.WriteLine($"Encoded ISO8583 Message String: {str}");

                ushort totalLength = (ushort)(str.Length + 22);
                byte[] numArray = new byte[totalLength];

                Console.WriteLine("Message Type: " + MsgToEncode.MessageType);
                byte[] messageTypeBytes = Encoding.ASCII.GetBytes(MsgToEncode.MessageType);

                Console.WriteLine("Message Type Bytes:");
                foreach (var b in messageTypeBytes)
                {
                    Console.WriteLine($"={b}=");
                }

                byte[] bitmapBytes = GenerateBitmap(bitArrays);
                byte[] messageBytes = Encoding.ASCII.GetBytes(str);

                int lengthWithoutHeader = totalLength - 2;

                if (lengthWithoutHeader > 256)
                {
                    numArray[0] = (byte)(lengthWithoutHeader / 256);
                    lengthWithoutHeader %= 256;
                }

                numArray[1] = (byte)lengthWithoutHeader;
                numArray[2] = 49;

                Array.Copy(messageTypeBytes, 0, numArray, 2, messageTypeBytes.Length);
                Array.Copy(bitmapBytes, 0, numArray, 6, bitmapBytes.Length);
                Array.Copy(messageBytes, 0, numArray, 22, messageBytes.Length);

                return numArray;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception during encoding: {exception.Message}; Inner Exception: {exception.InnerException?.Message}");
                throw;
            }
        }

        private byte[] GenerateBitmap(BitArray avb_flds_bits)
        {
            int numBytes = 16;
            int bitsPerByte = 8;
            StringBuilder bitmapString = new StringBuilder(avb_flds_bits.Length);

            // Build the bitmap string from the BitArray
            for (int i = 0; i < avb_flds_bits.Length; i++)
            {
                bitmapString.Append(avb_flds_bits[i] ? "1" : "0");
            }

            // Remove the unnecessary substring operation and padding logic
            // Convert the 128-bit string directly to a byte array
            byte[] bitmapBytes = new byte[numBytes];
            for (int i = 0; i < numBytes; i++)
            {
                string byteString = bitmapString.ToString(i * bitsPerByte, bitsPerByte);
                bitmapBytes[i] = BinToByte(byteString);
            }

            return bitmapBytes;
        }

        private byte BinToByte(string bin_val)
        {
            return Convert.ToByte(bin_val, 2);
        }

        public static string GetResponseMsg(string msg_code)
        {
            // Define the dictionary with the message codes and their corresponding messages
            var responseMessages = new Dictionary<string, string>
            {
                { "000", "Approved" },
                { "111", "Invalid Scheme type" },
                { "114", "Invalid account number" },
                { "115", "Func not supported" },
                { "116", "Insufficient funds" },
                { "119", "Txn not permitted" },
                { "121", "wdr limit exceeded" },
                { "163", "Inv Chq Status" },
                { "180", "Tfr Limit Exceeded" },
                { "181", "Chqs in different books" },
                { "182", "Not all chqs could be stopped" },
                { "183", "chqs not issued to this account" },
                { "184", "AccountNumber is closed" },
                { "185", "Inv Txn Currency or Amount" },
                { "186", "Block does not exist" },
                { "187", "Cheque Stopped" },
                { "188", "Invalid Rate Currency Combination" },
                { "189", "Cheque Book Already Issued" },
                { "190", "DD Already Paid" },
                { "800", "Network message was accepted" },
                { "902", "Invalid Txn" },
                { "904", "Format Error" },
                { "906", "Cut-over in progress" },
                { "907", "Card issuer inoperative" },
                { "909", "Malfunction" },
                { "911", "Card issuer timed out" },
                { "913", "Duplicate transmission" },
                { "419", "FAILED" }
            };

            // Attempt to get the response message; if not found, return "Unknown Msg Code"
            return responseMessages.TryGetValue(msg_code, out string response) ? response : "Unknown Msg Code";
        }


        private void HexDumpMsg(byte[] iso_msg)
        {
            try
            {
                int num = 16; // Number of bytes per line
                int length = iso_msg.Length;
                Trace.WriteLine($"HexDumpMsg Size= {length}");

                for (int num1 = 0; num1 < length; num1 += num)
                {
                    // Print hex values
                    for (int num2 = 0; num2 < num && (num1 + num2) < length; num2++)
                    {
                        Trace.Write($"{iso_msg[num1 + num2]:X2} ");
                    }

                    // Print ASCII representation
                    int remaining = Math.Min(num, length - num1);
                    string str = Encoding.ASCII.GetString(iso_msg, num1, remaining);
                    str = str.Replace('\0', '.'); // Replace null characters with dots

                    // Add padding for the last line if it's shorter than 'num' bytes
                    if (remaining < num)
                    {
                        Trace.Write(new string(' ', 3 * (num - remaining)));
                    }

                    Trace.WriteLine(str);
                }

                Trace.Flush();
            }
            catch (Exception exception)
            {
                // Handle or log the exception as needed
                Trace.WriteLine($"Exception in HexDumpMsg: {exception.Message}");
                throw;
            }
        }

        public void Send(Socket sckt, Iso8583Msg iso8583Msg)
        {
            try
            {
                // Encode the ISO 8583 message into a byte array
                byte[] numArray = Encode(iso8583Msg);
                Console.WriteLine("Completed encoding");

                // Send the encoded message via the socket
                sckt.Send(numArray, 0, numArray.Length, SocketFlags.None);
            }
            catch (Exception exception)
            {
                // Log the exception details
                Console.WriteLine($"Exception happened during send: {exception.Message} ; Inner exception: {exception.InnerException?.Message}");

                // Close the socket if an error occurs
                sckt.Close();
                throw;
            }
        }

        public string TextDumpMsgResponse(byte[] iso_msg, out Iso8583Msg iso8583Msg, out string messageType)
        {
            string res = "";
            iso8583Msg = new Iso8583Msg();
            messageType = string.Empty;

            try
            {
                int length = iso_msg.Length;
                Console.WriteLine($"iso_msg.Length : {length}");

                // Extract the message type from the ISO message
                byte[] numArrayTop = new byte[16];
                Array.Copy(iso_msg, 0, numArrayTop, 0, 15);
                string utfString = Encoding.UTF8.GetString(numArrayTop, 0, numArrayTop.Length);
                Console.WriteLine($"UTF String for message code : {utfString}");
                messageType = utfString.Substring(2, 4);
                Console.WriteLine($"Extracted message type: {messageType}");

                // Decode the ISO message
                iso8583Msg.Decode(iso_msg);
                Console.WriteLine($"Number of ISO fields decoded: {iso8583Msg.Iso8583flds.Count}");

                // Loop through the decoded fields
                for (int i = 0; i < iso8583Msg.Iso8583flds.Count; i++)
                {
                    string iString = i.ToString();

                    // Special case for field 127, which may have subfields
                    if (i == 127)
                    {
                        for (int j = 0; j < 128; j++)
                        {
                            string jString = j.ToString("D3");
                            iString = $"127.{jString}";

                            if (iso8583Msg.Iso8583flds.ContainsKey(iString) && iso8583Msg.Iso8583flds[iString]?.Value != null)
                            {
                                Console.WriteLine($"Field {iString}: {iso8583Msg.Iso8583flds[iString].Value}");
                                res = $"{res}: {iString} {iso8583Msg.Iso8583flds[iString].Value}";
                            }
                        }
                    }
                    else if (iso8583Msg.Iso8583flds.ContainsKey(iString) && iso8583Msg.Iso8583flds[iString]?.Value != null)
                    {
                        Console.WriteLine($"Field {iString}: {iso8583Msg.Iso8583flds[iString].Value}");
                        res = $"{res}: {iString} {iso8583Msg.Iso8583flds[iString].Value}";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in TextDumpMsgResponse: {ex.Message}");
            }

            return res;
        }

        private string TextDumpMsgResponse(byte[] iso_msg)
        {
            string res = "";
            try
            {
                int length = iso_msg.Length;
                Console.WriteLine($"iso_msg.Length : {length}");

                Iso8583Msg iso8583Msg = new Iso8583Msg();
                iso8583Msg.Decode(iso_msg);

                for (int i = 0; i < iso8583Msg.Iso8583flds.Count; i++)
                {
                    string iString = i.ToString();
                    if (iso8583Msg.Iso8583flds.ContainsKey(iString) && iso8583Msg.Iso8583flds[iString]?.Value != null)
                    {
                        Console.WriteLine($"Field {iString}: {iso8583Msg.Iso8583flds[iString].Value}");
                        res = $"{res}: {iString} {iso8583Msg.Iso8583flds[iString].Value}";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in TextDumpMsgResponse: {ex.Message}");
            }

            return res;
        }

        private void TextDumpMsg(byte[] iso_msg)
        {
            try
            {
                int length = iso_msg.Length;
                Console.WriteLine($"iso_msg.Length : {length}");

                Iso8583Msg iso8583Msg = new Iso8583Msg();
                iso8583Msg.Decode(iso_msg);

                for (int i = 0; i < iso8583Msg.Iso8583flds.Count; i++)
                {
                    string iString = i.ToString();
                    if (iso8583Msg.Iso8583flds.ContainsKey(iString) && iso8583Msg.Iso8583flds[iString]?.Value != null)
                    {
                        Console.WriteLine($"Field {iString}: {iso8583Msg.Iso8583flds[iString].Value}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in TextDumpMsg: {ex.Message}");
            }
        }
    }

    public enum FUNCTION_CODE
    {

        NormalRequest = 200,
        NormalResponse = 210,
        NormalAdvice = 220,
        NormalRepeat = 221,
        ReversalRequest = 400,
        ReversalAdvice = 420,
        ReversalRepeat = 421,
        ReversalResponse = 430,
        NetworkEchoTest = 831
    }
    
    public enum PROCESS_CODE
    {
        NormalPurchaseCA = 1000,
        NormalPurchaseSA = 2000,
        CashWithdrawalCA = 11000,
        CashWithdrawalSA = 12000,
        BalanceInquiryCA = 311000,
        BalanceInquirySA = 312000,
        MiniStatementCA = 381000,
        MiniStatementSA = 382000,
        FundTransferCA = 401010,
        FundTransferSA = 402010,
        FundTransferCACA = 402020,
        GeneralAccountInq = 821000,
        FullStatementCA = 931000,
        FullStatementSA = 932000,
        NormalAccountInq = 970000,
        FundTransferCACAReversal = 501010
    }
}
