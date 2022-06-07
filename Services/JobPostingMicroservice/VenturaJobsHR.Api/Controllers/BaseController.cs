using Microsoft.AspNetCore.Mvc;
using VenturaJobsHR.CrossCutting.Enums;
using VenturaJobsHR.CrossCutting.Notifications;
using VenturaJobsHR.CrossCutting.Responses;

namespace VenturaJobsHR.Api.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected readonly INotificationHandler _notificationHandler;

    public BaseController(
        INotificationHandler notificationHandler)
    {
        _notificationHandler = notificationHandler;
    }

    protected IActionResult HandleResponse(object data = null)
    {
        var response = GetResponse(data);

        if (_notificationHandler.HasErrorNotifications() && _notificationHandler.GetNotifications().All(x => x.Type == NotificationType.Error))
        {
            if (_notificationHandler.GetNotifications().All(x => x.Key.ToLower().Contains("notfound")))
                return NotFound(response);
            else
                return BadRequest(response);
        }

        return Ok(response);
    }

    private object GetResponse(object data = null)
    {
        var response = new HandleResponse();

        if (data != null)
            response.Data = data;
        else
            SetNotifications(response);


        return response;
    }

#pragma warning disable CS8601
    private void SetNotifications(HandleResponse response)
    {
        if (!_notificationHandler.HasNotifications()) return;

        var notifications = _notificationHandler.GetNotifications();

        response.Success = notifications
            .Where(x => x.Type == NotificationType.Success)
            .Select(x => new NotificationResponse
            {
                Code = x.ReferenceId,
                Reference = x.Reference,
                Notifications = notifications.Where(y => (y.Type == NotificationType.Warning || y.Type == NotificationType.Information) &&
                                                           y.Reference.ToUpper().Equals(x.ReferenceId.ToUpper()))
                                .Select(z => new NotificationResponseItem
                                {
                                    Key = z.Key,
                                    Message = z.Value
                                })?.ToList()
            })?.ToList();

        response.Success = response.Success.Any() ? response.Success : null;

        var references = notifications.Where(x => x.Type == NotificationType.Error).Select(x => x.Reference).Distinct();
        response.Errors = references.Any() ? new List<NotificationResponse>() : null;

        foreach (var reference in references)
        {
            var notification = notifications.FirstOrDefault(x => x.Reference == reference);
            if (notification == null) continue;

            response.Errors.Add(new NotificationResponse
            {
                Reference = notification.Reference,
                Notifications = notifications.Where(x => x.Reference == notification.Reference).Select(x => new NotificationResponseItem
                {
                    Key = x.Key,
                    Message = x.Value
                }).ToList()
            });
        }

    }
#pragma warning restore CS8601
}
