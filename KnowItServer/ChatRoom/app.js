'use strict'
const ws = require('nodejs-websocket');

var server = ws.createServer(function(conn){
    conn.on('text', function(str){
        console.log('Receive: ' + str);
        conn.sendText(str);
    });
    conn.on('close', function(code, reason){
        console.log('WebSocket连接关闭...');
    });
    conn.on('error', function(code, reason){});
}).listen(8081);

console.log('WebSocket Server listening on port 8081....');
