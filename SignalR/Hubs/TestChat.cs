using System;
using Microsoft.AspNet.SignalR;

namespace BombVacuum.SignalR.Hubs
{

    [Authorize]
    public class TestHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello(DateTime.Now.ToString("F"));
        }

        public void CurrentUser()
        {
            var user = Context.User;
            Clients.Caller.currentUser(user.Identity.Name);
        }
    }
}