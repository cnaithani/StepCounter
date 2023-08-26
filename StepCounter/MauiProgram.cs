using Microsoft.Extensions.Logging;
using Plugin.Maui.Pedometer;

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




        return builder.Build();
	}
}

