/// <reference path="../jquery-1.9.1.js" />
/// <reference path="../jquery.signalR-1.1.2.js" />
/// <reference path="~/Scripts/knockout-2.2.1.debug.js" />
/// <reference path="~/Scripts/knockout.mapping-latest.debug.js" />


var Bid = function (amount, bidder) {
				this.amount = ko.observable(amount);
				this.bidder = ko.observable(bidder);
};

var BidsViewModel = function (data, auctionHub) {
				var self = this;
				self.bidds = ko.observableArray();
				ko.mapping.fromJS(data, {}, self);

				auctionHub.client.bidAdded = function (bidder, amount) {
								self.bidds.push(new Bid(amount, bidder));
				};

				self.newBidAmount = ko.observable();
				self.newBidder = ko.observable();
				self.addBid = function () {
								auctionHub.server.addBid(self.newBidder(), self.newBidAmount());
				};

				self.submitButtonEnabled = ko.computed(function () {
								return Math.max.apply(null, self.bidds().map(function (bid) {
												return bid.amount();
								})) < self.newBidAmount();
				});
};

$(document).ready(function () {
				var url = window.location.href.split('/');
				var id = url[url.length - 1];

				var auctionHub = $.connection.auctionHub;


				$.getJSON('/api/auction/' + id, function (data) {
								ko.applyBindings(new BidsViewModel(data, auctionHub));

								//$.connection.hub.start({ transport: 'longPolling' }).done(function () {
								$.connection.hub.start().done(function () {
												auctionHub.server.addBid("auction started", 0);
								});
				});

});