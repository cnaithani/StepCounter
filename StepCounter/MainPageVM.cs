using Plugin.Maui.Pedometer;
using StepCounter.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepCounter
{
    public class MainPageVM : BaseViewModel
    {
        
        public MainPageVM()
        {
            //pedometer =  Application.Current.MainPage.Handler.MauiContext .Services.GetService<IPedometer>();
            //StartCounting();
        }

        //public int Steps { get; set; }

        //public void StartCounting()
        //{
        //    pedometer.ReadingChanged += (sender, reading) =>
        //    {
        //        Steps =reading.NumberOfSteps;
        //    };

        //    pedometer.Start();

        //}
        int steps = 0;
        public int Steps
        {
            get { return steps; }
            set
            {
                steps = value;
                OnPropertyChanged("Steps");
            }
        }
    }
}
