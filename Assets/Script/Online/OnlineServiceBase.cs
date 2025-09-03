using System;

abstract public class OnlineServiceBase
{
    public abstract void Create();

    public abstract void LoginUser(string username, string password, Action onSuccess, Action<string> onFail);
    public abstract void RegisterNewUser(string username, string password, Action onSuccess, Action<string> onFail);
    public abstract void SetUserName(string userName);

    public abstract string GetUserId();

}
