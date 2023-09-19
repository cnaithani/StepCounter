namespace StepCounter.Charts.Pie;

public partial class PieChart : StackLayout
{


    public static readonly BindableProperty DisplayProperty = BindableProperty.Create(nameof(Display),
        typeof(string),
        typeof(PieChart),
        string.Empty,
        propertyChanged: async (bindable, oldValue, newValue) =>
        {
            var chartView = ((PieChart)bindable);
            chartView.Chart.PieChartDrawable.Display = (string)newValue;
        });
    public string Display
    {
        get => (string )GetValue(DisplayProperty);
        set => SetValue(DisplayProperty, value);
    }

    public PieChart() => InitializeComponent();
}

