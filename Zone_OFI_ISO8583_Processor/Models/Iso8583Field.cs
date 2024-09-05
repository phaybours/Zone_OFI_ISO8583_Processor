namespace Zone_OFI_ISO8583_Processor.Models
{
    public class Iso8583Field
    {
        public string Format { get; }
        public int Length { get; }
        public string Name { get; }
        public string Value { get; set; }
        public Iso8583Field(string name, string format, int length)
        {
            Name = name;
            Format = format;
            Length = length;
        }
        public override string ToString()
        {
            return Value;
        }
    }
}
