using System;
using System.ComponentModel;
using Android.App;
using AndroidApp = Android.App.Application;
using Android.Content;
using Android.Content.PM;
using Android.Hardware;
using Android.OS;
using Android.Runtime;

namespace StepCounter.Platforms.Android.Services
{
    [Service(Enabled = true)]
    public class StepService :Service, ISensorEventListener, INotifyPropertyChanged
    {
        private int StepsCounter = 0;
        private SensorManager sManager;
        public IBinder Binder { get; private set; }

        public int Steps
        {
            get { return StepsCounter; } 
            set { StepsCounter = value; }
        }

        #region Service Methods
        public override IBinder OnBind(Intent intent)
        {
            //InitSensorService();

            this.Binder = new StepServiceBinder(this);
            MainActivity.Instance.SetpService = this;

            //Debug
            ToggleAccelerometer();
            //var startTimeSpan = TimeSpan.Zero;
            //var periodTimeSpan = TimeSpan.FromSeconds(30);

            //var timer = new System.Threading.Timer((e) =>
            //{
            //    OnSensorChangedDebug();
            //}, null, startTimeSpan, periodTimeSpan);


            return this.Binder;
        }
        public void ToggleAccelerometer()
        {
            if (Accelerometer.Default.IsSupported)
            {
                if (!Accelerometer.Default.IsMonitoring)
                {
                    // Turn on accelerometer
                    Accelerometer.Default.ReadingChanged += Accelerometer_ReadingChanged;
                    Accelerometer.Default.Start(SensorSpeed.UI);
                }
                else
                {
                    // Turn off accelerometer
                    Accelerometer.Default.Stop();
                    Accelerometer.Default.ReadingChanged -= Accelerometer_ReadingChanged;
                }
            }
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            StepsCounter++;
            OnPropertyChanged(nameof(StepsCounter));
        }
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            return StartCommandResult.Sticky;
        }

        #endregion

        public new void Dispose()
        {
            sManager.UnregisterListener(this);
            sManager.Dispose();
        }

        public void InitSensorService()
        {
            sManager = AndroidApp.Context.GetSystemService(Context.SensorService) as SensorManager;
            sManager.RegisterListener(this, sManager.GetDefaultSensor(SensorType.Accelerometer), SensorDelay.Normal);
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            Console.WriteLine("OnAccuracyChanged called");
        }

        public void OnSensorChangedDebug()
        {
            StepsCounter++; StepsCounter++; StepsCounter++;
            OnPropertyChanged(nameof(StepsCounter));
        }

        public void OnSensorChanged(SensorEvent e) 
        {
            StepsCounter++;
            OnPropertyChanged(nameof(StepsCounter));
        }

        public void StopSensorService()
        {
            sManager.UnregisterListener(this);
        }

        public bool IsAvailable()
        {
            return AndroidApp.Context.PackageManager.HasSystemFeature(PackageManager.FeatureSensorStepCounter) &&
                AndroidApp.Context.PackageManager.HasSystemFeature(PackageManager.FeatureSensorStepDetector);
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

