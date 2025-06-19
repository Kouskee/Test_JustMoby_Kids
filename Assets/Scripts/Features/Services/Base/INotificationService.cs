using System;

public interface INotificationService
{
    void ShowMessage(string messageKey);
    IObservable<string> OnMessageShown { get; }
}