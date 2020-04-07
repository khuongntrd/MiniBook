using CoreGraphics;
using MiniBook.iOS.Renderers;
using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Frame), typeof(CustomFrameRenderer))]

namespace MiniBook.iOS.Renderers
{
    public class CustomFrameRenderer : VisualElementRenderer<Frame>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            try
            {
                if (e.NewElement == null)
                    return;

                base.OnElementChanged(e);

                if (e.NewElement != null && Element != null)
                    SetupLayer();
            }
            catch (Exception)
            {
                //
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName ||
                e.PropertyName == Xamarin.Forms.Frame.BorderColorProperty.PropertyName ||
                e.PropertyName == Xamarin.Forms.Frame.HasShadowProperty.PropertyName ||
                e.PropertyName == Xamarin.Forms.Frame.CornerRadiusProperty.PropertyName)
                SetupLayer();
        }

        private void SetupLayer()
        {
            if (Layer == null || Element == null)
                return;

            var cornerRadius = Element.CornerRadius;

            if (cornerRadius == -1f)
                cornerRadius = 5f; // default corner radius

            Layer.CornerRadius = cornerRadius;

            Layer.BackgroundColor = Element.BackgroundColor == Color.Default
                ? UIColor.White.CGColor
                : Element.BackgroundColor.ToCGColor();

            if (Element.HasShadow)
            {
                Layer.MasksToBounds = false;
                Layer.ShadowColor = Color.FromHex("#000").ToCGColor();
                Layer.ShadowOffset = new CGSize(0, 0);
                Layer.ShadowOpacity = 0.3f;
            }
            else
            {
                Layer.ShadowOpacity = 0;
            }

            if (Element.BorderColor == Color.Default)
            {
                Layer.BorderColor = UIColor.Clear.CGColor;
            }
            else
            {
                Layer.BorderColor = Element.BorderColor.ToCGColor();
                Layer.BorderWidth = 1;
            }

            Layer.RasterizationScale = UIScreen.MainScreen.Scale;
            Layer.ShouldRasterize = true;
        }
    }
}