using Newtonsoft.Json;
using SBC.WPF.Enums;
using SBC.WPF.Interfaces;
using SBC.WPF.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBC.WPF.Services
{
	public class CInterface : ICInterface
	{
		private TestRoot _testData;

		public CInterface()
		{
			LoadTestData();
		}

		public void Connect(InterfaceConnection interfaceType, string comPortOrIp, int baudRateOrPort, string protocol)
		{

		}

		public void Disconnect(InterfaceConnection interfaceType)
		{

		}

		public int GetConnectionStatus(InterfaceConnection interfaceType)
		{
			return 1;
		}

		public void GetHWVersion(InterfaceConnection interfaceType, out HWVersion hwVersion)
		{
			hwVersion = HWVersion.SBC_6U;
		}


		public void GetVersionInfo(InterfaceConnection interfaceType, VersionInfo versionType, out string VersionInfo)
		{
			VersionInfo = $"Version for {versionType}: 1.0.0";
		}


		private void LoadTestData()
		{
			var jsonPath = Path.Combine(AppContext.BaseDirectory, "Resources", "Data", "Tests.json");
			if (!File.Exists(jsonPath))
			{
				_testData = new TestRoot { Test_Groups = [] };
				return;
			}

			var json = File.ReadAllText(jsonPath);
			_testData = JsonConvert.DeserializeObject<TestRoot>(json)
						?? new TestRoot { Test_Groups = [] };
		}

		public void RunTest(InterfaceConnection interfaceType, Group group, int subTest, out string logOutput)
		{
			try
			{
				int passCount = 0, failCount = 0;

				string groupKey = group.ToString(); // e.g., Group_Interface
				var selectedGroup = _testData.Test_Groups.FirstOrDefault(g => g.Type == groupKey);

				if (selectedGroup == null)
				{
					logOutput = $"Group '{groupKey}' not found.";
					return;
				}

				var sb = new StringBuilder();
				var rng = new Random(); // Random instance for simulation

				foreach (var test in selectedGroup.Testcases)
				{
					bool isSelected = (subTest & (1 << test.Bit)) != 0;
					if (isSelected)
					{
						bool passed = rng.Next(0, 2) == 0; // 0 or 1
						if (passed) passCount++; else failCount++;
						string result = passed ? "PASS" : "FAIL";
						sb.AppendLine($" {test.Name}: {result}");
					}
				}
				sb.AppendLine($"\nSummary: {passCount} Passed, {failCount} Failed");


				logOutput = sb.ToString();
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
