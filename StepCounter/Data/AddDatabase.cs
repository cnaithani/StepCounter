using System;
using SQLite;
using StepCounter.Models;

namespace StepCounter.Data
{
    public class AppDatabase
    {
        public SQLiteAsyncConnection database;
        private DatabaseUpdates updates;
        public AppDatabase(string dbPath)
        {
            initializeDB(dbPath);
        }
        public bool IsInitialized { get; set; } = false;

        public async void initializeDB(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            await database.EnableWriteAheadLoggingAsync();
        }
        public async Task UpdateDatabase()
        {
            updates = new DatabaseUpdates();
            await updates.UpdateDatabase();
            IsInitialized = true;
        }

        public async Task SetCurrent(DateTime timeStamp, int totalSteps)
        {
            if (App.IsDatabaseInitialized == false)
                return ;

            var current = await GetCurrent();
            int steps = totalSteps - current.TotalSteps;
            if (steps > 0)
            {
                current.Steps += steps;
                current.TotalSteps = totalSteps;
                current.Timestamp = timeStamp;
                await App.Database.database.UpdateAsync(current);
            }
            else if(steps < 0)
            {
                current.Steps = 0;
                current.TotalSteps = totalSteps;
                current.Timestamp = timeStamp;
                await App.Database.database.UpdateAsync(current);
            }

        }

        public async Task<CurrentCounter> GetCurrent()
        {
            if (App.IsDatabaseInitialized == false)
                return null;
            var current = await App.Database.database.Table<CurrentCounter>().FirstOrDefaultAsync();
            if (current == null)
            {
                current = new CurrentCounter();
                current.Timestamp = DateTime.Now;
                current.TotalSteps = 0;
                current.Steps = 0;
                await App.Database.database.InsertAsync(current);
            }
            return current;
        }
    }
}

