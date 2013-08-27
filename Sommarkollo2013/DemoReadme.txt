Steg 1:
	Add scripts/ViewModels/auction.js
		auctionViewModel = {
			auctions: ko.observableArray([])
		}
		$(document).ready(function() {
			$.getJSON('/api/auction', function(data) {
				auctionViewModel = ko.mapping.fromJS(data);
				ko.applyBindings(auctionViewModel);
			});
		});
	
	Update Views/Home/index.cshtml
		<div data-bind="foreach: auctions">
			...
			<a data-bind="attr: { href: url}">
				bind title
				<img data-bind="attr: { src: image }" />
				bind seller
				bind description
		@section scripts
		{
			@Scripts.Render("~/Scripts/ViewModels/Auction.js") 
		}

	Update Views/Shared/_Layout.cshtml
		@Scripts.Render("~/Scripts/knockout-2.2.0.js")
    	@Scripts.Render("~/Scripts/knockout.mapping-latest.js")

Steg 2
	Update Views/Home/Auction.cshtml
		<li data-bind="foreach: bids">
			data bind bidder
			data bind amount

		place bid
			data bind newBidder
			data bind newBidAmount
			<button data-bind="click: addBid">
		@section scripts
		{
			@Scripts.Render("~/Scripts/ViewModels/bidds.js") 
		}

	Add Scripts/ViewModels/bidds.js
		var Bid = function (amount, bidder) {
			this.amount = ko.observable(amount);
			this.bidder = ko.observable(bidder);
		};

		var BidsViewModel = function (data) {
			var self = this;
			ko.mapping.fromJS(data, {}, self);

			//self.bids = ko.observableArray([]);
			self.newBidAmount = ko.observable();
			self.newBidder = ko.observable();
			self.addBid = function () {
				self.bids.push(new Bid(self.newBidAmount(), self.newBidder()));
			};
		};

		$(document).ready(function () {
			var url = window.location.href.split('/');
			var id = url[url.length - 1];

			$.getJSON('/api/auction/' + id, function (data) {
				ko.applyBindings(new BidsViewModel(data));
			});
		});

Steg 3:
	Add SignalR from NuGet
	
	Add Hubs/AuctionHub.cs
			public class AuctionHub : Hub
			{
				public void AddBid(string bidder, int amount)
				{
					Clients.All.bidAdded(bidder, amount);
				}
			}
	update Scripts/ViewModels/bidds.js
		BidsViewModel
			var auction = $.connection.auctionHub;
			auction.client.bidAdded = function (bidder, amount) {
				self.bids.push(new Bid(amount, bidder));
			};

			update the addBid function
				auction.server.addBid(self.newBidder(), self.newBidAmount());
	
			$.connection.hub.start().done(function () {
				auction.server.addBid("auction started", 0);
			});
		
	update Views/Shared/_Layout.cshtml
			@Scripts.Render("~/Scripts/jquery.signalR-1.1.2.js")
			<script src="~/signalr/hubs"></script>

Steg4:
	Installera glimpse och glimpse knockout
	Visa timeline, views och execution
	Visa firebug
		visa hur signalR switchar till websockets
	Installera och visa Elmah
		slå in en adress som inte finns/skicka in bokstäver till en funktion som ska ta emot siffror

Steg 5:
	Installera Jasmine
	Om SignalR anslutningen ligger i vymodellen flytta ut den till document.ready
	Lägg till en enableBindning på addBid knappen: enable: submitButtonEnabled
	Skriv testet, visa i R# testrunner
	Från Notepad++ ta fram htmlsidan för jasmine testerna, visa debug med FF

