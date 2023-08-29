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

        WeakReferenceMessenger.Default.Register<StepStepUpdateMsg>(this, async (m, e) =>
        {
            if (App.IsDatabaseInitialized == false)
                return;

            var currentS = await App.Database.GetCurrent();
            lblSteps.Text = "Steps: " + currentS.Steps.ToString();
        });

    }

    protected override void OnAppearing()
    {
        currentVM = new MainPageVM();
        BindingContext = currentVM;
    }

    private void OnCounterClicked(object sender, EventArgs e)
	{
		//count++;

		//if (count == 1)
		//	CounterBtn.Text = $"Clicked {count} time";
		//else
		//	CounterBtn.Text = $"Clicked {count} times";

		//SemanticScreenReader.Announce(CounterBtn.Text);

	}
}


