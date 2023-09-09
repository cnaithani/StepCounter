using System;
using Android.Content;
using StepCounter;
using StepCounter.Interfaces;

namespace StepCounter.Platforms.Android.Services
{
	public class StepServiceWapper:IStepServiceWapper
	{
        public void StartForegroundServiceCompat()
        {
            var intent = new Intent(MainActivity.Instance, typeof(StepService));
            MainActivity.Instance.StartService(intent);
        }
    }
}

