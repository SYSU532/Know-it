using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

namespace know_it
{
    class NetworkControl
    {
        public static string AccessingURI = "127.0.0.1:18080";
        private const string httpsPrefix = "http://";
        public static string accessName { get { return httpsPrefix + AccessingURI; } }

        public static async Task<Dictionary<string, string>> QueryUserInfo(string userName)
        {
            Dictionary<String, String> requestData = new Dictionary<string, string>
            {
                {"name", userName}
            };

            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync(accessName + "/getUserInfo", new FormUrlEncodedContent(requestData));

                string responseString = await response.Content.ReadAsStringAsync();

                var parseResult = parseUserInfoJSON(responseString);

                return parseResult;
            }
            catch
            {
                //Connection error
                return new Dictionary<String, String>
                {
                    {"code", "-1"}
                };
            }
        }

        private static Dictionary<string, string> parseUserInfoJSON(string JSONStr)
        {
            var json = JsonObject.Parse(JSONStr);
            var returnCode = json.GetNamedNumber("code");
            if (returnCode == 0)
            {
                var errMsg = json.GetNamedString("errMessage");
                return new Dictionary<string, string>
                {
                    {"code", "0"},
                    {"errMessage", errMsg }
                };
            }
            else if (returnCode == 1)
            {
                var username = json.GetNamedString("username");
                var email = json.GetNamedString("email");
                var imageUrl = json.GetNamedString("imageUrl");
                var phone = json.GetNamedString("phone");
                return new Dictionary<string, string>
                {
                    {"code", "1"},
                    {"username", username },
                    {"email", email},
                    {"phone", phone},
                    {"imageUrl", imageUrl}
                };
            }
            //Unexpected Return value 
            return new Dictionary<string, string>
            {
                {"code", "-2"}
            };
        }

        public static async Task<Dictionary<string, string>> AttemptSignin(string username, string password)
        {
            var requestData = new Dictionary<string, string>
            {
                {"name", username},
                {"pass", password}
            };
            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync(accessName + "/login", new FormUrlEncodedContent(requestData));

                string responseString = await response.Content.ReadAsStringAsync();

                var parseResult = parseBinaryResponse(responseString);

                return parseResult;
            }
            catch
            {
                //Connection error
                return new Dictionary<String, String>
                {
                    {"code", "-1"}
                };
            }
        }

        public static async Task<Dictionary<string, object>> GetPostFromID(string username, string password, string postID)
        {
            var requestData = new Dictionary<string, string>
            {
                {"name", username},
                {"pass", password},
                {"id", postID }
            };
            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync(accessName + "/data", new FormUrlEncodedContent(requestData));

                string responseString = await response.Content.ReadAsStringAsync();

                var parseResult = parsePostInfo(responseString);

                return parseResult;
            }
            catch
            {
                //Connection error
                return new Dictionary<String, object>
                {
                    {"code", "-1"}
                };
            }
        }

        private static Dictionary<string, object> parsePostInfo(string JSONStr)
        {
            JsonObject json = JsonObject.Parse(JSONStr);
            var returnCode = json.GetNamedNumber("code");
            if (returnCode == 0)
            {
                var errMsg = json.GetNamedString("errMessage");
                return new Dictionary<string, object>
                {
                    {"code", "0"},
                    {"errMessage", errMsg }
                };
            }
            else if (returnCode == 1)
            {
                var content = json.GetNamedString("content");
                var media = json.GetNamedString("media");
                var image = json.GetNamedString("image");
                var thumbs = json.GetNamedNumber("thumbs");
                JsonObject comments = json.GetNamedObject("comments");
                Dictionary<string, object> res = new Dictionary<string, object>
                {
                    {"code", "1"},
                    {"content", content },
                    {"image", image },
                    {"thumbs", thumbs.ToString() }
                };
                Dictionary<String, String> commentDict = new Dictionary<string, string>();
                foreach (var pair in comments)
                {
                    commentDict.Add(pair.Key, pair.Value.GetString());
                }
                res.Add("comment", commentDict);
                return res;
            }
            //Unexpected Return value 
            return new Dictionary<string, object>
            {
                {"code", "-2"}
            };

        }

        public static async Task<List<ArrayList>> GetAllSimplePosts(string username, string password)
        {
            Dictionary<String, String> requestData = new Dictionary<string, string>()
            {
                {"name", username },
                {"pass", password }
            };

            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync(accessName + "/allDatas", new FormUrlEncodedContent(requestData));

                string responseString = await response.Content.ReadAsStringAsync();

                List<ArrayList> list = new List<ArrayList>();

                var json = JsonObject.Parse(responseString);

                if(json.GetNamedNumber("code") == 1)
                {
                    for(int i = 0; i < json.Count-1; i++)
                    {
                        var arr = new ArrayList();
                        var index = (i + 1).ToString();
                        var item = json.GetNamedObject(index);
                        arr.Add(item.GetNamedString("editor"));
                        arr.Add(item.GetNamedString("title"));
                        arr.Add(item.GetNamedString("content"));
                        arr.Add(item.GetNamedString("imageUrl"));
                        arr.Add(item.GetNamedNumber("thumbs"));
                        list.Add(arr);
                    }
                }else
                {
                    Debug.WriteLine(json.GetNamedString("errMessage"));
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<List<string>> GetAllPostID(string userName)
        {
            Dictionary<String, String> requestData = new Dictionary<string, string>();

            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync(accessName + "/allPostID", new FormUrlEncodedContent(requestData));

                string responseString = await response.Content.ReadAsStringAsync();

                var parseResult = parsePostIDs(responseString);

                return parseResult;
            }
            catch
            {
                //Connection error
                return null;
            }
        }

        public static async Task<Boolean> CheckUserThumbOrNot(string username, string id)
        {
            var requestData = new Dictionary<string, string>
            {
                {"name", username},
                {"id", id }
            };

            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync(accessName + "/checkThumb", new FormUrlEncodedContent(requestData));

                string responseString = await response.Content.ReadAsStringAsync();

                var json = JsonObject.Parse(responseString);
                var res = json.GetNamedNumber("haveThumb");
                if (res == 1)
                {
                    return true;
                }
                else return false;
            }
            catch
            {
                //Connection error
                return false;
            }
        }

        private static List<String> parsePostIDs(string JSONStr)
        {
            try
            {
                var json = JsonObject.Parse(JSONStr);
                List<String> li = new List<string>();
                var idArray = json.GetNamedArray("postIDs");
                foreach (var item in idArray)
                {
                    li.Add(item.GetString());
                }
                return li;
            }
            catch
            {
                //Unexpected Return value 
                return null;
            }
        }


        public static async Task<Dictionary<string, string>> GiveThumbToPost(string username, string password, string postID)
        {
            var requestData = new Dictionary<string, string>
            {
                {"name", username},
                {"pass", password},
                {"postID", postID }
            };
            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync(accessName + "/thumbUp", new FormUrlEncodedContent(requestData));

                string responseString = await response.Content.ReadAsStringAsync();

                var parseResult = parseBinaryResponse(responseString);

                return parseResult;
            }
            catch
            {
                //Connection error
                return new Dictionary<String, String>
                {
                    {"code", "-1"}
                };
            }
        }

        private static Dictionary<string, string> parseBinaryResponse(string JSONStr)
        {
            var json = JsonObject.Parse(JSONStr);
            var returnCode = json.GetNamedNumber("code");
            if (returnCode == 0)
            {
                var errMsg = json.GetNamedString("errMessage");
                return new Dictionary<string, string>
                {
                    {"code", "0"},
                    {"errMessage", errMsg }
                };
            }
            else if (returnCode == 1)
            {
                return new Dictionary<string, string>
                {
                    {"code", "1"}
                };
            }
            //Unexpected Return value 
            return new Dictionary<string, string>
            {
                {"code", "-2"}
            };
        }


        public static async Task<Dictionary<string, string>> CancelThumbToPost(string username, string password, string postID)
        {
            var requestData = new Dictionary<string, string>
            {
                {"name", username},
                {"pass", password},
                {"postID", postID }
            };
            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync(accessName + "/thumbDown", new FormUrlEncodedContent(requestData));

                string responseString = await response.Content.ReadAsStringAsync();

                var parseResult = parseBinaryResponse(responseString);

                return parseResult;

            }
            catch
            {
                //Connection error
                return new Dictionary<String, String>
                {
                    {"code", "-1"}
                };
            }
        }

        public static async Task<Dictionary<string, string>> PostComment(string username, string password, string postID, string content)
        {
            var requestData = new Dictionary<string, string>
            {
                {"name", username},
                {"pass", password},
                {"postID", postID },
                {"content", content }
            };
            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync(accessName + "/commentUp", new FormUrlEncodedContent(requestData));

                string responseString = await response.Content.ReadAsStringAsync();

                var parseResult = parseBinaryResponse(responseString);

                return parseResult;
            }
            catch
            {
                //Connection error
                return new Dictionary<String, String>
                {
                    {"code", "-1"}
                };
            }
        }

        public static async Task<Dictionary<string, string>> AttemptSignUp(string username, string password, string repass,
                                                                            string phone, string email, StorageFile avatar)
        {
            HttpClient client = new HttpClient();
           
            Stream fileStream = await avatar.OpenStreamForReadAsync();
            var buffer = new byte[(int)fileStream.Length];
            fileStream.Read(buffer, 0, (int)fileStream.Length);

            ByteArrayContent content = new ByteArrayContent(buffer);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            string imgType = avatar.FileType;

            StringBuilder builder = new StringBuilder();
            builder.Append(accessName);
            builder.Append("/logup?name=");
            builder.Append(username);
            builder.Append("&pass=");
            builder.Append(password);
            builder.Append("&rePass=");
            builder.Append(repass);
            builder.Append("&phone=");
            builder.Append(phone);
            builder.Append("&email=");
            builder.Append(email);
            builder.Append("&imageType=");
            builder.Append(imgType);
            string url = builder.ToString();

            try
            {
                var response = await client.PostAsync(url, content);

                string responseString = await response.Content.ReadAsStringAsync();

                var parseResult = parseBinaryResponse(responseString);

                return parseResult;
            }
            catch
            {
                //Connection error
                return new Dictionary<String, String>
                {
                    {"code", "-1"}
                };
            }
        }

        private static List<String> imagePostfixes = new List<String> { ".jpg", ".jpeg", ".bmp", ".png", ".gif", ".tiff" };
        private static List<String> videoPostfixes = new List<String> { ".mp4", ".wmv", ".mkv", ".avi" };

        public static async Task<Dictionary<string, string>> PublishPost(string username, string password, string title,
                                                                            string passage, StorageFile media)
        {
            HttpClient client = new HttpClient();
            byte[] buffer;
            string fileType = "";
            if (media != null && (imagePostfixes.Contains(media.FileType.ToLower()) || 
                videoPostfixes.Contains(media.FileType.ToLower()))) {
                Stream fileStream = await media.OpenStreamForReadAsync();
                buffer = new byte[(int)fileStream.Length];
                fileStream.Read(buffer, 0, (int)fileStream.Length);
                fileType = media.FileType.ToLower();
            }
            else
            {
                buffer = new byte[1];
                buffer[0] = 0;
            }

            ByteArrayContent content = new ByteArrayContent(buffer);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
           

            bool isImage = imagePostfixes.Contains(fileType);
            bool isVideo = videoPostfixes.Contains(fileType);

            StringBuilder builder = new StringBuilder();
            builder.Append(accessName);
            builder.Append("/upload?name=");
            builder.Append(username);
            builder.Append("&pass=");
            builder.Append(password);
            builder.Append("&title=");
            builder.Append(title);
            builder.Append("&content=");
            builder.Append(passage);
            builder.Append("&imageType=");
            builder.Append(isImage ? fileType : "");
            builder.Append("&mediaType=");
            builder.Append(isVideo ? fileType : "");
            string url = builder.ToString();

            try
            {
                var response = await client.PostAsync(url, content);

                string responseString = await response.Content.ReadAsStringAsync();

                var parseResult = parseBinaryResponse(responseString);

                return parseResult;
            }
            catch
            {
                //Connection error
                return new Dictionary<String, String>
                {
                    {"code", "-1"}
                };
            }
        }

        public static async Task<Dictionary<string, string>> UserInfoModify(string username, string password, string phone, 
                                                                            string email, StorageFile avatar)
        {
            HttpClient client = new HttpClient();
            byte[] buffer;
            string imgType = "";
            if (avatar != null) {
                Stream fileStream = await avatar.OpenStreamForReadAsync();
                buffer = new byte[(int)fileStream.Length];
                fileStream.Read(buffer, 0, (int)fileStream.Length);
                imgType = avatar.FileType;
            }
            else
            {
                buffer = new byte[1];
                buffer[0] = 0;
            }
            ByteArrayContent content = new ByteArrayContent(buffer);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            

            StringBuilder builder = new StringBuilder();
            builder.Append(accessName);
            builder.Append("/modifyUserInfo?name=");
            builder.Append(username);
            builder.Append("&pass=");
            builder.Append(password);
            builder.Append("&newPhone=");
            builder.Append(phone);
            builder.Append("&newEmail=");
            builder.Append(email);
            builder.Append("&imgType=");
            builder.Append(imgType);
            string url = builder.ToString();

            try
            {
                var response = await client.PostAsync(url, content);

                string responseString = await response.Content.ReadAsStringAsync();

                var parseResult = parseBinaryResponse(responseString);

                return parseResult;
            }
            catch
            {
                //Connection error
                return new Dictionary<String, String>
                {
                    {"code", "-1"}
                };
            }
        }
    }
}
