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

namespace StepCounter.Platforms.Android.Services
{
    [Service(Enabled = true)]
    public class StepService :Service, INotifyPropertyChanged
    {
        private int StepsCounter = 0;
        public IBinder Binder { get; private set; }
        Action runnableLog;
        Handler handler;
        Action runnableNotification;
        volatile bool canRun;

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

            //Debug
            ToggleAccelerometer();

            return this.Binder;
        }
        
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            //runnableLog = new Action(() =>
            //{
            //    if (handler == null) return;
            //    OnPropertyChanged("Steps");
            //    handler.PostDelayed(runnableLog, 1000);
            //});

            StartTimer(TimeSpan.FromSeconds(10), () => {  return true; });
            return StartCommandResult.Sticky;
        }

        public void StartTimer(TimeSpan interval, Func<bool> callback)
        {
            var handler = new Handler(Looper.MainLooper);
            handler.PostDelayed(() =>
            {
                OnPropertyChanged("Steps");
                if (callback())
                    StartTimer(interval, callback);

                handler.Dispose();
                handler = null;
            }, (long)interval.TotalMilliseconds);
        }

        #endregion

        public void ToggleAccelerometer()
        {
            if (Accelerometer.Default.IsSupported)
            {
                if (!Accelerometer.Default.IsMonitoring)
                {
                    // Turn on accelerometer
                    Accelerometer.Default.ReadingChanged += Accelerometer_ReadingChanged;
                    Accelerometer.Default.Start(SensorSpeed.Game);
                }
                else
                {
                    // Turn off accelerometer
                    Accelerometer.Default.Stop();
                    Accelerometer.Default.ReadingChanged -= Accelerometer_ReadingChanged;
                }
            }
        }
        bool inStep= false;
        //List<double> values = new List<decimal>();
        private async void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            if (inStep == true)
                return;

            inStep = true;

            float x = e.Reading.Acceleration.X;
            float y = e.Reading.Acceleration.Y;
            float z = e.Reading.Acceleration.Z;

            if (x==0 || y==0 || z==0)
            {
                inStep = false;
                return;
            }

            var currentvectorSum = Math.Sqrt(x* x + y* y + z* z);
           
            if(currentvectorSum > 1.45){
                //values.Add(currentvectorSum);
                //StepsCounter = values.Average();

                StepsCounter++;
                await Task.Delay(200);
            }
            inStep = false;
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

