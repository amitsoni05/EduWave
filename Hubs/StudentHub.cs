
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CrudStudentProject.Hubs
{
    public class StudentHub:Hub
    {
        public async Task SendNotification(string message)
        {
            // Broadcast the message to all connected clients
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}
