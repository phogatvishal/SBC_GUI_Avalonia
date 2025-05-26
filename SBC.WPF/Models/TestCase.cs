using CommunityToolkit.Mvvm.ComponentModel;

namespace SBC.WPF.Models
{
	public partial class TestCase : ObservableObject
	{
		[ObservableProperty]
		private string? id;

		[ObservableProperty]
		private string? name;

		[ObservableProperty]
		private string? description;

		[ObservableProperty]
		private int bit;

		[ObservableProperty]
		private bool defaultSelected;

		[ObservableProperty]
		private string? compatible;

		[ObservableProperty]
		private TestGroup? parentGroup;

		[ObservableProperty]
		private bool isSelected;

		partial void OnIsSelectedChanged(bool value)
		{
			ParentGroup?.UpdateSelectAllCheckbox();
		}

		[ObservableProperty]
		private string statusColor = "Gray";

		private string _status = "UNKNOWN";
		public string Status
		{
			get => _status;
			set
			{
				if (SetProperty(ref _status, value))
				{
					StatusColor = value.ToUpperInvariant() switch
					{
						"PASS" => "#00BF10",
						"FAIL" => "Red",
						_ => "Gray"
					};
				}
			}
		}
	}
}
