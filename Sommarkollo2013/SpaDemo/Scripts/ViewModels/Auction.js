var Auction = function (description, image, seller, title, url) {
	this.description = ko.observable(description);
	this.image = ko.observable(image);
	this.seller = ko.observable(seller);
	this.title = ko.observable(title);
	this.url = ko.observable(url);
};

var auctionViewModel = {
	auctions: ko.observableArray([])
};

$(document).ready(function() {
	$.getJSON('/api/auction', function(data) {
		auctionViewModel = ko.mapping.fromJS(data);
		ko.applyBindings(auctionViewModel);
	});
});
//Description = "The best lego ever made",
//								Image = "~/Images/lego.jpg",
//								Seller = "Kalle",
//								Title = "Lego auction",
//								Url = "~/Home/Auction/1"