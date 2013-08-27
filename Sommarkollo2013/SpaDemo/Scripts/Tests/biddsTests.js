/// <reference path="../jquery-1.9.1.js" />
/// <reference path="../jasmine.js" />
/// <reference path="../knockout-2.2.0.js" />
/// <reference path="../knockout.mapping-latest.js" />
/// <reference path="../jquery.signalR-1.1.2.js" />
/// <reference path="~/Scripts/ViewModels/bidds.js" />

describe("a bid", function () {
				var viewModel;
				beforeEach(function () {
								var auctionHub = jasmine.createSpyObj('auctionHub', ['client', ['server']]);

								var bidds = {
												title: 'auction',
												bidds: [{
																bidder: 'M',
																amount: 15
												}]
								};

								viewModel = new BidsViewModel(bidds, auctionHub);
				});

				it("that is higher enables submit button", function () {
								viewModel.newBidAmount(20);
								viewModel.newBidder('Joakim von Anka');

								expect(viewModel.submitButtonEnabled()).toBeTruthy();
				});

				it("that is lower disabled submit button", function () {
								viewModel.newBidAmount(5);
								viewModel.newBidder("Kalle Anka");

								expect(viewModel.submitButtonEnabled()).toBeFalsy();
				});
});