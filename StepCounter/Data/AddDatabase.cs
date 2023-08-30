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

            //if (timeStamp.Date < current.Timestamp.Date)
            //{
            //    current.Steps = 0;
            //    current.Steps = 0;
            //}

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

            var lastEntry = await App.Database.database.Table<Step>().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            if (lastEntry == null)
            {
                lastEntry = new Step();
                lastEntry.StartTime = (new DateTime(current.Timestamp.Year, current.Timestamp.Month, current.Timestamp.Day)).AddHours(current.Timestamp.Hour-2);
                lastEntry.EndTime = (new DateTime(current.Timestamp.Year, current.Timestamp.Month, current.Timestamp.Day)).AddHours(current.Timestamp.Hour - 1);
                lastEntry.Steps = 0;
                await App.Database.database.InsertAsync(lastEntry);
            }
            if ((current.Timestamp - lastEntry.StartTime).Hours > 0)
            {
                var lastEntrynew = new Step();
                lastEntrynew.StartTime = (new DateTime(current.Timestamp.Year, current.Timestamp.Month, current.Timestamp.Day)).AddHours(current.Timestamp.Hour - 1);
                lastEntrynew.EndTime = (new DateTime(current.Timestamp.Year, current.Timestamp.Month, current.Timestamp.Day)).AddHours(current.Timestamp.Hour );
                lastEntrynew.Steps = 0;
                await App.Database.database.InsertAsync(lastEntrynew);
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

