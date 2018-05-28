# Know-It Server APIs

### 1. Server数据库上的四张表：Users, Posts, Thumbs, Comments

* users(username, password, userImageUrl, phone, email)

  userImageUrl为用户头像的存储路径，通常为：img/用户名+照片类型。

* posts(id, title, editor, content, imageUrl, mediaUrl)

  主键id会随着记录的插入自增长, content为文本内容，imageUrl和mediaUrl为帖子的照片存储路径，

  视频文件存储路径（可为空）。

* thumbs(id, thumbUser)

  id为对应的点赞的帖子ID，thumbUser为点赞用户名。

* comments(id, comUser, message)

  id为对应的评论帖子ID，comUser为评论用户名，message为评论内容。

* talks(user, content)


  user为用户名称， content为发言内容。

### 2.Server APIs（均需要采用Post请求方式）

1. 用户登录功能

   * 接口地址：服务器地址/login

   * 返回格式：JSON

     ```js
     {
         'code' : boolean, //0 for failure, 1 for success
         'errMessage' : string
     }
     ```

   * 请求参数：

     name : 用户名称

     pass ：用户密码

     ​

2. 用户注册功能

   * 接口地址：服务器地址/logup

   * 返回格式：JSON

     ```js
     {
         'code' : boolean, //0 for failure, 1 for success
         'errMessage' : string
     }
     ```

   * 请求参数：

     name：用户名称

     pass：用户密码

     imgType：上传的照片格式（示例：,png, .jpg）

     rePass：用户第二次输入的密码

     image：用户上传的照片数据，以二进制流格式上传

     phone：用户电话

     email：用户邮箱

     ​

3. 拿取对应id的帖子数据功能

   * 接口地址：服务器地址/data

   * 返回格式：JSON

     * 成功返回

     ```js
     {
         'code' : 1, //0 for failure, 1 for success (login check)
         'editor' : string    
         'title' : string,   
         'content' : string,
         'image' : string, //Image name, like "jingyang.jpg" or "jingyang.png"
         'media' : string, //Media name
         'thumbs' : int, //Thumbs number
         'comments' : string with JSON format // Like {"jiangyang" : "fuck", "Richard" : "Damn"}
     }
     ```
     * 失败返回

     ```js
     {
         'code' : 0,
         'errMessage' : 'Invalid user and password!'
     }
     ```

   * 请求参数：

     name：用户名称

     pass：用户密码

     id：需要获取的帖子ID

     ​

4. 上传帖子的功能

   * 接口地址：服务器地址/upload

   * 返回格式：JSON

     ```js
     {
         'code' : boolean, //0 for failure, 1 for success (login check)
         'errMessage' : string
     }
     ```

   * 请求参数：

     name：用户名称

     pass：用户密码

     title：帖子主题

     content：帖子文本内容

     imageStream：帖子图像内容（二进制数据）

     mediaStream：帖子视频内容（二进制数据）

     imageType：帖子图像类型，后缀

     mediaType：帖子视频类型，后缀

     ​

5. 获取当前所有帖子的ID功能

   * 接口地址：服务器地址/allPostID

   * 返回格式：JSON

     ```js
     {
         'postIDs' : Array //[id1, id2, ......]
     }
     ```

   * 请求参数：无



6. 对应id的帖子点赞功能

   * 接口地址：服务器地址/thumbUp

   * 返回格式：JSON

     ```js
     {
         'code' : boolean, //0 for failure, 1 for success (login check)
         'errMessage' : string
     }
     ```

   * 请求参数

     name：用户名称

     pass：用户密码

     postID：帖子ID

     ​

7. 对应id帖子取消点赞功能

   * 接口地址：服务器地址/thumbDown

   * 返回格式：JSON

     ```js
     {
         'code' : boolean, //0 for failure, 1 for success (login check)
         'errMessage' : string
     }
     ```

   * 请求参数

     name：用户名称

     pass：用户密码

     postID：帖子ID

     ​

8. 对应帖子评论功能

   * 接口地址：服务器地址/commentUp

   * 返回格式：JSON

     ```js
     {
         'code' : boolean, //0 for failure, 1 for success (login check)
         'errMessage' : string
     }
     ```

   * 请求参数

     name：用户名称

     pass：用户密码

     postID：帖子ID

     content：评论内容

     ​

9. 更改用户Info的功能

   * 接口地址：服务器地址/modifyUserInfo

   * 返回格式：JSON

     ```js
     {
         'code' : boolean, //0 for failure, 1 for success (login check)
         'errMessage' : string
     }
     ```

   * 请求参数

     name：用户名称

     pass：用户密码

     newEmail：用户新邮箱

     newPhone：用户新电话

     newUserImage：用户新头像（二进制数据，若头像没有重新上传，则可以将上传二进制数据设为1字节）

     ​

10. 获取用户Info的功能

   * 接口地址：服务器地址/getUserInfo

   * 返回格式：JSON

     * 成功返回

     ```js
     {
         'code' : 1,
         'username' : string
         'phone' : string,
         'email' : string,
         'imageUrl' : string
     }
     ```

     * 失败返回

     ```js
     {
         'code' : 0,
         'errMessage' : 'User not exist!'
     }
     ```

   * 请求参数：

     name：用户名称

     ​

11. 获取聊天室所有消息功能

    * 接口地址：服务器地址/getAllTalks

    * 返回格式：JSON

      * 成功返回

      ```js
      {
          'code' : 1, //login check
          'user1' : content1,
          'user2' : content2,
          ....
      }
      ```

      * 失败返回

      ```js
      {
          'code' : 0,
          'errMessage' : string
      }
      ```

    * 请求参数

      name：用户名称

      pass：用户密码

      ​

12. 聊天室发送消息功能

    * 接口地址：服务器地址/sendTalk

    * 返回格式：JSON

      ```js
      {
          'code' : boolean, //login check
          'errMessage' : string
      }
      ```

    * 请求参数：

      name：用户名称

      pass：用户密码

      content：发送消息

      ​

13. 获取当前所有帖子精简信息功能

    * 接口地址：服务器地址/allDatas

    * 返回格式：JSON

      * 成功返回

      ```js
      {
          'code' : 1,
          '1' : { // post ID as the first level index
              'editor' : string,
              'title' : string,
              'content' : string,
              'imageUrl' : string,  // If post has no image, then it will be ''
              'thumbs' : int
          },
          '2' : {
              ...
          }
          ...
      }
      ```

      * 失败返回

      ```js
      {
          'code' : 0,
          'errMessage' : string
      }
      ```

    * 请求参数

      name：用户名称

      pass：用户密码