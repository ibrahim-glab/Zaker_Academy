using Microsoft.AspNetCore.SignalR;

namespace Zaker_Academy.Hubs
{
    public sealed class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {

            await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has sent message");
        }
    }
}
