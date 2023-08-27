using System;
using StepCounter.Global;
using StepCounter.Models;

namespace StepCounter.Modules.AppSettings.ViewModels
{
	public class AppSettingsVM: BaseViewModel
    {
        private bool _isDarkTheme;
        private int _target;

        public AppSettingsVM()
        {
            _isDarkTheme = Application.Current.UserAppTheme == AppTheme.Dark;
            
        }

        public bool IsDarkTheme
        {
            get
            {
                return _isDarkTheme;

            }
            set
            {
                _isDarkTheme = value;
                if (!_isDarkTheme)
                {
                    Application.Current.UserAppTheme = AppTheme.Light;
                    Preferences.Set(Constants.ThemeCaption, Constants.LightThemeName);

                }
                else
                {
                    Application.Current.UserAppTheme = AppTheme.Dark;
                    OnPropertyChanged("IsDarkTheme");
                    Preferences.Set(Constants.ThemeCaption, Constants.LightThemeName);
                }
            }
        }

        public int Target
        {
            get
            {
                return _target;

            }
            set
            {
                _target = value;
                OnPropertyChanged("Target");
            }
        }

        public async void Initialte()
        {
            var targetSetting = await App.Database.database.Table<AppSetting>().FirstOrDefaultAsync(x => x.Code == "TARGET");
            if (targetSetting == null)
            {
                targetSetting = new AppSetting();
                targetSetting.Code = "TARGET";
                targetSetting.Value = "1000";
                await App.Database.database.InsertAsync(targetSetting);
            }
            if (int.TryParse(targetSetting.Value, out _target))
            {
                Target = _target;
            }
            
        }
    }
}

