/// <reference path="../jquery.signalR-1.1.2.js" />
/// <reference path="../knockout-2.2.0.js" />


var Bid = function (amount, bidder) {
	this.amount = ko.observable(amount);
	this.bidder = ko.observable(bidder);
};

var BidsViewModel = function (data) {
	var self = this;
	ko.mapping.fromJS(data, {}, self);
	var auction = $.connection.auctionHub;

	auction.client.bidAdded = function (bidder, amount) {
		self.bids.push(new Bid(amount, bidder));
	};

	self.newBidAmount = ko.observable();
	self.newBidder = ko.observable();
	self.addBid = function () {
		auction.server.addBid(self.newBidder(), self.newBidAmount());
	};

	$.connection.hub.start().done(function() {
		auction.server.addBid("auction started", 0);
	});
};

$(document).ready(function () {
	var url = window.location.href.split('/');
	var id = url[url.length - 1];

	$.getJSON('/api/auction/' + id, function (data) {
		ko.applyBindings(new BidsViewModel(data));
	});
});