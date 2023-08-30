using System;
namespace StepCounter.Models
{
	public class Step
	{
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public Boolean IsActive { get; set; }
        public int ServerId { get; set; }
        public DateTime Modified { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Steps { get; set; }
    }
}

