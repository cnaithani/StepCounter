using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.ConstraintLayout.Core.Motion.Utils;
using StepCounter.Platforms.Android.Services;

namespace StepCounter;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public static MainActivity Instance { get; private set; }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        Instance = this;
    }

    public bool IsBound { get; set; }
    private StepServiceBinder binder;
    private bool registered;
    private StepServiceConnection serviceConnection;

    public StepServiceBinder Binder
    {
        get { return binder; }
        set
        {
            binder = value;
            if (binder == null)
                return;

            //trigger UI refresh on first bind
            HandlePropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs("StepsToday"));

            if (registered)
                binder.StepService.PropertyChanged -= HandlePropertyChanged;

            binder.StepService.PropertyChanged += HandlePropertyChanged;
            registered = true;
        }

    }

    private void StartStepService()
    {

        try
        {
            var service = new Intent(this, typeof(StepService));
            var componentName = StartService(service);
        }
        catch (Exception ex)
        {
            //TODO - Check
        }

    }

    protected override void OnStart()
    {
        base.OnStart();

        //TODO - Check
        //if (!firstRun)
        StartStepService();

        if (IsBound)
            return;

        var serviceIntent = new Intent(this, typeof(StepService));
        serviceConnection = new StepServiceConnection(this);
        BindService(serviceIntent, serviceConnection, Bind.AutoCreate);
    }

    protected override void OnStop()
    {
        base.OnStop();
        if (IsBound)
        {
            UnbindService(serviceConnection);
            IsBound = false;
        }
    }

    void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName != "StepsToday")
            return;

        //TODO - Check
        //UpdateUI();
    }
}

