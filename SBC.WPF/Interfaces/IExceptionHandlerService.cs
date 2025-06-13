using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBC.WPF.Interfaces
{
	public interface IExceptionHandlerService
	{
		void RegisterGlobalHandlers();
		void HandleException(string title, Exception ex = null);
		Task<bool> ShowMessageAsync(string? title, string message);
	}
}
