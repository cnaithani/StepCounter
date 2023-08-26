using System;
namespace StepCounter.Models
{
	public class CurrentCounter
	{
        [SQLite.PrimaryKey]
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int Steps { get; set; }
    }
}

