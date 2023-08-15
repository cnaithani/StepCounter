using System;
using System.ComponentModel;
using Android.App;
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
        //TODO - Check
        public override IBinder OnBind(Intent intent)
        {
            this.Binder = new StepServiceBinder(this);
            MainActivity.Instance.SetpService = this;
            return this.Binder;
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
            sManager = MainActivity.Instance.GetSystemService(Context.SensorService) as SensorManager;
            sManager.RegisterListener(this, sManager.GetDefaultSensor(SensorType.StepCounter), SensorDelay.Normal);

        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            Console.WriteLine("OnAccuracyChanged called");
        }

        public void OnSensorChanged(SensorEvent e)
        {
            StepsCounter++;
            Console.WriteLine(e.ToString());
        }

        public void StopSensorService()
        {
            sManager.UnregisterListener(this);
        }

        public bool IsAvailable()
        {
            return MainActivity.Instance.PackageManager.HasSystemFeature(PackageManager.FeatureSensorStepCounter) &&
                MainActivity.Instance.PackageManager.HasSystemFeature(PackageManager.FeatureSensorStepDetector);
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

