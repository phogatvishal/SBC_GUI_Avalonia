using Avalonia.Controls;
using SBC.WPF.Models;
using System;
using System.Threading.Tasks;

namespace SBC.WPF.Interfaces
{
	public interface IExceptionHandlerService
	{
		void RegisterGlobalHandlers();
		void HandleException(string title, Exception ex);
	
		Task<ExceptionDialogResult> ShowExceptionDialogAsync(
			string title,
			string message,
			string? details = null,
			bool canRetry = false,
			Window? parentWindow = null);
	}
}