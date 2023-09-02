namespace Aijkl.MoneyTracker;

public interface INotifyService
{
    public void Notify(List<WithdrawalDetail> details, DateTime since, DateTime until);
}