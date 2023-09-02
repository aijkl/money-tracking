using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit.Text;
using MessageSummaryItems = MailKit.MessageSummaryItems;

namespace Aijkl.MoneyTracker;

public class MoneyTracker
{
    private readonly string _hostname;
    private readonly string _mailAddress;
    private readonly string _mailPassword;
    private readonly string _senderAddress;
    private readonly int _thresholdYen;
    private readonly List<INotifyService> _notifyServices;

    public MoneyTracker(string hostname, string mailAddress, string mailPassword, string senderAddress, int thresholdYen, List<INotifyService> notifyServices)
    {
        _hostname = hostname;
        _mailAddress = mailAddress;
        _mailPassword = mailPassword;
        _senderAddress = senderAddress;
        _thresholdYen = thresholdYen;
        _notifyServices = notifyServices;
    }
    
    public async Task RunAsync(DateTime since)
    {
        using var client = new ImapClient();
        await client.ConnectAsync(_hostname, 993, true);
        await client.AuthenticateAsync(_mailAddress, _mailPassword);

        var inbox = client.Inbox;
        await inbox.OpenAsync(FolderAccess.ReadWrite);

        var until = DateTime.Now;
        var ids = await inbox.SearchAsync(SearchQuery.FromContains(_senderAddress).And(SearchQuery.SentSince(since).And(SearchQuery.SentOn(until))));
        var summaries = await inbox.FetchAsync(ids, MessageSummaryItems.UniqueId | MessageSummaryItems.Body | MessageSummaryItems.Flags);
        
        if(summaries.Any(x => x.Flags != null && ((MessageFlags)x.Flags!).HasFlag(MessageFlags.Seen) is false) is false) // 処理不要
        {
            return;
        }
        
        var messages = ids.Select(x => inbox.GetMessage(x));
        var details = messages.Select(x => MailParser.TryParseWithdrawal(x.GetTextBody(TextFormat.Plain))).Where(x => x != null).Select(x => x!).ToList();
        if (details.Sum(x => x.Amount) >= _thresholdYen)
        {
            foreach (var notifyService in _notifyServices)
            {
                notifyService.Notify(details, since, until);
            }
        }
        
        foreach (var summary in summaries.Where(x => x.Flags?.HasFlag(MessageFlags.Seen) is false))
        {
            await inbox.AddFlagsAsync(summary.UniqueId, MessageFlags.Seen, true);
        }
    }
}