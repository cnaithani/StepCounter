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


        BindingContext = new Dictionary<string, float>()
        {
            {"Apples",25},
            {"Bananas",13},
            {"Strawberries",25},
            {"Blueberries", 53},
            {"Oranges", 14},
            {"Grapes", 52},
            {"Watermelons", 15},
            {"Pears",34 },
            {"Cantalopes", 67},
            {"Citrus",53 },
            {"Starfruit", 43},
            {"Papaya", 22},
            {"Papassya", 22},
        };

    }

    protected override void OnAppearing()
    {
        currentVM = new MainPageVM();
        BindingContext = currentVM;
    }

}


