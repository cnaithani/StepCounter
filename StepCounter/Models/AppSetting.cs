using System;
namespace StepCounter.Models
{
	public class AppSetting
	{
        [SQLite.PrimaryKey]
        public int Id { get; set; }
        public int ServerId { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
    }
}

