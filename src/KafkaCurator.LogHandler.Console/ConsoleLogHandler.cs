using System;
using System.Text.Json;
using KafkaCurator.Abstractions;

namespace KafkaCurator.LogHandler.Console
{
    internal class ConsoleLogHandler : ILogHandler
    {
        public void Error(string message, Exception ex, object data)
        {
            var serializedException = JsonSerializer.Serialize(
                new
                {
                    Type = ex.GetType().FullName,
                    ex.Message,
                    ex.StackTrace,
                });

            Print(
                $"KafkaCurator: {message} | Data: {JsonSerializer.Serialize(data)} | Exception: {serializedException}",
                ConsoleColor.Red);
        }

        public void Error(string message, Exception ex)
        {
            var serializedException = JsonSerializer.Serialize(
                new
                {
                    Type = ex.GetType().FullName,
                    ex.Message,
                    ex.StackTrace,
                });

            Print(
                $"KafkaCurator: {message} | Exception: {serializedException}",
                ConsoleColor.Red);
        }

        public void Warning(string message, object data) => Print(
            $"KafkaCurator: {message} | Data: {JsonSerializer.Serialize(data)}",
            ConsoleColor.Yellow);

        public void Warning(string message) => Print(
            $"KafkaCurator: {message}",
            ConsoleColor.Yellow);

        public void Info(string message, object data) => Print(
            $"KafkaCurator: {message} | Data: {JsonSerializer.Serialize(data)}",
            ConsoleColor.Green);
        
        public void Info(string message) => Print(
            $"KafkaCurator: {message}", ConsoleColor.Green);

        public void Verbose(string message, object data) => Print(
            $"KafkaCurator: {message} | Data: {JsonSerializer.Serialize(data)}",
            ConsoleColor.Blue);

        public void Verbose(string message) => Print(
            $"KafkaCurator: {message}",
            ConsoleColor.Blue);


        private static void Print(string message, ConsoleColor color)
        {
            var colorBefore = System.Console.ForegroundColor;
            System.Console.ForegroundColor = color;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = colorBefore;
        }
    }
}