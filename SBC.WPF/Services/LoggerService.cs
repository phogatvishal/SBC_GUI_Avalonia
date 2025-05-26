using SBC.WPF.Interfaces;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace SBC.WPF.Services
{
	public class LoggerService : ILoggerService
	{
		private readonly string _logFilePath;
		private readonly string _apilogFilePath;
		private readonly int _maxLogLines;
		private readonly ConcurrentQueue<string> _logQueue;
		private readonly ConcurrentQueue<string> _apilogQueue;

		public event Action<string> OnNewLogLine;

		public LoggerService(string logFilePath, string apilogFilePath, int maxLogLines = 500)
		{
			_logFilePath = $"{DateTime.Now.Date:yyyyMMdd}{logFilePath}";
			_apilogFilePath = $"{DateTime.Now.Date:yyyyMMdd}{apilogFilePath}";
			_maxLogLines = maxLogLines;
			_logQueue = new ConcurrentQueue<string>();
			_apilogQueue = new ConcurrentQueue<string>();
		}

		public void Log(string message)
		{
			string timestamped = $"[{DateTime.Now:HH:mm:ss}] {message}";
			_logQueue.Enqueue(timestamped);
			OnNewLogLine?.Invoke(timestamped);

			// Trim old logs
			while (_logQueue.Count > _maxLogLines)
				_logQueue.TryDequeue(out _);

			// Optionally write to file immediately
			File.AppendAllText(_logFilePath, timestamped + Environment.NewLine);
		}

		public void APILog(string message)
		{
			string timestamped = $"[{DateTime.Now:HH:mm:ss}] {message}";

			_apilogQueue.Enqueue(timestamped);
			while (_apilogQueue.Count > _maxLogLines)
				_apilogQueue.TryDequeue(out _);

			// Optionally write to file immediately
			File.AppendAllText(_apilogFilePath, timestamped + Environment.NewLine);
		}

		public string GetCurrentLog(ConcurrentQueue<string> queue)
		{
			return string.Join(Environment.NewLine, queue);
		}

		public async Task SaveLogToFileAsync(string customPath)
		{
			var logData = GetCurrentLog(_logQueue);
			await Task.Run(() => File.WriteAllText(customPath, logData));
		}

		public async Task SaveAPILogToFileAsync(string customPath)
		{
			var logData = GetCurrentLog(_apilogQueue);
			await Task.Run(() => File.WriteAllText(customPath, logData));
		}

		public async Task<string> ExportAllLogsAsync()
		{
			// Ensure the log file exists before reading
			if (File.Exists(_logFilePath))
			{
				return await Task.Run(() => File.ReadAllText(_logFilePath));
			}
			else
			{
				return string.Empty;
			}
		}
		public async Task<string> ExportAllAPILogsAsync()
		{
			// Ensure the log file exists before reading
			if (File.Exists(_apilogFilePath))
			{
				return await Task.Run(() => File.ReadAllText(_apilogFilePath));
			}
			else
			{
				return string.Empty;
			}
		}

		public Task<string> ExportCurrentAPILogsAsync()
		{
			var logData = GetCurrentLog(_apilogQueue);
			return Task.FromResult(logData);
		}

		public void ClearCurrentAPILogs()
		{
			while (_apilogQueue.TryDequeue(out _)) { }
		}
		public void ClearCurrentLogs()
		{
			while (_logQueue.TryDequeue(out _)) { }
		}
	}
}
