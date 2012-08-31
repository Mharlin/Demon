using System.Threading.Tasks;
using SignalR;
using SignalR.Hubs;

namespace MvcApplication1.Hubs
{
    public class MoveShapeHub : Hub
    {
         public Task MoveShape(int x, int y)
         {
             return Clients.shapeMoved(Context.ConnectionId, x, y);
         }
    }
}