using Spectre.Console;

namespace Aijkl.MoneyTracker.NotifyServices;

public class ConsoleNotifyService : INotifyService
{
    public Task Notify(List<WithdrawalDetail> details, DateTime since, DateTime until)
    {
        AnsiConsole.MarkupLine("怪しい出金がありました");
        AnsiConsole.MarkupLine($"期間: [bold]{since: yyyy/MM/dd}[/] 〜 [bold]{until: yyyy/MM/dd}[/]");
        AnsiConsole.MarkupLine($"合計: [bold]{details.Sum(x => x.Amount)}[/] 円");

        var table = new Table
        {
            Title = new TableTitle("出金通知"),
            Border = TableBorder.Rounded
        };
        table.AddColumn("日付");
        table.AddColumn("金額");
        table.AddColumn("内容");
        foreach (var detail in details)
        {
            table.AddRow(detail.Date.ToString("yyyy/MM/dd"), detail.Amount.ToString(), detail.Description);
        }
        
        AnsiConsole.Write(table);

        return Task.CompletedTask;
    }
}