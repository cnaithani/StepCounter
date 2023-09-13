using Microsoft.Toolkit.Mvvm.Messaging;
using Plugin.Maui.Pedometer;
using StepCounter.Global;

namespace StepCounter;

public partial class MainPage : ContentPage
{
	int count = 0;
    public MainPageVM currentVM;

    public MainPage()
	{
		InitializeComponent();

        WeakReferenceMessenger.Default.Register<StepStepUpdateMsg>(this, (m, e) =>
        {
            lblSteps.Text = "Steps: " + e.Steps.ToString();
        });

    }

    protected override void OnAppearing()
    {
        currentVM = new MainPageVM();
        BindingContext = currentVM;
    }

}


