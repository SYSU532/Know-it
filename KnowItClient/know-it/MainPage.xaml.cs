using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Composition;
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
            
            InitializeFrostedGlass(GlassHost);
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

        private async void FileUpload_Click(object sender, RoutedEventArgs e)
        {
            var fop = new FileOpenPicker();
            fop.FileTypeFilter.Add(".jpg");
            fop.FileTypeFilter.Add(".jpeg");
            fop.FileTypeFilter.Add(".png");
            fop.FileTypeFilter.Add(".mp4");
            fop.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            var file = await fop.PickSingleFileAsync();
            if (file != null)
            {

                Stream fileStream = await file.OpenStreamForReadAsync();
                

                StorageFolder appLocalFolder = ApplicationData.Current.LocalFolder;
                StorageFile newFile = await appLocalFolder.CreateFileAsync("fucking jian-yang.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);


                var buffer = new byte[(int)fileStream.Length];
                fileStream.Read(buffer, 0, (int)fileStream.Length);

                await FileIO.WriteBytesAsync(newFile, buffer);
            }



        }

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;

            if (rootFrame != null)
            {
                rootFrame.Navigate(typeof(SignupPage), UserNameBox.Text);
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
