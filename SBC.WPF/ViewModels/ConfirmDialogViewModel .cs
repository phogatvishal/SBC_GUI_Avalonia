using CommunityToolkit.Mvvm.ComponentModel;

namespace SBC.WPF.ViewModels
{
	public partial class ConfirmDialogViewModel : ObservableObject
	{
		[ObservableProperty]
		private string? _title = "Confirm Exit";

		[ObservableProperty]
		private string? _message = "Are you sure you want to exit?";

		[ObservableProperty]
		private bool _dontAskAgain;

		[ObservableProperty]
		private bool showYesNo = true;   // <-- Default for app exit

		[ObservableProperty]
		private bool showOk = false;     // <-- Set true when showing "Settings Applied"

		public ConfirmDialogViewModel()
        {
        }
    }
}
