using System;
using System.ComponentModel;
using Android.App;
using AndroidApp = Android.App.Application;
using Android.Content;
using Android.Content.PM;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Nfc;
using Android.Util;
using AndroidX.LocalBroadcastManager.Content;
using Java.Util.Logging;
using Handler = Android.OS.Handler;
using Plugin.Maui.Pedometer;

namespace StepCounter.Platforms.Android.Services
{
    [Service(Enabled = true)]
    public class StepService :Service, INotifyPropertyChanged
    {
        private int StepsCounter = 0;
        public IBinder Binder { get; private set; }
        Handler handler;
        IPedometer Pedometer;

        public int Steps
        {
            get { return StepsCounter; } 
            set { StepsCounter = value; }
        }

        #region Service Methods
        public override void OnCreate()
        {
            base.OnCreate();

            handler = new Handler(Looper.MainLooper);
        }
        public override IBinder OnBind(Intent intent)
        {
            this.Binder = new StepServiceBinder(this);
            MainActivity.Instance.SetpService = this;

            ToggleAccelerometer();

            return this.Binder;
        }
        
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            StartTimer(TimeSpan.FromSeconds(10), () => {  return true; });
            return StartCommandResult.Sticky;
        }
        #endregion

        public void ToggleAccelerometer()
        {
            Pedometer = Microsoft.Maui.Controls.Application.Current.Handler.MauiContext.Services.GetService<IPedometer>();
            Pedometer.Start();
        }

        public void StartTimer(TimeSpan interval, Func<bool> callback)
        {
            var handler = new Handler(Looper.MainLooper);
            handler.PostDelayed(async () =>
            {
                await Refresh();
                if (callback())
                    StartTimer(interval, callback);

                handler.Dispose();
                handler = null;
            }, (long)interval.TotalMilliseconds);
        }

        public async Task Refresh()
        {
            //For Real Device
            //var totSteps = (int)Pedometer.TotalSteps;
            //await App.Database.SetCurrent(DateTime.Now, totSteps);
            //Steps = (await App.Database.GetCurrent()).Steps;
            //OnPropertyChanged("Steps");

            //For Emulator
            var rnd = new Random();
            await App.Database.SetCurrent(DateTime.Now, (int)rnd.NextInt64(1, 1000));
            Steps = (await App.Database.GetCurrent()).Steps;
            OnPropertyChanged("Steps");
        }


        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion

    }
}

