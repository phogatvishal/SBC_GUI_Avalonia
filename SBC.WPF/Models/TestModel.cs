using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace SBC.WPF.Models
{
	public class TestGroup
	{
		public string? Name { get; set; }
		public string? Type { get; set; }
		public string? Description { get; set; }

		public ObservableCollection<TestCase> Testcases { get; set; } = new();

		private bool? _isAllSelected;
		public bool? IsAllSelected
		{
			get => _isAllSelected;
			set
			{
				if (_isAllSelected != value)
				{
					_isAllSelected = value;
					OnPropertyChanged(nameof(IsAllSelected));
					UpdateTestCaseSelection(value);
				}
			}
		}

		private void UpdateTestCaseSelection(bool? value)
		{
			if (value.HasValue)
			{
				foreach (var testCase in Testcases)
					testCase.IsSelected = value.Value;
			}
		}

		public void UpdateIsAllSelectedFromCases()
		{
			if (Testcases.All(tc => tc.IsSelected))
				IsAllSelected = true;
			else if (Testcases.All(tc => !tc.IsSelected))
				IsAllSelected = false;
			else
				IsAllSelected = null;
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		private void OnPropertyChanged(string propertyName) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public class TestCase:  INotifyPropertyChanged
	{
		public string? Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public int Bit { get; set; }
		public bool Default_Selected { get; set; }
		public string? Compatible { get; set; }

		public TestGroup? ParentGroup { get; set; }

		private bool _isSelected;
		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				if (_isSelected != value)
				{
					_isSelected = value;
					OnPropertyChanged(nameof(IsSelected));
					ParentGroup?.UpdateIsAllSelectedFromCases();
				}
			}
		}

		private string _statusColor = "Gray";
		public string StatusColor
		{
			get => _statusColor;
			set
			{
				if (_statusColor != value)
				{
					_statusColor = value;
					OnPropertyChanged(nameof(StatusColor));
				}
			}
		}

		private string _status = "UNKNOWN";
		public string Status
		{
			get => _status;
			set
			{
				if (_status != value)
				{
					_status = value;
					OnPropertyChanged(nameof(Status));
					StatusColor = value.ToUpper() == "PASS" ? "#00BF10" :
								  value.ToUpper() == "FAIL" ? "Red" : "Gray";
				}
			}
		}

		public ICommand? OnSelectionChangedCommand { get; set; }


		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public class TestRoot
	{
		public List<TestGroup> Test_Groups { get; set; }
	}
}
