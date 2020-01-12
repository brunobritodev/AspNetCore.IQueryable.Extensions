namespace RESTFul.Api.Notification
{
    public interface IDomainNotificationMediatorService
    {
        void Notify(DomainNotification notify);
    }
}
