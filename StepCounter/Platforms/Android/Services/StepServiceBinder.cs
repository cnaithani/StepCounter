using System;
using Android.OS;
namespace StepCounter.Platforms.Android.Services
{
    public class StepServiceBinder : Binder
    {
        StepService stepService;
        public StepServiceBinder(StepService service)
        {
            this.stepService = service;
        }

        public StepService StepService
        {
            get { return stepService; }
        }
    }
}

