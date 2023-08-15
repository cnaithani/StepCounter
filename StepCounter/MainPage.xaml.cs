using Microsoft.Toolkit.Mvvm.Messaging;
using StepCounter.Global;

namespace StepCounter;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();

        WeakReferenceMessenger.Default.Register<StepStepUpdateMsg>(this, (m, e) =>
        {
            CounterBtn.Text = $"Steps {e.Steps} ";
        });

    }

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}


