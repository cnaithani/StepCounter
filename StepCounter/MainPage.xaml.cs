using Microsoft.Toolkit.Mvvm.Messaging;
using Plugin.Maui.Pedometer;
using StepCounter.Global;

namespace StepCounter;

public partial class MainPage : ContentPage
{
	int count = 0;
    readonly IPedometer pedometer;

    public MainPage(IPedometer pedometer)
	{
		InitializeComponent();

        //WeakReferenceMessenger.Default.Register<StepStepUpdateMsg>(this, (m, e) =>
        //{
        //    CounterBtn.Text = $"Steps {e.Steps} ";
        //});

        this.pedometer = pedometer;
        StartCounting();

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

    public void StartCounting()
    {
        pedometer.ReadingChanged += (sender, reading) =>
        {
            CounterBtn.Text = $"Steps: {reading.NumberOfSteps.ToString()}";
        };

        pedometer.Start();
    }
}


