'use strict'
const mysql = require('mysql');

const connection = mysql.createConnection({
    host : 'localhost',
    user : 'root',
    password : '123456',
    database : 'knowit'
});

connection.connect();

// Query sentences definition
// 1. Users
var allUser = 'SELECT * FROM Users';
var selectUser = 'SELECT * FROM Users WHERE username = ?';
var insertUser = 'INSERT INTO Users(username, password, userImageUrl, phone, email) VALUES(?,?,?,?,?)';
var updateUser = 'UPDATE Users SET phone = ?, email = ? WHERE username = ?';

// 2. Posts
var allPost = 'SELECT * FROM Posts';
var selectPost = 'SELECT * FROM Posts WHERE id = ?';
var insertPost = 'INSERT INTO Posts(id, title, editor, content, imageUrl, mediaUrl, thumbStore, messStore)';

exports.InsertUser = function(username, pass, rePass, imgType, phone, email){
    var resultCode = [];
    /* 
    *  -3 for username already exists, -2 for rePass not equal to pass, 
    *  -1 for phone already exists, 0 for email alreadt exists, 
    *  1 for success
    */
    connection.query(allUser, function(err, result){
        if(err) throw err;
        for(var user in result){
            if(user.username === username){
                resultCode.push(-3);
            }
            if(user.phone === phone){
                resultCode.push(-1);
            }
            if(user.email === email){
                resultCode.push(0);
            }
        }
    });
    if(pass !== rePass){
        resultCode.push(-2);
    }
    // If no error items, then push success flag, insert user
    if(resultCode.length === 0){
        resultCode.push(1);
        var userParam = [username, pass, username+'.'+imgType, phone, email];
        connection.query(insertUser, userParam, function(err, result){
            if(err) throw err;
        });
    }
    return resultCode;
}

exports.InsertPost = function(title, editor, postContent, postImageUrl, postMediaUrl){

}

exports.TestLogIn = function(username, testPass){
    var stateCode = -1; // No such user
    connection.query(allUser, function(err, result){
        if(err) throw err;
        for(var user in result){
            if(user.username === username){
                if(testPass == user.password){
                    code = 1; // Success
                }else code = 0; // Password error
            }
        }
    });
    return stateCode;
}

exports.ModifyUserInfo = function(username, newUserImage, newPhone, newEmail){
    var updateParam = [newPhone, newEmail, username];
    connection.query(updateUser, updateParam, function(err, result){
        if(err) throw err;
    });
    var selectParam = [username];
    if(newUserImage !== null)(
        connection.query(selectUser, selectParam, function(err, result){
            if(err) throw err;
            changeUserImg(newUserImage, result[0].userImageUrl);
        })
    )
}

exports.GiveThumbUp = function(username, postID){

}

exports.LeaveMessage = function(username, postID, messContent){

}

exports.ViewSinglePost = function(postID){

}

exports.ViewAllPosts = function(){

}

exports.GetThumbsNum = function(postID){

}

exports.GetLeaveMessages = function(postID){

}

exports.StoreUserImg = function(userImage, imgType){

}

var changeUserImg = function(newUserImage, userImageUrl){

}