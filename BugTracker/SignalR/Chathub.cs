using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace BugTracker
{
    public class ChatHub : Hub
    {
        public void Send( string message)
        {
            var username = Context.User.Identity.Name;
            //call the broadcastmessage method to update clients
            Clients.All.addNewMessageToPage(username, message);
        }

        public void JoinRoom(string room)
        {
            Groups.Add(Context.ConnectionId, room);
        }
        public void SendMessage(string room, string message, int x, int y)
        {
            var username = Context.User.Identity.Name;
            Clients.All.onNewMessage(username, message, x, y);
        }

        public void SendPrivateMessage(string toUsername, string message)
        {

            var fromUsername = Context.User.Identity.Name;
            Clients.Group("users/" + toUsername).onNewPrivateMessage(fromUsername, message);
        }
        public override Task OnConnected()
        {
            var username = Context.User.Identity.Name;
            if (!string.IsNullOrEmpty(username))
            {
                Groups.Add(Context.ConnectionId, "users/" + username);
            }
            return base.OnConnected();

        }
    }
}