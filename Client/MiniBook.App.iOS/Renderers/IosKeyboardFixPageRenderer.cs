using Foundation;
using MiniBook.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(IosKeyboardFixPageRenderer))]

namespace MiniBook.iOS.Renderers
{
    public class IosKeyboardFixPageRenderer : PageRenderer
    {
        private Thickness? _basePadding;
        private NSObject _observerHideKeyboard;
        private NSObject _observerShowKeyboard;

        public IosKeyboardFixPageRenderer()
        {
            MessagingCenter.Subscribe<App>(this, "OnAppSleep", OnAppSleep);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            _observerHideKeyboard =
                NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
            _observerShowKeyboard =
                NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            NSNotificationCenter.DefaultCenter.RemoveObserver(_observerHideKeyboard);
            NSNotificationCenter.DefaultCenter.RemoveObserver(_observerShowKeyboard);

            if (Element is Page page && _basePadding != null) page.Padding = _basePadding.Value;
        }

        private void OnAppSleep(App obj)
        {
            if (Element is ContentPage cp && _basePadding != null) cp.Padding = _basePadding.Value;
        }

        private void OnKeyboardNotification(NSNotification notification)
        {
            if (!IsViewLoaded) return;

            var frameBegin = UIKeyboard.FrameBeginFromNotification(notification);
            var frameEnd = UIKeyboard.FrameEndFromNotification(notification);

            if (Element is ContentPage page) // && !(page.Content is ScrollView))
            {
                var padding = page.Padding;
                if (_basePadding == null)
                    _basePadding = padding;
                page.Padding = new Thickness(padding.Left, padding.Top, padding.Right,
                    padding.Bottom + frameBegin.Top - frameEnd.Top);
            }
        }
    }
}