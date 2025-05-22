using System;
using System.Threading.Tasks;

namespace SBC.WPF.Interfaces
{
	public interface ILoggerService
	{
		event Action<string> OnNewLogLine;
		void Log(string message);
		string GetCurrentLog();
		Task SaveLogToFileAsync(string customPath);
	}
}
