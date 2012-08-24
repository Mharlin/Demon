using System;
using System.Threading;
using SignalR;

namespace MvcApplication1.Hubs
{
    public class ConnectionCounter : PersistentConnection
    {
        private static int connectionCount;

        protected override System.Threading.Tasks.Task OnConnectedAsync(IRequest request, string connectionId)
        {
            Interlocked.Increment(ref connectionCount);
            return Connection.Broadcast(connectionCount);
        }

        protected override System.Threading.Tasks.Task OnReconnectedAsync(IRequest request, System.Collections.Generic.IEnumerable<string> groups, string connectionId)
        {
            Interlocked.Increment(ref connectionCount);
            return Connection.Broadcast(connectionCount);
        }

        protected override System.Threading.Tasks.Task OnDisconnectAsync(string connectionId)
        {
            Interlocked.Decrement(ref connectionCount);
            return Connection.Broadcast(connectionCount);
        }
    }
}