using Discord;
using Discord.Webhook;
using Spectre.Console;

namespace Aijkl.MoneyTracker.NotifyServices;

public class DiscordNotifyService : INotifyService
{
    private readonly IEnumerable<string> _webhookUrls;

    public DiscordNotifyService(IEnumerable<string> webhookUrls)
    {
        _webhookUrls = webhookUrls;
    }

    public async Task Notify(List<WithdrawalDetail> details, DateTime since, DateTime until)
    {
        var embed = new EmbedBuilder
        {
            Title = "怪しい出金を検出しました :man_police_officer:",
            Description = $"期間: {since:yyyy-m-d} 〜 {until:yyyy-m-d}\n合計: {details.Sum(x => x.Amount)} 円"
        };
        foreach (var detail in details)
        {
            embed.AddField(detail.Description, $"{detail.Amount} 円\n{detail.Date}");
        }
    
        foreach (var webhookUrl in _webhookUrls)
        {
            try
            {
                using var client = new DiscordWebhookClient(webhookUrl);
                await client.SendMessageAsync(embeds: new[] { embed.Build() });
            }
            catch(Exception e)
            {
                AnsiConsole.WriteException(e);
            }
        }
    }
}