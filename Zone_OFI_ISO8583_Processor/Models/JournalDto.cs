using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zone_OFI_ISO8583_Processor.Models
{
    public class JournalDto
    {
        public string Rrn { get; set; }
        public string Stan { get; set; }
        public string AcquirerBank { get; set; }
        public int Amount { get; set; }
        public string AccountNumber { get; set; }
        public string Pan { get; set; }
        public string TransactionStatus { get; set; }
        public string CurrencyCode { get; set; }
        public string Comment { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionTime { get; set; }
        public string Error { get; set; }
        public string TerminalId { get; set; }

        public static JournalDto FromArray(string[] data)
        {
            if (data.Length < 13) // Validation to ensure the array has enough elements for journal object
            {
                Console.WriteLine("Transaction or Reversal Result length is insufficient to populate JournalDto");
                return null;
            }

            return new JournalDto
            {
                Rrn = data[37],
                Stan = data[11],
                AcquirerBank = data[32],
                Amount = int.TryParse(data[4], out var amount) ? amount : 0, // Parse integer for Amount
                AccountNumber = data[102],
                Pan = data[2],
                TransactionStatus = data[39],
                CurrencyCode = data[49],
                Comment = data[123],
                TransactionDate = data[12],
                TransactionTime = data[13],
                Error = data[39],
                TerminalId = data[12]
            };
        }
    }
}
