using SBC.WPF.Interfaces;
using SBC.WPF.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SBC.WPF.Services
{
	public class TestLoaderService : ITestLoaderService
	{
		public async Task<List<TestGroup>> LoadTestGroupsAsync(string jsonFilePath)
		{
			try
			{
				string json = await Task.Run(() => File.ReadAllText(jsonFilePath));

				var parsed = JsonSerializer.Deserialize<TestRoot>(json, new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				});

				foreach (var group in parsed.Test_Groups)
				{
					foreach (var test in group.Testcases)
					{
						test.IsSelected = test.Default_Selected;
					}
				}

				return parsed.Test_Groups;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
