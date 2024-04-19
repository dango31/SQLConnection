using Microsoft.AspNetCore.SignalR;
using BlazorGroupB.Models;
namespace BlazorGroupB.Hubs;

public class ChatHub : Hub
{

    public async Task SendMessage(Messages msg)
    {
        await Clients.All.SendAsync("ReceiveMessage", msg);
    }
}