using System.Threading.Tasks;
using Xamarin.Forms;

namespace MiniBook.Controls.Behaviors
{

    public class AutoscrollBehavior : Behavior<ScrollView>
    {
        protected override void OnAttachedTo(ScrollView bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.SizeChanged += ScrollViewSizeChanged;

        }

        protected override void OnDetachingFrom(ScrollView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.SizeChanged -= ScrollViewSizeChanged;
        }


        public static readonly BindableProperty FormLayoutProperty =
            BindableProperty.Create(nameof(FormLayout), typeof(View), typeof(AutoscrollBehavior));

        public View FormLayout
        {
            get => (View)GetValue(FormLayoutProperty);
            set => SetValue(FormLayoutProperty, value);
        }



        private async void ScrollViewSizeChanged(object sender, System.EventArgs e)
        {
            var scrollView = sender as ScrollView;
            if (scrollView == null)
                return;

            var mainLayout = FormLayout ?? scrollView.Content;

            if (_defaultHeight == 0)
            {
                _defaultHeight = scrollView.Height;
            }
            else
            {
                if (scrollView.Height < _defaultHeight)
                {
                    if (scrollView.Height > _lastHeight && _lastHeight != 0D)
                        return;
                    _lastHeight = scrollView.Height;
                    await Task.Delay(50);
                    await scrollView.ScrollToAsync(mainLayout, ScrollToPosition.End, true);
                }
                else
                {
                    _lastHeight = 0;
                }
            }
        }
        double _lastHeight = 0;
        double _defaultHeight = 0;
    }
}
