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

    //public static readonly BindableProperty BackGroundColorProperty = BindableProperty.Create(nameof(BackGroundColor),
    //typeof(string),
    //typeof(PieChart),
    //"#ffffff");
    //public string BackGroundColor
    //{
    //    get { return (string)GetValue(BackGroundColorProperty); }
    //    set
    //    {
    //        SetValue(BackGroundColorProperty, value);
    //        OnPropertyChanged(nameof(BackGroundColorProperty));
    //    }
    //}

    //public static readonly BindableProperty PrimaryColorProperty = BindableProperty.Create(nameof(PrimaryColor),
    //typeof(string),
    //typeof(PieChart),
    //"#6101ee");
    //public string PrimaryColor
    //{
    //    get { return (string)GetValue(PrimaryColorProperty); }
    //    set
    //    {
    //        SetValue(PrimaryColorProperty, value);
    //        OnPropertyChanged(nameof(PrimaryColorProperty));

    //    }
    //}

    public static readonly BindableProperty PercentProperty = BindableProperty.Create(nameof(Percent),
    typeof(decimal),
    typeof(PieChart),
    (Decimal)0.0);
    public decimal Percent
    {
        get { return (decimal)GetValue(PercentProperty); }
        set
        {
            SetValue(PercentProperty, value);
            OnPropertyChanged(nameof(PercentProperty));

        }
    }


    public PieChart() => InitializeComponent();
}

