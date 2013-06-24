using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SpaDemo.Controllers
{
	public class AuctionController : ApiController
	{
		public auctionList Get()
		{
			return new auctionList
					   {
						   auctions = new List<auction>
							   {
								   new auction
									   {
										   description = "The best lego ever made",
										   image = "Images/lego.jpg",
										   seller = "Kalle",
										   title = "Lego auction",
										   url = "Home/Auction/1"
									   },
								   new auction
									   {
										   description = "Cute barbies in mint condition",
										   image = "Images/barbie.jpg",
										   seller = "Stina",
										   title = "Barbie auction",
										   url = "Home/Auction/2"
									   }
							   }
					   };
		}

		public class auctionList
		{
			public List<auction> auctions { get; set; }
		}

		public bidding Get(int id)
		{
			if (id == 1)
			{
				return new bidding
						   {
							   title = "Lego auction",
							   bids = new List<bid>
										  {
											  new bid { Amount = 20, Bidder = "Fredrik" },
											  new bid { Amount = 30, Bidder = "Magnus" }
										  }
						   };
			}

			return new bidding
					   {
						   title = "Barbie auction",
						   bids = new List<bid>
										  {
											  new bid { Amount = 50, Bidder = "Kalle" },
											  new bid { Amount = 55, Bidder = "Magnus" }
										  }
					   };
		}

		public class auction
		{
			public string url { get; set; }
			public string title { get; set; }
			public string description { get; set; }
			public string image { get; set; }
			public string seller { get; set; }
		}

		public class bidding
		{
			public string title { get; set; }
			public List<bid> bids { get; set; }
		}

		public class bid
		{
			public int Amount { get; set; }
			public string Bidder { get; set; }
		}
	}
}
