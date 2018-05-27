using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace KnowItClient
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        
        private async void Logup_Click(object sender, RoutedEventArgs e)
        {
            System.Net.Http.HttpClient hc = new System.Net.Http.HttpClient();
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.GetFileAsync("yellow.jpg");
            string apiUrl = "http://127.0.0.1:18080/logup/?";
            if (file != null)
            {
                var stream = await file.OpenStreamForReadAsync();
                var bytes = new byte[(int)stream.Length];
                //using(var reader = new DataReader(stream))
                //{
                //    await reader.LoadAsync((uint)stream.Size);
                //    reader.ReadBytes(bytes);
                //}
                stream.Read(bytes, 0, (int)stream.Length);
                apiUrl += "name=" + username.Text + "&pass=" + password.Text + "&phone=" + null + "&email=" + email.Text
                            + "&imageType=.jpg&rePass=" + rePass.Text;
                System.Net.Http.ByteArrayContent content = new System.Net.Http.ByteArrayContent(bytes);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                var response = await hc.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(response.Content);
                }
            }
            
        }
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            HttpClient hc = new HttpClient();
            var content = new HttpFormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"name" , username.Text },
                {"pass", password.Text },
            });
            var response = await hc.PostAsync(new Uri("http://127.0.0.1:18080/login/"), content);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsBufferAsync();
                Debug.WriteLine(response.Content);
            }
        }

        private async void addPost_Click(object sender, RoutedEventArgs e)
        {
            HttpClient hc = new HttpClient();
            var content = new HttpFormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"name" , username.Text },
                {"pass", password.Text },
                {"imageStream", "" },
                {"imageType", imageType.Text },
                {"title", title.Text },
                {"content", content1.Text },
                {"mediaType", mediaType.Text },
                {"mediaStream", "" }
            });
            var response = await hc.PostAsync(new Uri("http://127.0.0.1:18080/upload/"), content);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsBufferAsync();
                Debug.WriteLine(response.Content);
            }
        }

        private async void Data_Click(object sender, RoutedEventArgs e)
        {
            HttpClient hc = new HttpClient();
            var content = new HttpFormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"name" , username.Text },
                {"pass", password.Text },
                {"id", "1" }
            });
            var response = await hc.PostAsync(new Uri("http://127.0.0.1:18080/data/"), content);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsBufferAsync();
                Debug.WriteLine(response.Content);
            }
        }

        private async void Info_Click(object sender, RoutedEventArgs e)
        {
            HttpClient hc = new HttpClient();
            var content = new HttpFormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"name" , username.Text },
            });
            var response = await hc.PostAsync(new Uri("http://127.0.0.1:18080/getUserInfo/"), content);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsBufferAsync();
                Debug.WriteLine(response.Content);
            }
        }
    }
}
