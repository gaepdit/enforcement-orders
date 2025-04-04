﻿using Enfo.WebApp.Platform.OrgNotifications;
using Microsoft.AspNetCore.Mvc;

namespace Enfo.WebApp.Pages.Shared.Components.Notifications;

public class NotificationsViewComponent(IOrgNotifications orgNotifications) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync() =>
        View("Default", await orgNotifications.GetOrgNotificationsAsync());
}
