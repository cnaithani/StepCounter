using StepCounter.Modules.AppSettings.ViewModels;

namespace StepCounter.Modules.AppSettings.Views;

public partial class AppSettings : ContentPage
{
    public AppSettingsVM currentVM;
    public AppSettings()
    {
        InitializeComponent();
    }

    protected override  void OnAppearing()
    {
        currentVM = new AppSettingsVM();
        BindingContext = currentVM;

        currentVM.Initialte();
    }

    void Ok_Clicked(System.Object sender, System.EventArgs e)
    {
        (App.Current.MainPage as Shell).GoToAsync("//Main/Home", true);
    }
}
