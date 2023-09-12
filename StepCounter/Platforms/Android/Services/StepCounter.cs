using System;
using System.ComponentModel;
using Android.App;
using AndroidApp = Android.App.Application;
using AC = Android.Content;
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
using Android.Appwidget;
using MauiWidgets.Platforms.Android;
using static Android.Provider.CalendarContract;

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
        public override IBinder OnBind(AC.Intent intent)
        {
            return null;
        }
        public override void OnDestroy()
        {
            //base.OnDestroy();
        }

        public override StartCommandResult OnStartCommand(AC.Intent intent, StartCommandFlags flags, int startId)
        {
            RegisterForegroundService();
            ToggleAccelerometer();
            StartTimer(TimeSpan.FromSeconds(10), () => {  return true; });
            return StartCommandResult.Sticky ;
        }
        private NotificationManager notificationManager;
        Notification notification;
        Notification.Builder notificationBuilder;
        void RegisterForegroundService()
        {
            CreateNotificationChannel();

            // Enlist this instance of the service as a foreground service
            StartForeground(1000, notification);
        }
        void CreateNotificationChannel()
        {
            notificationBuilder = new Notification.Builder(MainActivity.Instance, "StepCounterChannelId");

            var builder = notificationBuilder
                                .SetContentTitle("Steps")
                                .SetContentText(Steps.ToString())
                                .SetColorized(true)
                                .SetOngoing(true);

            notification = builder.Build();

            var channelName = "StepCounterChannel";
            var channelDescription = "Step Counts";
            var channel = new NotificationChannel("StepCounterChannelId", channelName, NotificationImportance.Low)
            {
                Description = channelDescription
            };

            notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        void UpdateNotificationChannel()
        {
            try
            {
                notificationBuilder.SetContentText(Steps.ToString());
                notificationManager.Notify(10000, notificationBuilder.Build());
            }
            catch (Exception ex)
            {
                var str = ex.Message;
            }
        }
        #endregion

        #region Step Counter
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

        private void UpdateWidget()
        {
            var intent = new AC.Intent(this, typeof(SampleWidget));
            intent.SetAction("android.appwidget.action.APPWIDGET_UPDATE");
            var ids = AppWidgetManager.GetInstance(MainActivity.Instance).GetAppWidgetIds(new AC.ComponentName(MainActivity.Instance, Java.Lang.Class.FromType(typeof(SampleWidget)).Name));
            intent.PutExtra(AppWidgetManager.ExtraAppwidgetIds, ids);
            SendBroadcast(intent);
        }

        public async Task Refresh()
        {
            //For Real Device
            var totSteps = (int)Pedometer.TotalSteps;
            await App.Database.SetCurrent(DateTime.Now, totSteps);
            Steps = (await App.Database.GetCurrent()).Steps;
            UpdateWidget();
            OnPropertyChanged("Steps");

            //For Emulator
            //var rnd = new Random();
            //await App.Database.SetCurrent(DateTime.Now, (int)rnd.NextInt64(1, 1000));
            //Steps = (await App.Database.GetCurrent()).Steps;
            //UpdateWidget();
            //OnPropertyChanged("Steps");
        }
        #endregion

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

