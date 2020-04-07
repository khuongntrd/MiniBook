using Android.Content;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using MiniBook.Droid.Renderers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using AWidget = Android.Widget;

[assembly: ExportRenderer(typeof(Xamarin.Forms.TabbedPage), typeof(CustomBottomTabbedPageRenderer))]
namespace MiniBook.Droid.Renderers
{
    public class CustomBottomTabbedPageRenderer : TabbedPageRenderer
    {
        public CustomBottomTabbedPageRenderer(Context context) : base(context)
        {
        }
        bool IsBottomTabPlacement => (Element != null) && Element.OnThisPlatform().GetToolbarPlacement() == ToolbarPlacement.Bottom;

        BottomNavigationView _bottomNavigationView;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TabbedPage> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
                ((IPageController)e.OldElement).InternalChildren.CollectionChanged -= OnChildrenCollectionChanged;

            if (e.NewElement != null)
            {
                ((IPageController)e.NewElement).InternalChildren.CollectionChanged += OnChildrenCollectionChanged;

                RemoveShiftMode(_bottomNavigationView);
            }

            var childViews = GetAllChildViews(ViewGroup);

            var scale = Resources.DisplayMetrics.Density;
            var paddingDp = 9;
            var dpAsPixels = (int)(paddingDp * scale + 0.5f);

            foreach (var childView in childViews)
            {
                if (childView is BottomNavigationItemView tab)
                {
                    tab.SetPadding(tab.PaddingLeft, dpAsPixels, tab.PaddingRight, tab.PaddingBottom);
                }
                else if (childView is AWidget.TextView textView)
                {
                    textView.SetTextColor(Android.Graphics.Color.Transparent);
                }
            }

        }
        List<Android.Views.View> GetAllChildViews(Android.Views.View view)
        {
            if (!(view is ViewGroup group))
            {
                return new List<Android.Views.View> { view };
            }

            var result = new List<Android.Views.View>();

            for (int i = 0; i < group.ChildCount; i++)
            {
                var child = group.GetChildAt(i);

                var childList = new List<Android.Views.View> { child };
                childList.AddRange(GetAllChildViews(child));

                result.AddRange(childList);
            }

            return result.Distinct().ToList();
        }
        private void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RemoveShiftMode(_bottomNavigationView);
        }

        public override void AddView(Android.Views.View child)
        {
            if (IsBottomTabPlacement)
                if (child is AWidget.RelativeLayout layout)
                {
                    if (layout.GetChildAt(1) is BottomNavigationView bnv)
                    {
                        _bottomNavigationView = bnv;

                        RemoveShiftMode(_bottomNavigationView);
                    }
                }
            base.AddView(child);
        }


        public static void RemoveShiftMode(BottomNavigationView bottomNavigationView)
        {
            try
            {
                if (bottomNavigationView == null)
                    return;
                var menuView = (BottomNavigationMenuView)bottomNavigationView.GetChildAt(0);
                var shiftingMode = Java.Lang.Class.FromType(typeof(BottomNavigationMenuView)).GetDeclaredField("mShiftingMode");
                shiftingMode.Accessible = true;
                shiftingMode.SetBoolean(menuView, false);
                shiftingMode.Accessible = false;

                for (var i = 0; i < menuView.ChildCount; i++)
                {
                    var item = (BottomNavigationItemView)menuView.GetChildAt(i);
                    var icon = (AppCompatImageView)item.GetChildAt(0);

                    var layoutParameters = (AWidget.FrameLayout.LayoutParams)icon.LayoutParameters;
                    layoutParameters.Gravity = GravityFlags.Center;
                    item.SetShifting(false);
                    item.SetChecked(item.ItemData.IsChecked);
                }

            }
            catch (Exception e)
            {
            }
        }
    }
}