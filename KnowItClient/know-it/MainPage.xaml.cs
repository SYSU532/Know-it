using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Composition;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace know_it
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
       
        public MainPage()
        {
            this.InitializeComponent();
            
            //InitializeFrostedGlass(GlassHost);
        }
        
        private string serverName {
            get { return NetworkControl.AccessingURI; }
            set { NetworkControl.AccessingURI = value; }
        }

        private void InitializeFrostedGlass(UIElement glassHost)
        {
            Visual hostVisual = ElementCompositionPreview.GetElementVisual(glassHost);
            Compositor compositor = hostVisual.Compositor;
            var backdropBrush = compositor.CreateHostBackdropBrush();
            var glassVisual = compositor.CreateSpriteVisual();
            glassVisual.Brush = backdropBrush;
            ElementCompositionPreview.SetElementChildVisual(glassHost, glassVisual);
            var bindSizeAnimation = compositor.CreateExpressionAnimation("hostVisual.Size");
            bindSizeAnimation.SetReferenceParameter("hostVisual", hostVisual);
            glassVisual.StartAnimation("Size", bindSizeAnimation);
        }

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;

            if (rootFrame != null)
            {
                rootFrame.Navigate(typeof(SignupPage), UserNameBox.Text);
            }
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var res = await NetworkControl.AttemptSignin(UserNameBox.Text, PasswordBox.Password);
            string msg = "";
            bool success = false;
            if (res["code"] == "-1")
            {
                msg = "Connection to server appears to be down, please check.";
            }
            else if (res["code"] == "-2")
            {
                msg = "Ughh... The server doesn't seem to be a Know-it Server, please check again.";
            }
            else if (res["code"] == "0")
            {
                msg = res["errMessage"];
            }
            else if (res["code"] == "1")
            {
                msg = "Success!";
                success = true;
            }
            MessageDialog dialog = new MessageDialog(msg);
            await dialog.ShowAsync();
            if (success)
            {
                await NetworkControl.PostComment(UserNameBox.Text, PasswordBox.Password, "1", "shit...");
                var re = await NetworkControl.GetPostFromID(UserNameBox.Text, PasswordBox.Password, "1");

                //go to content page, passing a pair with username as key and password as value as parameter.
            }
        }

        private async void test()
        {
            var fop = new FileOpenPicker();
            fop.FileTypeFilter.Add(".jpg");
            fop.FileTypeFilter.Add(".jpeg");
            fop.FileTypeFilter.Add(".png");
            fop.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            var file = await fop.PickSingleFileAsync();
            await NetworkControl.PublishPost(UserNameBox.Text, PasswordBox.Password, "ThirdTry", "Fucking Jian Yang!", file);
            await NetworkControl.GiveThumbToPost(UserNameBox.Text, PasswordBox.Password, "2");
            var result = await NetworkControl.GetPostFromID(UserNameBox.Text, PasswordBox.Password, "2");
            if (result["image"] != null)
            {
                string fullPath = NetworkControl.accessName + "/img/" + result["image"];
                Logo.Source = new BitmapImage(new Uri(fullPath, UriKind.Absolute));
                Debug.WriteLine(result["thumbs"]);
            }
        }
    }
}
