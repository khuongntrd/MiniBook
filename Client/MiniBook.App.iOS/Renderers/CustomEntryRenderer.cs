using CoreGraphics;
using MiniBook.Controls;
using MiniBook.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace MiniBook.iOS.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                UITextField textField = Control;
                textField.BorderStyle = UITextBorderStyle.None;
                textField.ClearButtonMode = UITextFieldViewMode.WhileEditing;
            }
        }
    }
}