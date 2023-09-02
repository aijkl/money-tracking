using Aijkl.MoneyTracker.NotifyServices;
using Aijkl.MoneyTracker.Settings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Aijkl.MoneyTracker.Commands;

public class DaemonCommand : AsyncCommand<DamonSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, DamonSettings settings)
    {
        var notifyServices = new List<INotifyService>
        {
            new ConsoleNotifyService()
        };
        
        var moneyTracker = new MoneyTracker(settings.Hostname, settings.MailAddress, settings.Password, settings.SenderAddress, settings.ThresholdYen, notifyServices);
        while (CancellationToken.None.IsCancellationRequested is false)
        {
            try
            {
                await moneyTracker.RunAsync(DateTime.Now.Date.AddDays(-settings.DayCount));
                AnsiConsole.MarkupLine($"Checked. {DateTime.Now}");
                await Task.Delay(settings.IntervalMs);
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
            }
        }
        return 0;
    }
}