'use strict'
const mysql = require('mysql');
const fs = require('fs');

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
var selectUser = 'SELECT * FROM users WHERE username = ?';
var insertUser = 'INSERT INTO users(username, password, userImageUrl, phone, email) VALUES(?,?,?,?,?)';
var updateUser = 'UPDATE users SET phone = ?, email = ? WHERE username = ?';

// 2. Posts
var allPost = 'SELECT * FROM Posts';
var selectPost = 'SELECT * FROM Posts WHERE id = ?';
// thumbStore strucyure: [thumbGuy1, thumbGuy2, .....], messStore structure: [[messGuy1, mess1], [messGuy2, mess2], .....]
var insertPost = 'INSERT INTO Posts(title, editor, content, imageUrl, mediaUrl) VALUES(?,?,?,?,?)';
var getPostCount = 'SELECT id FROM Posts';

// 3. Thumbs
var selectThumb = 'SELECT * FROM Thumbs WHERE id = ?';
var insertThumb = 'INSERT INTO Thumbs(id, thumbUser) VALUES(?,?)';
var deleteThumb = 'DELETE FROM Thumbs WHERE id = ? AND thumbUser = ?';

// 4. Comments
var selectComment = 'SELECT * FROM Comments WHERE id = ?';
var insertComment = 'INSERT INTO Comments(id, comUser, message) VALUES(?,?,?)';

exports.InsertUser = function(username, pass, userImageUrl, phone, email){
    /* 
    *  -3 for username already exists, -2 for rePass not equal to pass, 
    *  -1 for phone already exists, 0 for email alreadt exists, 
    *  1 for success
    */
    var userParam = [username, pass, userImageUrl, phone, email];
    connection.query(insertUser, userParam, function(err, result){
        if(err) throw err;
    });
}

exports.AllUser = async function(){
    return new Promise((resolve, reject)=>{
        connection.query(allUser, function(err, result){
            if(err){
                reject(err);
            }else {
                resolve(result);
            }
        });
    });
}

exports.InsertPost = function(title, editor, postContent, postImageUrl, postMediaUrl){
    var insertParam = [title, editor, postContent, postImageUrl, postMediaUrl];
    connection.query(insertPost, insertParam, function(err, result){
        if(err) throw err;
    });
}

exports.TestLogIn = async function(username, testPass){
    return new Promise((resolve, reject)=>{
        connection.query(allUser, function(err, result){
            var code = -1; // No such user
            if(err){
                reject(err);
            }else {
                result.forEach(function(user) {
                    if(user.username === username){
                        if(testPass == user.password){
                            code = 1; // Success
                        }else code = 0; // Password error
                    }
                });
                resolve(code);
            }
        });
    });
}

exports.ModifyUserInfo = function(username, newUserImage, newPhone, newEmail){
    var updateParam = [newPhone, newEmail, username];
    connection.query(updateUser, updateParam, function(err, result){
        if(err) throw err;
    });
    var selectParam = [username];
    if(newUserImage !== '')(
        connection.query(selectUser, selectParam, function(err, result){
            if(err) throw err;
            changeUserImg(newUserImage, result[0].userImageUrl);
        })
    )
}

exports.GiveThumbUp = function(username, postID){
    var insertParam = [postID, username];
    connection.query(insertThumb, insertParam, function(err, result){
        if(err) throw err;
    });
}

exports.DeleteThumbUp = function(username, postID){
    var deleteParam = [postID, username];
    connection.query(deleteThumb, deleteParam, function(err, result){
        if(err) throw err;
    });
}

exports.LeaveMessage = function(username, postID, messContent){
    var insertParam = [postID, username, messContent];
    connection.query(insertComment, insertParam, function(err, result){
        if(err) throw err;
    });
}

exports.GetUserInfo = async function(username){
    return new Promise((resolve, reject)=>{
        var selectParam = [username];
        var res = {};
        connection.query(selectUser, selectParam, function(err, result){
            if(err){
                reject(err);
            }
            if(result.length > 0){
                res['phone'] = result[0].phone;
                res['email'] = result[0].email;
                res['image'] = result[0].userImageUrl;
            }
            resolve(res);
        });
    });
}

// View a single posts's details
exports.ViewSinglePost = async function(postID){
    return new Promise((resolve, reject)=>{
        var res = [];
        var realID = parseInt(postID);
        connection.query(selectPost, [realID], function(err, result){
            if(err){
                reject(err);
            }
            var data = result[0];
            var imageUrl = data.imageUrl, mediaUrl = data.mediaUrl;
            res.push(data.content);
            res.push(imageUrl);
            res.push(mediaUrl);
            connection.query(selectThumb, [realID], function(err, result){
                if(err){
                    reject(err);
                }
                res.push(result.length);
                connection.query(selectComment, [realID], function(err, result){
                    if(err){
                        reject(err);
                    }
                    var comments = {};
                    result.forEach(function(com){
                        comments[com.comUser] = comments[com.message];
                    })
                    res.push(comments);
                    resolve(res);
                });
            });
        });
    });
}

exports.StoreUserImg = function(userImage, imageUrl){   
    var buff = new Buffer(userImage, 'ascii');
    var trueImageUrl = 'img/' + imageUrl;
    fs.writeFileSync(trueImageUrl, buff);
}

exports.StorePostImg = function(imageUrl, imageStream){
    var buff = new Buffer(imageStream, 'ascii');
    var trueImageUrl = 'img/' + imageUrl;
    fs.writeFileSync(trueImageUrl, buff, function(err){
        if(err) throw err;
    });
}

exports.StorePostMedia = function(mediaUrl, mediaStream){
    var buff = new Buffer(mediaStream, 'ascii');
    var trueMediaUrl = 'img/' + mediaUrl;
    fs.writeFileSync(trueMediaUrl, buff, function(err){
        if(err) throw err;
    });
}

exports.GetPostsIDs = async function(){
    return new Promise((resolve, reject)=>{
        var res = [];
        connection.query(getPostCount, function(err, result){
            if(err){
                reject(err);
            }
            for(var id in result){
                res.push(id);
            }
            resolve(res);
        });
    })
}

var changeUserImg = function(newUserImage, userImageUrl){
    var buff = new Buffer(newUserImage);
    var trueImageUrl = '../img/' + UserImageUrl;
    fs.writeFileSync(trueImageUrl, buff, function(err){
        if(err) throw err;
    });
}