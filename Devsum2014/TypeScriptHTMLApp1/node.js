/// <reference path="scripts/typings/node/node.d.ts" />
var http = require("http");

http.createServer(function (req, res) {
    res.writeHead(200, { 'Content-Type': 'text/plain' });
    res.end('Hello World\n');
}).listen(1337);
//# sourceMappingURL=node.js.map
