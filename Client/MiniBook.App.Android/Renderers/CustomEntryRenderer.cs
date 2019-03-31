using System.ComponentModel;
using Android.Content;
using Android.Graphics.Drawables;
using MiniBook.Controls;
using MiniBook.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]

namespace MiniBook.Droid.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {

        public CustomEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null || e.NewElement == null)
                return;

            Control.Background = new ColorDrawable(Color.Transparent.ToAndroid());
        }
    }
}