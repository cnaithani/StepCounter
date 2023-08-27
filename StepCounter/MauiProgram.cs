using Microsoft.Extensions.Logging;
using Plugin.Maui.Pedometer;
using StepCounter.Interfaces;
using StepCounter.Classes;

namespace StepCounter;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton(Pedometer.Default);

#if ANDROID
#elif IOS
#else
	//builder.Services.AddSingleton(Pedometer.Default);
#endif

        builder = RegisterAppServices(builder);

        return builder.Build();
	}

    public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<ICommonDeviceHelper, CommonDeviceHelper>();
        return mauiAppBuilder;
    }
}

