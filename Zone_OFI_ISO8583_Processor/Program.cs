using Microsoft.Data.Sqlite;
using System.Transactions;
using System.Xml.Linq;
using System;
using Zone_OFI_ISO8583_Processor.Models;
using Zone_OFI_ISO8583_Processor.Utilities;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.TimeZoneInfo;

internal class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Data Source=KeyVault.db;";
        CreateDB(connectionString);

        bool continueRunning = true;

        while (continueRunning)
        {
            Console.Clear();
            Console.WriteLine("==== Main Menu ====");
            Console.WriteLine("1. Set ZMK");
            Console.WriteLine("2. Send Echo Message");
            Console.WriteLine("3. Send Key Exchange Request Message");
            Console.WriteLine("4. Send Transaction");
            Console.WriteLine("5. Send Reversal");
            Console.WriteLine("6. Decrypt ZPK");
            Console.WriteLine("7. Send Push Journal Rest call");
            Console.WriteLine("8. Test PIN Block Generation");
            Console.WriteLine("9. Quit");
            Console.WriteLine("===================");
            Console.Write("Please select an option (1-9): ");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    SetZMK(connectionString);
                    break;
                case "2":
                    SendEchoMessage();
                    break;
                case "3":
                    SendKeyExchangeRequestMessage(connectionString);
                    break;
                case "4":
                    SendFinancialMessage(connectionString);
                    break;
                case "5":
                    SendReversalMessage(connectionString);
                    break;
                case "6":
                    DecryptZPK();
                    break;
                case "7":
                    SendPushJournalIntegration();
                    break;
                case "8":
                    TestPinBlockGeneration(connectionString);
                    break;
                case "9":
                    continueRunning = false;
                    Console.WriteLine("Exiting the application...");
                    break;
                default:
                    Console.WriteLine("Invalid selection. Please try again.");
                    break;
            }

            if (continueRunning)
            {
                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    static void SetZMK(string connectionString)
    {
        Console.WriteLine("======Enter First and Second Component of ZMK Respectively======");
        Console.Write("ZMK First Component: ");
        string zmkFirstComponent = Console.ReadLine();
        Console.Write("ZMK Second Component: ");
        string zmkSecondComponent = Console.ReadLine();
        string ZMKRes = XORHelper.XorHexStrings(zmkFirstComponent, zmkSecondComponent);

        var connection = new SqliteConnection(connectionString);
        connection.Open();

        var zmkValues = new KeyVault
        {
            ZMK = ZMKRes,
            ZMK_Date = DateTime.Now,
        };


        Console.WriteLine($"Result: {ZMKRes}");

        SQLite.UpdateZMK(connection, zmkValues);
    }

    static void SendEchoMessage()
    {
        Console.WriteLine("==========Initiating Echo Message==========");
        //ISO8583Processing.SendEchoMessage();
        ISO8583MessageProcessingTrx iso8583Proc = new ISO8583MessageProcessingTrx();
        iso8583Proc.SendEchoMessage();
    }

    static void SendKeyExchangeRequestMessage(string connectionString)
    {
        Console.WriteLine("====== Initiating Key Exchange Message ========");
        var connection = new SqliteConnection(connectionString);
        connection.Open();
        ISO8583MessageProcessingTrx iso8583Proc = new ISO8583MessageProcessingTrx();
        iso8583Proc.SendKeyRequestMessage(connection);
    }

    static void SendFinancialMessage(string connectionString)
    {
        Console.WriteLine("==========Enter PIN and PAN Below==========");
        Console.Write("Enter PAN: ");
        string pan = Console.ReadLine();
        Console.Write("Enter PIN: ");
        string pin = Console.ReadLine();

        var connection = new SqliteConnection(connectionString);
        connection.Open();
        ISO8583MessageProcessingTrx iso8583Proc = new ISO8583MessageProcessingTrx();
        iso8583Proc.SendFinancialMessage(pin, pan, connection);
    }

    static void SendPushJournalIntegration()
    {
        var journalDto = new JournalDto
        {
            Rrn = "000210007849",
            Stan = "120301",
            AcquirerBank = "107",
            Amount = 1000, 
            AccountNumber = "12345643234",
            Pan = "539983******9398",
            TransactionStatus = "APPROVED",
            CurrencyCode = "566",
            Comment = "THE TRANSACTION WAS SUCCESSFULLY COMPLETED",
            TransactionDate = "11/09/2023",
            TransactionTime = "16:43",
            Error = string.Empty,
            TerminalId = "2076ES85"
        };
        ISO8583Processing.PostJournal(journalDto);
    }

    static void SendReversalMessage(string connectionString)
    {
        Console.WriteLine("==========Enter PIN and PAN Below==========");
        Console.Write("Enter PAN: ");
        string pan = Console.ReadLine();
        Console.Write("Enter PIN: ");
        string pin = Console.ReadLine();

        var connection = new SqliteConnection(connectionString);
        connection.Open();
        ISO8583Processing.SendReversalMessage(pin, pan, connection);
    }

    static void DecryptZPK()
    {
        string clearZPK = string.Empty, generatedKCV = string.Empty, valKCV = string.Empty;
        Console.Write("Enter ZMK: ");
        string zmk = Console.ReadLine();
        Console.Write("Enter Encrypted ZPK: ");
        string zpk = Console.ReadLine();

        var result = ZPKSessionKeyHandler.DecryptZPKSessionKey(zmk, zpk);
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
            //SQLite.UpdateZPK(connection, values);
        }
        else
        {
            Console.WriteLine("=============Error: KCV validation failed =============");
        }

        Console.WriteLine("==========Done=========");
    }

    private static void TestPinBlockGeneration(string connectionString)
    {
        Console.WriteLine("==========Enter PIN and PAN Below==========");
        Console.Write("Enter PAN: ");
        string pan = Console.ReadLine();
        Console.Write("Enter PIN: ");
        string pin = Console.ReadLine();

        var connection = new SqliteConnection(connectionString);
        connection.Open();
        var encryptedPINBlock = ISO8583Processing.GenerateEncryptedPinBlock(pin, pan, connection);
        Console.WriteLine($"Encrypted PIN Block:-> {encryptedPINBlock}");
    }

    static void CreateDB(string connectionString)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            // Create the KeyVault table
            SQLite.CreateTable(connection);
            //Insert sample keys
            var initialKeyVault = new KeyVault
            {
                ZMK = "63E4880A2D502DD8E835C68DD8061BBB",
                ZPK = "1A6B0B08DFC7AB4AC7153E524C07643D",
                ZMK_Date = DateTime.Now,
                ZPK_Date = DateTime.Now
            };

            SQLite.InsertOrUpdateKeyVault(connection, initialKeyVault);

        }
    }


}
