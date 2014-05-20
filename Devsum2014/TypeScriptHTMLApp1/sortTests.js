/// <reference path="scripts/typings/jasmine/jasmine.d.ts" />
/// <reference path="app.ts" />
/// <chutzpah_reference path="scripts/typings/jasmine/jasmine.d.ts" />
/// <chutzpah_reference path="app.ts" />
describe("sortBy", function () {
    it("defines key and returns sorted result", function () {
        var sorted = sortByKey(brands, function (x) {
            return x.name;
        });
        expect(sorted[0].name).toBe("Apple");
    });

    it("defines key of value and returns sorted result", function () {
        var sorted = sortByKey(brands, function (x) {
            return x.value;
        });
        expect(sorted[0].name).toBe("Dell");
    });
});
//# sourceMappingURL=sortTests.js.map
