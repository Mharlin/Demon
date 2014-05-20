/// <reference path="scripts/typings/jasmine/jasmine.d.ts" />
/// <reference path="app.ts" />

/// <chutzpah_reference path="scripts/typings/jasmine/jasmine.d.ts" />
/// <chutzpah_reference path="app.ts" />

describe("sortBy", () => {
    it("defines key and returns sorted result", () => {
        var sorted = sortByKey(brands, x => x.name);
        expect(sorted[0].name).toBe("Apple");
    });

    it("defines key of value and returns sorted result", () => {
        var sorted = sortByKey(brands, x => x.value);
        expect(sorted[0].name).toBe("Dell");
    });
});