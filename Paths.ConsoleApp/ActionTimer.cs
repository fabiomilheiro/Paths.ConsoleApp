using System;
using System.Diagnostics;

namespace Paths.ConsoleApp
{
    public class ActionTimer
    {
        public void Execute(string actionName, Action action)
        {
            var stopwatch = Stopwatch.StartNew();

            action();

            stopwatch.Stop();

            Console.WriteLine($"Action '{actionName}' took: {stopwatch.Elapsed}");
        }
    }
}