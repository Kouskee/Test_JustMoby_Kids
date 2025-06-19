using System;
using UniRx;

public class NotificationService : INotificationService
{
    private readonly Subject<string> _onMessageShown = new Subject<string>();

    public IObservable<string> OnMessageShown => _onMessageShown;

    public void ShowMessage(string messageKey)
    {
        _onMessageShown.OnNext(messageKey);
    }
}