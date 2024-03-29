#pragma warning disable 0,
// ReSharper disable

using Helper;
using Helper.Utility;

namespace Helper.View.PackageBuilder {

	public sealed partial class SpecialWemResourcePropertyPanel : CustomControl {

		#region life

		public SpecialWemResourcePropertyPanel (
		) {
			this.InitializeComponent();
			this.Controller = new () { View = this };
		}

		// ----------------

		private SpecialWemResourcePropertyPanelController Controller { get; }

		// ----------------

		protected override void StampUpdate (
		) {
			this.Controller.Update();
			return;
		}

		#endregion

		#region property

		public static readonly DependencyProperty ConversionSourceProperty = DependencyProperty.Register(
			nameof(SpecialWemResourcePropertyPanel.ConversionSource),
			typeof(List<String>),
			typeof(SpecialWemResourcePropertyPanel),
			new (new List<String>())
		);

		public List<String> ConversionSource {
			get => this.GetValue(SpecialWemResourcePropertyPanel.ConversionSourceProperty).AsClass<List<String>>();
			set => this.SetValue(SpecialWemResourcePropertyPanel.ConversionSourceProperty, value);
		}

		// ----------------

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
			nameof(SpecialWemResourcePropertyPanel.Value),
			typeof(SpecialWemResourceProperty),
			typeof(SpecialWemResourcePropertyPanel),
			new (new SpecialWemResourceProperty() {
				Conversion = "",
				Path = "",
			})
		);

		public SpecialWemResourceProperty Value {
			get => this.GetValue(SpecialWemResourcePropertyPanel.ValueProperty).AsClass<SpecialWemResourceProperty>();
			set => this.SetValue(SpecialWemResourcePropertyPanel.ValueProperty, value);
		}

		#endregion

	}

	public class SpecialWemResourcePropertyPanelController : CustomController {

		#region data

		public SpecialWemResourcePropertyPanel View { get; init; } = default!;

		// ----------------

		public List<String> ConversionSource => this.View.ConversionSource;

		public SpecialWemResourceProperty Value => this.View.Value;

		#endregion

		#region update

		public async void Update (
		) {
			this.NotifyPropertyChanged(
				nameof(this.uConversion_ItemsSource),
				nameof(this.uConversion_SelectedItem),
				nameof(this.uPath_Text)
			);
			return;
		}

		#endregion

		#region conversion

		public List<String> uConversion_ItemsSource {
			get {
				return this.ConversionSource;
			}
		}

		public String uConversion_SelectedItem {
			get {
				return this.Value.Conversion;
			}
		}

		public async void uConversion_SelectionChanged (
			Object                    sender,
			SelectionChangedEventArgs args
		) {
			var senders = sender.AsClass<ComboBox>();
			this.Value.Conversion = senders.SelectedItem.AsClass<String>();
			return;
		}

		#endregion

		#region path

		public String uPath_Text {
			get {
				return this.Value.Path;
			}
		}

		public async void uPath_TextChanged (
			Object               sender,
			TextChangedEventArgs args
		) {
			var senders = sender.AsClass<TextBox>();
			this.Value.Path = senders.Text;
			return;
		}

		#endregion

	}

}
