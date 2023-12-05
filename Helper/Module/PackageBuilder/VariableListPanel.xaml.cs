#pragma warning disable 0,
// ReSharper disable

using Helper;
using Helper.Utility;

namespace Helper.Module.PackageBuilder {

	public sealed partial class VariableListPanel : CustomControl {

		#region life

		public VariableListPanel (
		) {
			this.InitializeComponent();
			this.Controller = new VariableListPanelController() { View = this };
		}

		// ----------------

		private VariableListPanelController Controller { get; }

		// ----------------

		protected override void StampUpdate (
		) {
			this.Controller.Update();
			return;
		}

		#endregion

		#region property

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
			nameof(VariableListPanel.Value),
			typeof(List<Variable>),
			typeof(VariableListPanel),
			new PropertyMetadata(new List<Variable>())
		);

		public List<Variable> Value {
			get => this.GetValue(VariableListPanel.ValueProperty).AsClass<List<Variable>>();
			set => this.SetValue(VariableListPanel.ValueProperty, value);
		}

		#endregion

	}

	public class VariableListPanelController : CustomController {

		#region data

		public VariableListPanel View { get; init; } = default!;

		// ----------------

		public List<Variable> Value => this.View.Value;

		#endregion

		#region update

		public async void Update (
		) {
			this.uList_ItemsSource = new ObservableCollection<VariableListPanelItemController>(this.Value.Select((value) => (new VariableListPanelItemController() { Host = this, Item = value })));
			this.NotifyPropertyChanged(
				nameof(this.uList_ItemsSource)
			);
			return;
		}

		#endregion

		#region item

		public ObservableCollection<VariableListPanelItemController> uList_ItemsSource { get; set; } = new ObservableCollection<VariableListPanelItemController>();

		public async void uList_DragItemsCompleted (
			Object                      sender,
			DragItemsCompletedEventArgs args
		) {
			var senders = sender.AsClass<ListView>();
			this.Value.Clear();
			this.Value.AddRange(this.uList_ItemsSource.Select((value) => (value.Item)));
			return;
		}

		// ----------------

		public async void uAppend_Click (
			Object          sender,
			RoutedEventArgs args
		) {
			var senders = sender.AsClass<Button>();
			var newItem = new Variable() { Name = "", Value = "" };
			this.Value.Add(newItem);
			this.uList_ItemsSource.Add(new VariableListPanelItemController() { Host = this, Item = newItem });
			return;
		}

		#endregion

	}

	public class VariableListPanelItemController : CustomController {

		#region data

		public VariableListPanelController Host { get; init; } = default!;

		// ----------------

		public Variable Item { get; set; } = default!;

		#endregion

		#region view

		public String uName_Text {
			get {
				return this.Item.Name;
			}
		}

		public async void uName_TextChanged (
			Object               sender,
			TextChangedEventArgs args
		) {
			var senders = sender.AsClass<TextBox>();
			this.Item.Name = senders.Text;
			return;
		}

		// ----------------

		public String uValue_Text {
			get {
				return this.Item.Value;
			}
		}

		public async void uValue_TextChanged (
			Object               sender,
			TextChangedEventArgs args
		) {
			var senders = sender.AsClass<TextBox>();
			this.Item.Value = senders.Text;
			return;
		}

		// ----------------

		public async void uRemove_Click (
			Object          sender,
			RoutedEventArgs args
		) {
			var senders = sender.AsClass<Button>();
			this.Host.Value.Remove(this.Item);
			this.Host.uList_ItemsSource.Remove(this);
			return;
		}

		#endregion

	}

}
