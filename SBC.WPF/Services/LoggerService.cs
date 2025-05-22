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
		private readonly int _maxLogLines;
		private readonly ConcurrentQueue<string> _logQueue;

		public event Action<string> OnNewLogLine;

		public LoggerService(string logFilePath, int maxLogLines = 500)
		{
			_logFilePath = logFilePath;
			_maxLogLines = maxLogLines;
			_logQueue = new ConcurrentQueue<string>();
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

		public string GetCurrentLog()
		{
			return string.Join(Environment.NewLine, _logQueue);
		}

		public async Task SaveLogToFileAsync(string customPath)
		{
			var logData = GetCurrentLog();
			await Task.Run(() => File.WriteAllText(customPath, logData));
		}
	}
}
