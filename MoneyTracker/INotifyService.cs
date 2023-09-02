namespace Aijkl.MoneyTracker;

public interface INotifyService
{
    public Task Notify(List<WithdrawalDetail> details, DateTime since, DateTime until);
}