namespace Shared.Core.Notification;

public class Notification
{
    public string Key { get; }
    public string Message { get; }

    private Notification(string key, string message)
    {
        Key = key;
        Message = message;
    }

    public static Notification Create(string key, string message) => new(key, message);
}