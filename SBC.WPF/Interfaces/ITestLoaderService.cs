using SBC.WPF.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SBC.WPF.Interfaces
{
	public interface ITestLoaderService
	{
		Task<List<TestGroup>> LoadTestGroupsAsync(string filePath);
	}
}
