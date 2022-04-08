using VenturaJobsHR.CrossCutting.Notifications;

namespace VenturaJobsHR.Application.Services.Concretes;

public class ApplicationServiceBase
{
    protected INotificationHandler Notification;

    protected ApplicationServiceBase(INotificationHandler notification)
    {
        Notification = notification;
    }
}
