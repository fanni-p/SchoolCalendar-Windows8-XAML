using System;
using System.Collections.Generic;
using TimetableApp.Common;
using TimetableApp.Services;

namespace TimetableApp.ViewModel
{
    public class ViewModelBase : BindableBase
    {
        public virtual void LoadState(object navParameter, Dictionary<string, object> viewModelState)
        {
        }

        public virtual void SaveState(Dictionary<string, object> viewModelState)
        {
        }

        public INavigationService NavigationService { get; set; }

        public virtual bool CanGoBack
        {
            get
            {
                if (this.NavigationService != null)
                {
                    return this.NavigationService.CanGoBack;
                }
                else
                {
                    return false;
                }
            }
        }

        public virtual bool CanGoForward
        {
            get
            {
                if (this.NavigationService != null)
                {
                    return this.NavigationService.CanGoForward;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
