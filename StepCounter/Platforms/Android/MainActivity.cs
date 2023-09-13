using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.ConstraintLayout.Core.Motion.Utils;
using MauiWidgets.Platforms.Android;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using StepCounter.Global;
using StepCounter.Interfaces;
using StepCounter.Platforms.Android.Services;

namespace StepCounter;

[Activity(Theme = "@style/Maui.MainTheme.NoActionBar", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public static MainActivity Instance { get; private set; }
    public StepService SetpService { get;  set; }
    private Android.Content.Intent StepServiceIntent;
    private IStepServiceWapper serviceWrapper;

    public MainActivity()
    {
        serviceWrapper = App.Current.Handler.MauiContext.Services.GetServices<IStepServiceWapper>().FirstOrDefault();
    }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        Instance = this;
    }

    private void StartStepService()
    {
        try
        {
            serviceWrapper.StartForegroundServiceCompat();
        }
        catch (Exception ex)
        {
            //TODO - Check
            var str = ex.Message;
        }

    }

    protected override void OnStart()
    {
        base.OnStart();

        //TODO - Check

        if (StepServiceIntent == null)
            StartStepService();
    }

    protected async override void OnResume()
    {
        base.OnResume();
        if (SetpService != null)
            await SetpService.Refresh();
    }

    protected override void OnStop()
    {
        base.OnStop();
    }
}

