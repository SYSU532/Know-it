'use strict'
const Koa = require('koa');
const router = require('koa-router')();
const bodyParser = require('koa-bodyparser');

const control = require('control/controller');

const app = new Koa();
app.use(bodyParser());

//URLs Handle
router.post('/login', async (ctx, next)=>{
    var username = ctx.request.body.name,
            password = ctx.request.body.pass;
    var loginRes = control.Login(username, password); // [StateCode, errMessgae]
    var jsonBack = {
        'code' : loginRes[0], // 0 for failure, 1 for success
        'errMessage' : loginRes[1] // when fail, print error message
    };
    ctx.response.body = JSON.stringify(jsonBack);
});

router.post('/logup', async (ctx, next)=>{
    var body = ctx.request.body;
    var username = body.name, password = body.pass, imgType = body.imageType, rePass = body.rePass,
        userImage = body.image, phone = body.phone, email = body.email;
    var logupRes = control.Logup(username, password, rePass, userImage, imgType, phone, email); // [StateCode, errMessgae]
    var jsonBack = {
        'code' : logupRes[0], // 0 for failure, 1 for success
        'errMessgae' : logupRes[1] // when fail, print error message
    };
    ctx.response.body = JSON.stringify(jsonBack);
});

router.post('/data', async (ctx, next)=>{
    var body = ctx.request.body;
    var username = body.name, password = body.pass,
        IDtype = body.type, postID = body.id;
    var dataRes = control.Data(username, password, IDtype, postID); // [data, IDtype, postID]
    var jsonBack = {
        'dataContent' : dataRes[0], // The content of reponse data
        'Type' : dataRes[1], // Request data's type
        'ID' : dataRes[2] // Request post's id
    };
    ctx.response.body = JSON.stringify(jsonBack);
});

router.post('/upload', async (ctx, next)=>{
    var body = ctx.request.body, stream;
    var username = body.name, password = body.pass,
    IDtype = body.type, content = body.content;
    if(IDtype !== 'pid'){
        stream = body.dataStream;
    }
    var dataRes = control.Upload(username, password, IDtype, content, stream); // [StateCode, errMessage]
    var jsonBack = {
        'code' : dataRes[0],
        'errMessgae' : dataRes[1]
    };
    ctx.response.body = JSON.stringify(jsonBack);
});

app.listen(8080);
console.log('Server listening in port 8080.......');