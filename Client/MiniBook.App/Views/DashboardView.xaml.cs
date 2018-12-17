using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiniBook.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardView
    {
        public DashboardView()
        {
            InitializeComponent();
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            if (CurrentPage != null)
            {
                SetViewModelByView(CurrentPage);
            }
        }

        private void SetViewModelByView(Page view)
        {
            try
            {
                if (view.BindingContext != BindingContext && view.BindingContext is ViewModels.ViewModelBase vm)
                {
                    vm.OnNavigationAsync(new Services.Navigation.NavigationParameters(), Services.Navigation.NavigationType.Back);

                    return;
                }

                var viewType = view.GetType();

                var viewModelType = Type.GetType(viewType.FullName.Replace("View", "ViewModel"));

                if (viewModelType == null)
                    throw new Exception($"Mapping type for {viewModelType} is not a page");

                vm = ServiceLocator.Instance.Resolve(viewModelType) as ViewModels.ViewModelBase;

                if (vm != null)
                {
                    view.BindingContext = vm;

                    vm.OnNavigationAsync(new Services.Navigation.NavigationParameters(), Services.Navigation.NavigationType.New);
                }


            }
            catch (Exception ex)
            {
                Debugger.Break();

                throw;
            }
        }
    }
}