using System;
namespace StepCounter.Interfaces
{
	public interface ICommonDeviceHelper
	{
        string GetLocalFilePath(string filename);

        Task<string> GetDBFile();

        string CopyDBFile();
    }
}

