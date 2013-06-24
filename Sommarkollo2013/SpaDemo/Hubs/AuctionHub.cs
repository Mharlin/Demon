using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;

namespace SpaDemo.Hubs
{
	public class AuctionHub : Hub
	{
		public void AddBid(string bidder, int amount)
		{
			Clients.All.bidAdded(bidder, amount);
		}
	}
}