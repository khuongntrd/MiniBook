using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using NavigationPage = Xamarin.Forms.NavigationPage;
using Page = Xamarin.Forms.Page;

namespace MiniBook.Controls
{
    public class CustomNavigationPage : NavigationPage
    {
        public CustomNavigationPage(Page root) : base(root)
        {
            BarTextColor = Color.White;

            On<iOS>()
                .SetStatusBarTextColorMode(StatusBarTextColorMode.MatchNavigationBarTextLuminosity);
        }
    }
}
