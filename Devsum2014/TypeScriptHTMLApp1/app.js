﻿var brands = [
    { name: "Apple", value: 400 },
    { name: "Microsoft", value: 700 },
    { name: "Dell", value: 200 }
];

function sortByKey(a, keyOf) {
    var result = a.slice(0);
    result.sort(function (x, y) {
        var kx = keyOf(x);
        var ky = keyOf(y);

        return kx > ky ? 1 : kx < ky ? -1 : 0;
    });
    return result;
}

var sorted = sortByKey(brands, function (x) {
    return x.value;
});
var name1 = sorted[0];
//document.body.innerText = JSON.stringify(sorted, null, 4);
//# sourceMappingURL=app.js.map
