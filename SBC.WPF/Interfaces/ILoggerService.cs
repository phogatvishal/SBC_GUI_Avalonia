using System;
using System.Threading.Tasks;

namespace SBC.WPF.Interfaces
{
	public interface ILoggerService
	{
		event Action<string> OnNewLogLine;
		void Log(string message);
		void APILog(string message);
		Task SaveLogToFileAsync(string customPath);
		Task SaveAPILogToFileAsync(string customPath);
		Task<string> ExportAllAPILogsAsync();
		Task<string> ExportCurrentAPILogsAsync();
		void ClearCurrentAPILogs();
		void ClearCurrentLogs();
	}
}
