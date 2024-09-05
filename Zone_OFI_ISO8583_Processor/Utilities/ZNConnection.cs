using Microsoft.Extensions.Configuration;
using Zone_OFI_ISO8583_Processor.Models;

namespace Zone_OFI_ISO8583_Processor.Utilities
{
    internal static class ZNConnection 
    {
        public static AppSettings GetDetiails()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) 
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) 
                .Build();

            // Binding configuration to AppSettings Object
            return configuration.GetSection("AppSettings").Get<AppSettings>();
        }
    }
}
