using System.Collections.Generic;

namespace MiniBook.Services.Navigation
{
    public class NavigationParameters : Dictionary<string, object>
    {
        public bool TryGetValue<T>(string key, out T value)
        {
            if (base.TryGetValue(key, out object result))
            {
                if (result is T variable)
                {
                    value = variable;
                    return true;
                }
            }

            value = default(T);

            return false;
        }

        public object Default
        {
            get
            {
                if (TryGetValue("Default", out object obj))
                    return obj;

                return null;
            }
        }

        public NavigationParameters()
        {

        }
        public NavigationParameters(object defaultParameter)
        {
            Add("Default", defaultParameter);
        }
    }
}
