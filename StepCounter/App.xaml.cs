using StepCounter.Data;
using StepCounter.Interfaces;

namespace StepCounter;

public partial class App : Application
{
    static AppDatabase database;
    static bool isDatabaseInitialized = false;
    public App()
	{
		InitializeComponent();

        if (Preferences.Get("APPTHEME", "Light") == "Dark")
        {
            Application.Current.UserAppTheme = AppTheme.Dark;
        }
        else
        {
            Application.Current.UserAppTheme = AppTheme.Light;
        }


        if (database == null)
        {
            InitiateDB().ConfigureAwait(false);
        }
        Task.Run(async () =>
        {
            await database.UpdateDatabase();
            isDatabaseInitialized = true;
        });

        MainPage = new AppShell();
	}

    public static AppDatabase Database
    {
        get
        {
            if (database == null)
            {
                InitiateDB().ConfigureAwait(false);
            }
            return database;
        }
    }
    private static async Task InitiateDB()
    {
        if (database == null)
        {
            var commonDeviceHandler = App.Current.Handler.MauiContext.Services.GetServices<ICommonDeviceHelper>().FirstOrDefault();
            database = new AppDatabase(await commonDeviceHandler.GetDBFile());
        }
    }
}

