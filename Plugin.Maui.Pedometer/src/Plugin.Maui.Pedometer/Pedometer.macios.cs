﻿using CoreMotion;
using Microsoft.Maui.ApplicationModel;

namespace Plugin.Maui.Pedometer;

partial class FeatureImplementation : IPedometer
{
	readonly CMPedometer pedometer;

	public FeatureImplementation()
	{
		pedometer = new();
	}

	public bool IsSupported => CMPedometer.IsStepCountingAvailable;

	public bool IsMonitoring { get; private set; }
	public double TotalSteps { get { return 0; } }

	public event EventHandler<PedometerData>? ReadingChanged;

	public void Start()
	{
		if (!IsSupported)
		{
			throw new FeatureNotSupportedException();
		}

		if (IsMonitoring)
		{
			return;
		}

		IsMonitoring = true;

		pedometer.StartPedometerUpdates(NSDate.Now, (data, error) =>
		{
			ReadingChanged?.Invoke(this, new()
			{
				NumberOfSteps = (int)data.NumberOfSteps,
				Timestamp = DateTimeOffset.Now,
			});
		});
	}

	public void Stop()
	{
		if (!IsSupported)
		{
			throw new FeatureNotSupportedException();
		}

		if (!IsMonitoring)
		{
			return;
		}

		pedometer.StopPedometerUpdates();

		IsMonitoring = false;
	}
}