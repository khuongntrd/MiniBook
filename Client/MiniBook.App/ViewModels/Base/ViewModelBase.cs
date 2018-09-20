using System;
using System.Collections.Generic;
using System.Text;

namespace MiniBook.ViewModels
{
    public class ViewModelBase : Mvvm.BindableBase
    {
        private string _title;
        private bool _isBusy;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }


        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value, () => RaisePropertyChanged(nameof(IsNotBusy)));
        }

        public bool IsNotBusy => !IsBusy;
    }
}
