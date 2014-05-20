var brands = [
    { name: "Apple", value: 400 },
    { name: "Microsoft", value: 700 },
    { name: "Dell", value: 200 }
];

interface IName {
    name: string;
}

function sortByName<T extends IName>(a: T[], keyOf: (item: T) => any) {
    var result = a.slice(0);
    result.sort((x, y) => {
        var kx = keyOf(x);
        var ky = keyOf(y);

        return kx > ky ? 1 : kx < ky ? -1 : 0;
    });
    return result;
}

var sorted = sortByName(brands, x => x.value);
var name = sorted[0].name;

document.body.innerText = JSON.stringify(sorted, null, 4);