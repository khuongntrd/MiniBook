using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Diagnostics;
using MiniBook.ViewModels;
using System.Threading.Tasks;
using MiniBook.Views;
using MiniBook.Controls;
using System.Linq;

namespace MiniBook.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        protected Application CurrentApplication => Application.Current;

        public Task InitializeAsync()
        {
            return NavigateToAsync<LoginViewModel>();
        }


        public async Task NavigateToAsync(Type viewModelType, NavigationParameters parameters)
        {
            var view = FindViewByViewModel(viewModelType);

            if (view is LoginView)
            {
                CurrentApplication.MainPage = new CustomNavigationPage(view);
            }
            else if (view is DashboardView)
            {
                CurrentApplication.MainPage = new CustomNavigationPage(view);
            }
            else if (CurrentApplication.MainPage is CustomNavigationPage customNavigation)
            {
                await customNavigation.PushAsync(view);
            }
            else
            {
                CurrentApplication.MainPage = new CustomNavigationPage(view);
            }

            if (view.BindingContext is ViewModelBase vm)
            {
                await vm.OnNavigationAsync(parameters, NavigationType.New);
            }
        }
        public async Task NavigateBackAsync(NavigationParameters parameters)
        {
            if (CurrentApplication.MainPage is CustomNavigationPage navigationPage)
            {
                await navigationPage.PopAsync();

                if (navigationPage.Navigation.NavigationStack.LastOrDefault() is Page view)
                    if (view.BindingContext is ViewModelBase vm)
                    {
                        await vm.OnNavigationAsync(parameters, NavigationType.Back);
                    }
            }
        }

        public async Task NavigateBackToMainPageAsync()
        {
            if (!(CurrentApplication.MainPage is CustomNavigationPage))
                return;

            for (var i = CurrentApplication.MainPage.Navigation.NavigationStack.Count - 2; i > 0; i--)
                CurrentApplication.MainPage?.Navigation.RemovePage(CurrentApplication.MainPage.Navigation
                    .NavigationStack[i]);

            await CurrentApplication.MainPage.Navigation.PopAsync();
        }

        public Task NavigateBackAsync()
        {
            return NavigateBackAsync(null);
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return NavigateToAsync(typeof(TViewModel), new NavigationParameters());
        }

        public Task NavigateToAsync<TViewModel>(NavigationParameters parameters) where TViewModel : ViewModelBase
        {
            return NavigateToAsync(typeof(TViewModel), parameters);
        }

        public Task NavigateToAsync(Type viewModelType)
        {
            return NavigateToAsync(viewModelType, new NavigationParameters());
        }



        protected Page FindViewByViewModel(Type viewModelType)
        {
            try
            {
                var viewType = Type.GetType(viewModelType.FullName.Replace("ViewModel", "View"));

                if (viewType == null)
                    throw new Exception($"Mapping type for {viewModelType} is not a page");

                var view = Activator.CreateInstance(viewType) as Page;

                if (view != null)
                {                   
                    view.BindingContext = ServiceLocator.Instance.Resolve(viewModelType);
                }

                return view;
            }
            catch (Exception ex)
            {
                Debugger.Break();

                throw;
            }
        }
    }
}
