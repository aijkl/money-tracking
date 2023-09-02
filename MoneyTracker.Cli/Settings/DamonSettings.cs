using Spectre.Console;
using Spectre.Console.Cli;

namespace Aijkl.MoneyTracker.Settings;

public class DamonSettings : CommandSettings
{
    public DamonSettings(string hostname, string mailAddress, string password, string senderAddress, int thresholdYen, int dayCount)
    {
        Hostname = hostname;
        MailAddress = mailAddress;
        Password = password;
        SenderAddress = senderAddress;
        ThresholdYen = thresholdYen;
        DayCount = dayCount;
    }

    [CommandArgument(0, "<HOSTNAME>")]
    public string Hostname { get; set; }
    
    [CommandArgument(1, "<MAIL_ADDRESS>")]
    public string MailAddress { get; set; }
    
    [CommandArgument(2, "<PASSWORD>")]
    public string Password { get; set; }
    
    [CommandArgument(3, "<SENDER_ADDRESS>")]
    public string SenderAddress { get; set; }
    
    [CommandArgument(4, "<THRESHOLD_YEN>")]
    public int ThresholdYen { get; set; }
    
    [CommandArgument(5, "<DAY_COUNT>")]
    public int DayCount { get; set; }
    
    [CommandArgument(6, "<INTERVAL_MS>")]
    public int IntervalMs { get; set; }

    // TODO Validate
}