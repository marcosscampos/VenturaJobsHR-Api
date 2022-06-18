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

    protected IActionResult HandleResponse(bool isCreated, object data = null)
    {
        var response = GetResponse(data);

        if (_notificationHandler.HasErrorNotifications() &&
            _notificationHandler.GetNotifications().All(x => x.Type == NotificationType.Error))
        {
            if (_notificationHandler.GetNotifications().All(x => x.Key.ToLower().Contains("notfound")))
                return NotFound(response);
            else
                return BadRequest(response);
        }

        if (isCreated)
            return Created("", null);

        return Ok(response);
    }

    private object GetResponse(object data = null)
    {
        var response = new HandleResponse();

        if (data != null)
        {
            response.StatusCode = null;
            return data;
        }

        SetErrorNotifications(response);
        return response;
    }

#pragma warning disable CS8601
    private void SetErrorNotifications(HandleResponse response)
    {
        if (!_notificationHandler.HasNotifications()) return;

        var notifications = _notificationHandler.GetNotifications();

        var references = notifications.Where(x => x.Type == NotificationType.Error).Select(x => x.Reference).Distinct();
        var enumerable = references.ToList();
        response.Errors = enumerable.Any() ? new Dictionary<string, string>() : null;

        foreach (var reference in enumerable)
        {
            var notification = notifications.FirstOrDefault(x => x.Reference == reference);
            if (notification == null) continue;

            var notificationDictionary =
                notifications.Where(x => x.Reference == notification.Reference).Select(x =>
                    new Dictionary<string, string>
                    {
                        { x.Key, x.Value }
                    });

            foreach (var dictionary in notificationDictionary)
            {
                foreach (var key in dictionary.Keys)
                {
                    foreach (var value in dictionary.Values)
                    {
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        response.Errors?.Add(key, value);
                    }
                }
            }
        }
    }
#pragma warning restore CS8601
}