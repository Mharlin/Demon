using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SpaDemo.Controllers
{
	public class AuctionController : ApiController
	{
		public List<Auction> Get()
		{
			return new List<Auction>
					   {
						   new Auction
							   {
								   Description = "The best lego ever made",
								   Image = "~/Images/lego.jpg",
								   Seller = "Kalle",
								   Title = "Lego auction",
								   Url = "~/Home/Auction/1"
							   },
						   new Auction
							   {
								   Description = "Cute barbies in in mint condition",
								   Image = "~/Images/barbie.jpg",
								   Seller = "Stina",
								   Title = "Barbie auction",
								   Url = "~/Home/Auction/2"
							   }
					   };
		}

		public Bidding Get(int id)
		{
			if (id == 1)
			{
				return new Bidding
						   {
							   Title = "Lego auction",
							   Bids = new List<Bid>
										  {
											  new Bid { Amount = 20, Bidder = "Fredrik" },
											  new Bid { Amount = 30, Bidder = "Magnus" }
										  }
						   };
			}

			return new Bidding
					   {
						   Title = "Barbie auction",
						   Bids = new List<Bid>
										  {
											  new Bid { Amount = 50, Bidder = "Kalle" },
											  new Bid { Amount = 55, Bidder = "Magnus" }
										  }
					   };
		}

		public class Auction
		{
			public string Url { get; set; }
			public string Title { get; set; }
			public string Description { get; set; }
			public string Image { get; set; }
			public string Seller { get; set; }
		}

		public class Bidding
		{
			public string Title { get; set; }
			public List<Bid> Bids { get; set; }
		}

		public class Bid
		{
			public int Amount { get; set; }
			public string Bidder { get; set; }
		}
	}
}
