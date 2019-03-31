using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MiniBook.Controls;
using MiniBook.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Button = Xamarin.Forms.Button;
using ButtonRenderer = Xamarin.Forms.Platform.Android.AppCompat.ButtonRenderer;

[assembly: ExportRenderer(typeof(Button), typeof(CustomButtonRenderer))]

namespace MiniBook.Droid.Renderers
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        public CustomButtonRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if(Control==null|| e.NewElement==null)
                return;

            Control.SetSupportAllCaps(false);
            Control.SetAllCaps(false);
        }
    }
}