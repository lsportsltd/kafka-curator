namespace KafkaCurator.Abstractions.Configuration
{
    public class CleanupPolicy : Enumeration
    {
        public static CleanupPolicy Delete = new CleanupPolicy("delete");
        public static CleanupPolicy Compact = new CleanupPolicy("compact");

        private CleanupPolicy(string name) : base(name)
        {
        }
    }
}