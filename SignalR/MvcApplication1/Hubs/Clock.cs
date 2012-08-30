using System;
using System.Threading;
using System.Threading.Tasks;
using SignalR;

namespace MvcApplication1.Hubs
{
    public class Clock : PersistentConnection
    {
        private static Timer timer;

        protected override Task OnConnectedAsync(IRequest request, string connectionId)
        {
            new Timer(_ => Connection.Broadcast(DateTime.Now.ToLongTimeString())).Change(0, 1000);

            return Task.FromResult<object>(null);
        }
    }
}