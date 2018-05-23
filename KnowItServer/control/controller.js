'use strict'
const model = require('../model/model');

exports.Login = function(username, pass){
    var result = {
        'code' : 0, // 0 for failure, 1 for success
        'errMessage' : ''
    };
    // -1 for user not exit, 0 for password error, 1 for success
    var code = model.TestLogIn(username, pass); 
    switch(code){
        case -1: 
            result['errMessage'] = 'User not exist!';
            break;
        case 0:
            result['errMessage'] = 'Wrong Password!';
            break;
        case 1:
            result['code'] = 1;
    }
    return result;
}

exports.Logup = function(username, pass, rePass, userImage, imgType, phone, email){
    var result = {
        'code' : 0,
        'errMessage' : ''
    };
    var errItems = model.InsertUser(username, pass, rePass, imgType, phone, email);
    for(var item in errItems){
        switch(item){
            case -3:
                result['errMessage'] += 'Username already exists!';
                break;
            case -2:
                result['errMessage'] += 'The second password does not equal to first password!';
                break;
            case -1:
                result['errMessage'] += 'Phone number already exists!';
                break;
            case 0:
                result['errMessage'] += 'E-mail already exist!';
                break;
            case 1:
                result['code'] = 1;
                model.StoreUserImg(userImage, imgType);
        }
    }
    return result;
}

exports.Data = function(username, pass, IDtype, id){

}

exports.Upload = function(username, pass, IDtype, content, dataStream){

}