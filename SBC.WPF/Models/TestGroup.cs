using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace SBC.WPF.Models
{
	public partial class TestGroup : ObservableObject
	{
		[ObservableProperty]
		private string? name;

		[ObservableProperty]
		private string? type;

		[ObservableProperty]
		private string? description;

		[ObservableProperty]
		private bool? isAllSelected;

		[ObservableProperty]
		private ObservableCollection<TestCase> testcases = new();

        partial void OnIsAllSelectedChanged(bool? value)
		{
			if (!value.HasValue) return;

			foreach (var testCase in Testcases)
			{
				if (testCase.IsSelected != value.Value)
					testCase.IsSelected = value.Value;
			}
		}

		public void UpdateSelectAllCheckbox()
		{
			if (Testcases.Count == 0)
			{
				IsAllSelected = false;
				return;
			}

			IsAllSelected = Testcases.All(tc => tc.IsSelected)
				? true
				: Testcases.All(tc => !tc.IsSelected)
					? false
					: (bool?)null;
		}
	}
}
