using System;
using StepCounter.Models;

namespace StepCounter.Data
{
	public class DatabaseUpdates
	{
        public const int LAST_DATABASE_VERSION = 2;
        public async Task UpdateDatabase()
        {
            int currentDbVersion = 2;
            currentDbVersion = await GetDatabaseVersion();

            if (currentDbVersion < LAST_DATABASE_VERSION)
            {
                int startUpgradingFrom = currentDbVersion + 1;
                switch (startUpgradingFrom)
                {
                    case 1: //starting version
                        goto case 2;
                    case 2:
                        UpgradeFrom1To2();
                        break;
                    default:
                        break;
                }
            }
            UpgradeFrom1To2();
        }
        private async Task<int> GetDatabaseVersion()
        {
            return await App.Database.database.ExecuteScalarAsync<int>("PRAGMA user_version");
        }
        private async Task<int> SetDatabaseToVersion(int version)
        {
            return await App.Database.database.ExecuteAsync("PRAGMA user_version =" + version.ToString());
        }
        internal async void UpgradeFrom1To2()
        {
            await App.Database.database.CreateTableAsync<CurrentCounter>();
            await App.Database.database.CreateTableAsync<Step>();
            await App.Database.database.CreateTableAsync<AppSetting>();
        }
    }
}

