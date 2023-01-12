using System;
using KafkaCurator.Core.Constants;
using Microsoft.Extensions.Hosting;

namespace LSports.Extensions.Kafka.Curator
{
    public static class HostBuilderContextExtensions
    {
        public static void SetEnvironmentName(this HostBuilderContext context)
        {
            var hasEnvironmentArgs = context.Configuration[EnvironmentVariableName.EnvironmentArgumentName];

            //check if there is an environment variable ASPNETCORE_ENVIRONMENT
            var hasEnvironmentVariable = Environment.GetEnvironmentVariable(EnvironmentVariableName.HostEnvironment);

            //sets if these flags are null or empty
            var isNullOrEmptyEnvArgs = string.IsNullOrEmpty(hasEnvironmentArgs);
            var isNullOrEmptyEnvVariable = string.IsNullOrEmpty(hasEnvironmentVariable);

            //first, sets development
            //to avoid mistakes
            var finalEvnName = "dev";

            //if env. args is not empty and env. variable not empty, then take env args
            if (!isNullOrEmptyEnvArgs && isNullOrEmptyEnvVariable)
                finalEvnName = hasEnvironmentArgs;

            //if both not empty, then take args
            if (!isNullOrEmptyEnvArgs && !isNullOrEmptyEnvVariable)
                finalEvnName = hasEnvironmentArgs;

            //if env. args is empty and env. variable not empty, then take env variable
            if (isNullOrEmptyEnvArgs && !isNullOrEmptyEnvVariable)
                finalEvnName = hasEnvironmentVariable;

            //finally, sets the env
            context.HostingEnvironment.EnvironmentName = finalEvnName;
        }
    }
}