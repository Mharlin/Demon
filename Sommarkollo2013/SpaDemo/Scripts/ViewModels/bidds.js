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