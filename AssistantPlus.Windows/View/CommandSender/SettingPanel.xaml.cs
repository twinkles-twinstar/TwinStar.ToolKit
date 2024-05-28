#pragma warning disable 0,
// ReSharper disable

using AssistantPlus;
using AssistantPlus.Utility;

namespace AssistantPlus.View.CommandSender {

	public sealed partial class SettingPanel : CustomControl {

		#region life

		public SettingPanel (
		) {
			this.InitializeComponent();
			this.Controller = new () { View = this };
		}

		// ----------------

		private SettingPanelController Controller { get; }

		// ----------------

		protected override void StampUpdate (
		) {
			this.Controller.Update();
			return;
		}

		#endregion

		#region property

		public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
			nameof(SettingPanel.Data),
			typeof(Setting),
			typeof(SettingPanel),
			new (new Setting() {
				MethodConfiguration = "",
			})
		);

		public Setting Data {
			get => this.GetValue(SettingPanel.DataProperty).AsClass<Setting>();
			set => this.SetValue(SettingPanel.DataProperty, value);
		}

		#endregion

	}

	public class SettingPanelController : CustomController {

		#region data

		public SettingPanel View { get; init; } = default!;

		// ----------------

		public Setting Data => this.View.Data;

		#endregion

		#region update

		public async void Update (
		) {
			this.NotifyPropertyChanged(
				nameof(this.uMethodConfigurationText_Text)
			);
			return;
		}

		#endregion

		#region method configuration

		public async void uMethodConfigurationText_LostFocus (
			Object          sender,
			RoutedEventArgs args
		) {
			var senders = sender.AsClass<TextBox>();
			this.Data.MethodConfiguration = StorageHelper.Regularize(senders.Text);
			this.NotifyPropertyChanged(
				nameof(this.uMethodConfigurationText_Text)
			);
			return;
		}

		public String uMethodConfigurationText_Text {
			get {
				return this.Data.MethodConfiguration;
			}
		}

		public async void uMethodConfigurationPick_Click (
			Object          sender,
			RoutedEventArgs args
		) {
			var senders = sender.AsClass<Button>();
			var value = await StorageHelper.PickOpenFile(WindowHelper.Find(this.View), $"{nameof(CommandSender)}.MethodConfiguration");
			if (value is not null) {
				this.Data.MethodConfiguration = value;
				this.NotifyPropertyChanged(
					nameof(this.uMethodConfigurationText_Text)
				);
			}
			return;
		}

		#endregion

	}

}