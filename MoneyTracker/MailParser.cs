using System.Text.RegularExpressions;

namespace Aijkl.MoneyTracker;

public static class MailParser
{
    private static readonly Regex WithdrawalRegex = new Regex("出金日 ： (?<date>.+)\n出金額 ： (?<amount>.+)\n内容 ： (?<description>.+)", RegexOptions.Compiled | RegexOptions.Multiline);
    public static WithdrawalDetail? TryParseWithdrawal(string body)
    {
        try
        {
           return ParseWithdrawal(body);
        }
        catch (Exception)
        {
            return null;
        }
    }
    public static WithdrawalDetail? ParseWithdrawal(string body)
    {
        var match = WithdrawalRegex.Match(body);
        if (match.Success is false) return null;
        
        try
        {
            var date = DateOnly.Parse(match.Groups["date"].Value);
            var amount = int.Parse(match.Groups["amount"].Value.Replace(",", "").Replace("円", ""));
            var description = match.Groups["description"].Value;
            return new WithdrawalDetail(date, amount, description);
        }
        catch (Exception e)
        {
            throw new ArgumentException("body is not withdrawal mail", e);
        }
    }
}