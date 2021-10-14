namespace KafkaCurator.Core.Constants
{
    public static class EnvironmentVariableName
    {
        public static string EnvironmentPrefix = "ASPNETCORE_";
        public static string HostEnvironment = $"{EnvironmentPrefix}ENVIRONMENT";
        public static string EnvironmentArgumentName = "env";
    }
}