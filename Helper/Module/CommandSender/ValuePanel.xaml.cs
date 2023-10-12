#pragma warning disable 0,
// ReSharper disable

using Helper;
using Helper.Utility;
using Windows.ApplicationModel.DataTransfer;
using Windows.Globalization.NumberFormatting;

namespace Helper.Module.CommandSender {

	public sealed partial class ValuePanel : UserControl {

		#region life

		public ValuePanel (
		) {
			this.InitializeComponent();
			this.Controller = new ValuePanelController() { View = this };
		}

		// ----------------

		private ValuePanelController Controller { get; }

		// ----------------

		private void UpdateVisualState (
		) {
			VisualStateManager.GoToState(this, $"{(this.Option is not null ? "Enumeration" : this.Type is null ? "Null" : this.Type)}State", false);
			return;
		}

		#endregion

		#region property

		public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
			nameof(ValuePanel.Type),
			typeof(ArgumentType),
			typeof(ValuePanel),
			new PropertyMetadata(null)
		);

		public ArgumentType? Type {
			get => this.GetValue(ValuePanel.TypeProperty) as ArgumentType?;
			set => this.SetValue(ValuePanel.TypeProperty, value);
		}

		// ----------------

		public static readonly DependencyProperty OptionProperty = DependencyProperty.Register(
			nameof(ValuePanel.Option),
			typeof(List<Object>),
			typeof(ValuePanel),
			new PropertyMetadata(null)
		);

		public List<Object>? Option {
			get => this.GetValue(ValuePanel.OptionProperty) as List<Object>;
			set => this.SetValue(ValuePanel.OptionProperty, value);
		}

		// ----------------

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
			nameof(ValuePanel.Value),
			typeof(ArgumentValue),
			typeof(ValuePanel),
			new PropertyMetadata(null)
		);

		public ArgumentValue? Value {
			get => this.GetValue(ValuePanel.ValueProperty) as ArgumentValue;
			set => this.SetValue(ValuePanel.ValueProperty, value);
		}

		// ----------------

		public static readonly DependencyProperty StampProperty = DependencyProperty.Register(
			nameof(ValuePanel.Stamp),
			typeof(UniqueStamp),
			typeof(ValuePanel),
			new PropertyMetadata(UniqueStamp.Default, (o, e) => {
				(o as ValuePanel)!.UpdateVisualState();
				(o as ValuePanel)!.Controller.Update();
			})
		);

		public UniqueStamp Stamp {
			get => this.GetValue(ValuePanel.StampProperty) as UniqueStamp ?? throw new Exception();
			set => this.SetValue(ValuePanel.StampProperty, value);
		}

		#endregion

	}

	public class ValuePanelController : CustomController {

		#region data

		public ValuePanel View { get; init; } = default!;

		// ----------------

		public ArgumentType? Type => this.View.Type;

		public List<Object>? Option => this.View.Option;

		public ArgumentValue? Value => this.View.Value;

		#endregion

		#region update

		public async void Update (
		) {
			if (this.Type is not null && this.Value is not null) {
				this.NotifyPropertyChanged(
					nameof(this.uAction_IsEnabled)
				);
				if (this.Option is not null) {
					this.NotifyPropertyChanged(
						nameof(this.uEnumerationValue_ItemsSource),
						nameof(this.uEnumerationValue_SelectedValue)
					);
				} else {
					switch (this.Type) {
						case ArgumentType.Boolean: {
							this.NotifyPropertyChanged(
								nameof(this.uBooleanValue_Text)
							);
							break;
						}
						case ArgumentType.Integer: {
							this.NotifyPropertyChanged(
								nameof(this.uIntegerValue_Value)
							);
							break;
						}
						case ArgumentType.Floater: {
							this.NotifyPropertyChanged(
								nameof(this.uFloaterValue_Value)
							);
							break;
						}
						case ArgumentType.Size: {
							this.NotifyPropertyChanged(
								nameof(this.uSizeValue_Value),
								nameof(this.uSizeUnit_SelectedItem)
							);
							break;
						}
						case ArgumentType.String: {
							this.NotifyPropertyChanged(
								nameof(this.uStringValue_Text)
							);
							break;
						}
						case ArgumentType.Path: {
							this.NotifyPropertyChanged(
								nameof(this.uPathValue_Text)
							);
							break;
						}
						default: throw new ArgumentOutOfRangeException();
					}
				}
			}
			return;
		}

		#endregion

		#region action

		public Boolean uAction_IsEnabled {
			get {
				if (this.Value is not { Data: not null }) { return false; }
				return true;
			}
		}

		// ----------------

		public String uBooleanValue_Text {
			get {
				if (this.Type is not ArgumentType.Boolean || this.Option is not null || this.Value is not { Data: Boolean }) { return ""; }
				return ConvertHelper.BooleanToConfirmationStringLower(this.Value.OfBoolean);
			}
		}

		public async void uBooleanValue_OnClick (
			Object          sender,
			RoutedEventArgs args
		) {
			if (sender is not Button senders) { return; }
			if (this.Type is not ArgumentType.Boolean || this.Option is not null || this.Value is not { Data: Boolean }) { return; }
			this.Value.OfBoolean = !this.Value.OfBoolean;
			this.NotifyPropertyChanged(
				nameof(this.uBooleanValue_Text)
			);
			return;
		}

		// ----------------

		public DecimalFormatter uIntegerValue_NumberFormatter {
			get {
				return new DecimalFormatter() { IntegerDigits = 1, FractionDigits = 0 };
			}
		}

		public Floater uIntegerValue_Value {
			get {
				if (this.Type is not ArgumentType.Integer || this.Option is not null || this.Value is not { Data: Integer }) { return Floater.NaN; }
				return (Floater)this.Value.OfInteger;
			}
		}

		public async void uIntegerValue_OnValueChanged (
			NumberBox                      sender,
			NumberBoxValueChangedEventArgs args
		) {
			if (sender is not NumberBox senders) { return; }
			if (this.Type is not ArgumentType.Integer || this.Option is not null || this.Value is not { Data: Integer }) { return; }
			if (!Floater.IsNaN(args.NewValue)) {
				this.Value.OfInteger = (Integer)args.NewValue;
			}
			this.NotifyPropertyChanged(
				nameof(this.uIntegerValue_Value)
			);
			return;
		}

		// ----------------

		public DecimalFormatter uFloaterValue_NumberFormatter {
			get {
				return new DecimalFormatter() { IntegerDigits = 1, FractionDigits = 1 };
			}
		}

		public Floater uFloaterValue_Value {
			get {
				if (this.Type is not ArgumentType.Floater || this.Option is not null || this.Value is not { Data: Floater }) { return Floater.NaN; }
				return this.Value.OfFloater;
			}
		}

		public async void uFloaterValue_OnValueChanged (
			NumberBox                      sender,
			NumberBoxValueChangedEventArgs args
		) {
			if (sender is not NumberBox senders) { return; }
			if (this.Type is not ArgumentType.Floater || this.Option is not null || this.Value is not { Data: Floater }) { return; }
			if (!Floater.IsNaN(args.NewValue)) {
				this.Value.OfFloater = args.NewValue;
			}
			this.NotifyPropertyChanged(
				nameof(this.uFloaterValue_Value)
			);
			return;
		}

		// ----------------

		public DecimalFormatter uSizeValue_NumberFormatter {
			get {
				return new DecimalFormatter() { IntegerDigits = 1, FractionDigits = 1 };
			}
		}

		public Floater uSizeValue_Value {
			get {
				if (this.Type is not ArgumentType.Size || this.Option is not null || this.Value is not { Data: SizeExpression }) { return Floater.NaN; }
				return this.Value.OfSize.Value;
			}
		}

		public async void uSizeValue_OnValueChanged (
			NumberBox                      sender,
			NumberBoxValueChangedEventArgs args
		) {
			if (sender is not NumberBox senders) { return; }
			if (this.Type is not ArgumentType.Size || this.Option is not null || this.Value is not { Data: SizeExpression }) { return; }
			if (!Floater.IsNaN(args.NewValue)) {
				this.Value.OfSize.Value = args.NewValue;
			}
			this.NotifyPropertyChanged(
				nameof(this.uSizeValue_Value)
			);
			return;
		}

		public List<SizeUnit> uSizeUnit_ItemsSource {
			get {
				return new List<SizeUnit>() { SizeUnit.B, SizeUnit.K, SizeUnit.M, SizeUnit.G };
			}
		}

		public SizeUnit uSizeUnit_SelectedItem {
			get {
				if (this.Type is not ArgumentType.Size || this.Option is not null || this.Value is not { Data: SizeExpression }) { return SizeUnit.B; }
				return this.Value.OfSize.Unit;
			}
		}

		public async void uSizeUnit_OnSelectionChanged (
			Object                    sender,
			SelectionChangedEventArgs args
		) {
			if (sender is not ComboBox senders) { return; }
			if (this.Type is not ArgumentType.Size || this.Option is not null || this.Value is not { Data: SizeExpression }) { return; }
			this.Value.OfSize.Unit = senders.SelectedItem as SizeUnit? ?? throw new Exception();
			return;
		}

		// ----------------

		public String uStringValue_Text {
			get {
				if (this.Type is not ArgumentType.String || this.Option is not null || this.Value is not { Data: String }) { return ""; }
				return this.Value.OfString;
			}
		}

		public async void uStringValue_OnTextChanged (
			Object               sender,
			TextChangedEventArgs args
		) {
			if (sender is not TextBox senders) { return; }
			if (this.Type is not ArgumentType.String || this.Option is not null || this.Value is not { Data: String }) { return; }
			this.Value.OfString = senders.Text;
			this.NotifyPropertyChanged(
				nameof(this.uStringValue_Text)
			);
			return;
		}

		// ----------------

		public String uPathValue_Text {
			get {
				if (this.Type is not ArgumentType.Path || this.Option is not null || this.Value is not { Data: PathExpression }) { return ""; }
				return this.Value.OfPath.Value;
			}
		}

		public async void uPathValue_OnTextChanged (
			Object               sender,
			TextChangedEventArgs args
		) {
			if (sender is not TextBox senders) { return; }
			if (this.Type is not ArgumentType.Path || this.Option is not null || this.Value is not { Data: PathExpression }) { return; }
			this.Value.OfPath.Value = senders.Text;
			this.NotifyPropertyChanged(
				nameof(this.uPathValue_Text)
			);
			return;
		}

		public async void uPathValue_OnDragOver (
			Object        sender,
			DragEventArgs args
		) {
			if (sender is not TextBox senders) { return; }
			if (this.Type is not ArgumentType.Path || this.Option is not null || this.Value is not { Data: PathExpression }) { return; }
			if (args.DataView.Contains(StandardDataFormats.StorageItems)) {
				args.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Link;
			}
			return;
		}

		public async void uPathValue_OnDrop (
			Object        sender,
			DragEventArgs args
		) {
			if (sender is not TextBox senders) { return; }
			if (this.Type is not ArgumentType.Path || this.Option is not null || this.Value is not { Data: PathExpression }) { return; }
			args.Handled = true;
			if (args.DataView.Contains(StandardDataFormats.StorageItems)) {
				var newValue = StorageHelper.Regularize((await args.DataView.GetStorageItemsAsync())[0].Path);
				this.Value.OfPath.Value = newValue;
				this.NotifyPropertyChanged(
					nameof(this.uPathValue_Text)
				);
			}
			return;
		}

		public async void uPathPickFile_OnClick (
			Object          sender,
			RoutedEventArgs args
		) {
			if (sender is not MenuFlyoutItem senders) { return; }
			if (this.Type is not ArgumentType.Path || this.Option is not null || this.Value is not { Data: PathExpression }) { return; }
			var newValue = await StorageHelper.PickFile(WindowHelper.GetForElement(this.View));
			if (newValue is not null) {
				this.Value.OfPath.Value = newValue;
				this.NotifyPropertyChanged(
					nameof(this.uPathValue_Text)
				);
			}
			return;
		}

		public async void uPathPickDirectory_OnClick (
			Object          sender,
			RoutedEventArgs args
		) {
			if (sender is not MenuFlyoutItem senders) { return; }
			if (this.Type is not ArgumentType.Path || this.Option is not null || this.Value is not { Data: PathExpression }) { return; }
			var newValue = await StorageHelper.PickDirectory(WindowHelper.GetForElement(this.View));
			if (newValue is not null) {
				this.Value.OfPath.Value = newValue;
				this.NotifyPropertyChanged(
					nameof(this.uPathValue_Text)
				);
			}
			return;
		}

		// ----------------

		public List<Tuple<Object, String>> uEnumerationValue_ItemsSource {
			get {
				if (this.Type is null || this.Option is null || this.Value is null) { return new List<Tuple<Object, String>>(); }
				return this.Option.Select((value) => (new Tuple<Object, String>(value, ConfigurationHelper.MakeArgumentValueString(value)))).ToList();
			}
		}

		public Object? uEnumerationValue_SelectedValue {
			get {
				if (this.Type is null || this.Option is null || this.Value is null) { return null; }
				return this.Value.Data;
			}
		}

		public async void uEnumerationValue_OnSelectionChanged (
			Object                    sender,
			SelectionChangedEventArgs args
		) {
			if (sender is not ComboBox senders) { return; }
			if (this.Type is null || this.Option is null || this.Value is null) { return; }
			this.Value.Data = senders.SelectedValue;
			this.NotifyPropertyChanged(
				nameof(this.uEnumerationValue_SelectedValue)
			);
			return;
		}

		#endregion

	}

}
