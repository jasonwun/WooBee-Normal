using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Security.Authentication.Web;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WooBee_MVVMLight.Common;
using WooBee_MVVMLight.View;

namespace WooBee_MVVMLight.ViewModel
{
    public class LoginViewModel:ViewModelBase, INotifyPropertyChanged
    {

        public LoginViewModel() { }

        private bool _extendedButtonEnabled = true;
        public bool ExtendedButtonEnabled
        {
            get
            {
                return _extendedButtonEnabled;
            }
            set
            {
                Set(nameof(ExtendedButtonEnabled), ref _extendedButtonEnabled, value);
                NotifyPropertyChanged();
            }

        }

        private bool _normalButtonEnabled = true;
        public bool NormalButtonEnabled
        {
            get
            {
                return _normalButtonEnabled;
            }
            set
            {
                Set(nameof(NormalButtonEnabled), ref _normalButtonEnabled, value);
                NotifyPropertyChanged();
            }
        }




        private RelayCommand _normalLoginCommand;

        /// <summary>
        /// Gets the NormalLoginCommand.
        /// </summary>
        public RelayCommand NormalLoginCommand
        {
            get
            {
                return _normalLoginCommand = new RelayCommand(() =>
                {
                    NormalLoginTask();
                });
            }
        }

        private void NormalLoginTask()
        {
            try
            {
                NavigationService.NaivgateToPage(typeof(WebViewXAML), "normal");
            }
            catch
            {
                string aa = "";
            }
        }

        private RelayCommand _extendedLoginCommand;

        /// <summary>
        /// Gets the _extendedLoginCommand.
        /// </summary>
        public RelayCommand ExtendedLoginCommand
        {
            get
            {
                return _extendedLoginCommand
                    ?? (_extendedLoginCommand = new RelayCommand(() =>
                    {
                        ExtendedLoginTask();
                    }));
            }
        }

        private void ExtendedLoginTask()
        {
            NavigationService.NaivgateToPage(typeof(WebViewXAML), "weico");
        }

        // PropertyChanged event.
        public event PropertyChangedEventHandler PropertyChanged;

        // PropertyChanged event triggering method.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
