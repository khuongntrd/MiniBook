// Copyright © 25inc.asia. All rights reserved.

using MiniBook.Services;
using MiniBook.Services.Navigation;
using MiniBook.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace MiniBook
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            BuildDependencies();

            InitNavigation();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            MessagingCenter.Send(this, "OnAppSleep");
        }

        protected override void OnStart()
        {
            ServiceLocator.Instance.Resolve<Services.Navigation.INavigationService>()
                .NavigateToAsync<ViewModels.SplashViewModel>();
        }

        void BuildDependencies()
        {
            if (ServiceLocator.Instance.Built)
                return;

            // Register dependencies
            ServiceLocator.Instance.RegisterInstance<Services.Dialog.IDialogService, Services.Dialog.DialogService>();
            ServiceLocator.Instance.RegisterInstance<Services.Navigation.INavigationService, Services.Navigation.NavigationService>();
            ServiceLocator.Instance.Register<Services.HttpService>();
            ServiceLocator.Instance.Register<Services.AccountService>();

            ServiceLocator.Instance.RegisterViewModels();

            ServiceLocator.Instance.Build();
        }

        void InitNavigation()
        {
            ServiceLocator.Instance.Resolve<INavigationService>()
                .NavigateToAsync<LoginViewModel>();
        }
    }
}