namespace KafkaCurator.Abstractions.Configuration
{
    public class CompressionType : Enumeration
    {
        public static CompressionType Zstd = new CompressionType("zstd");
        public static CompressionType Gzip = new CompressionType("gzip");
        public static CompressionType Snappy = new CompressionType("snappy");
        public static CompressionType Lz4 = new CompressionType("lz4");
        public static CompressionType Producer = new CompressionType("producer");

        private CompressionType(string name) : base(name)
        {
        }
    }
}