using Aijkl.MoneyTracker.Commands;
using Spectre.Console.Cli;

namespace Aijkl.MoneyTracker;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var commandApp = new CommandApp();
        commandApp.Configure(x =>
        {
            x.AddCommand<DaemonCommand>("daemon").WithDescription("Starts the daemon");
        });
        return await commandApp.RunAsync(args);
    }
}