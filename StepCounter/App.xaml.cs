using StepCounter.Classes;
using StepCounter.Data;
using StepCounter.Interfaces;

namespace StepCounter;

public partial class App : Application
{
    static AppDatabase database;
    public static bool IsDatabaseInitialized = false;

    public App(IServiceProvider Provider)
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
            database.UpdateDatabase().ConfigureAwait(false);
            IsDatabaseInitialized = true;
        }
        //Task.Run(async () =>
        //{
        //    await database.UpdateDatabase();
        //    IsDatabaseInitialized = true;
        //});


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
            var commonDeviceHelper = new CommonDeviceHelper();
            database = new AppDatabase(await commonDeviceHelper.GetDBFile());
        }
    }
}

