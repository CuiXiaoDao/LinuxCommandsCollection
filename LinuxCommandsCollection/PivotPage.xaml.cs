using LinuxCommandsCollection.Common;
using LinuxCommandsCollection.Data;
using System;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Store;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Pivot Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace LinuxCommandsCollection
{
	public sealed partial class PivotPage : Page
	{        
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
		private readonly NavigationHelper navigationHelper;
		private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

		public PivotPage()
		{
			this.InitializeComponent();

			this.NavigationCacheMode = NavigationCacheMode.Required;

			this.navigationHelper = new NavigationHelper(this);
			this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
			this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
		}

		/// <summary>
		/// Gets the view model for this <see cref="Page"/>.
		/// This can be changed to a strongly typed view model.
		/// </summary>
		public ObservableDictionary DefaultViewModel
		{
			get { return this.defaultViewModel; }
		}

		/// <summary>
		/// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
		/// </summary>
		public NavigationHelper NavigationHelper
		{
			get { return this.navigationHelper; }
		}
		private void AboutGrid_Tapped(object sender, TappedRoutedEventArgs e)
		{
			if (!Frame.Navigate(typeof(About)))
			{
				throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
			}
		}

		private void ClassGrid_Tapped(object sender, TappedRoutedEventArgs e)
		{
			string tag = ((Grid)sender).Tag.ToString();
			if (!Frame.Navigate(typeof(CommandsClass), tag))
			{
				throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
			}
		}

		private async void CollectionButton_Click(object sender, RoutedEventArgs e)
		{
			if (CollectionButton.Label == "选择")
			{
				CollectionButton.Label = "删除";
				CollectionButton.Icon = new SymbolIcon(Symbol.Delete);
				CollectionList.SelectionMode = ListViewSelectionMode.Multiple;
				CollectionList.IsItemClickEnabled = false;
			}
			else
			{
				var selectedItems = CollectionList.SelectedItems;
				foreach (Command command in selectedItems)
				{
					await Collection.changeFavoriteAsync(command.CommandName);
				}
				cancelSelectionMode();
				updateDefaultViewModel();
			}
		}

		private async void CommentGrid_Tapped(object sender, TappedRoutedEventArgs e)
		{
			await Windows.System.Launcher.LaunchUriAsync(new Uri(string.Format("ms-windows-store:reviewapp?appid={0}", CurrentApp.AppId)));
		}

		private void FavoriteItemClick(object sender, ItemClickEventArgs e)
		{
			var command = (Command)e.ClickedItem;
			if (!Frame.Navigate(typeof(CommandDetail), command.ClassName + command.CommandName))
			{
				throw new Exception();
			}
		}

		private void FeedBackGrid_Tapped(object sender, TappedRoutedEventArgs e)
		{
            FeedbackOverlay.Feedback();
		}

		/// <summary>
		/// Populates the page with content passed during navigation. Any saved state is also
		/// provided when recreating a page from a prior session.
		/// </summary>
		/// <param name="sender">
		/// The source of the event; typically <see cref="NavigationHelper"/>.
		/// </param>
		/// <param name="e">Event data that provides both the navigation parameter passed to
		/// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
		/// a dictionary of state preserved by this page during an earlier
		/// session. The state will be null the first time a page is visited.</param>
		private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
		{
			// TODO: Create an appropriate data model for your problem domain to replace the sample data
		}

		/// <summary>
		/// Preserves state associated with this page in case the application is suspended or the
		/// page is discarded from the navigation cache. Values must conform to the serialization
		/// requirements of <see cref="SuspensionManager.SessionState"/>.
		/// </summary>
		/// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/>.</param>
		/// <param name="e">Event data that provides an empty dictionary to be populated with
		/// serializable state.</param>
		private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
		{
			// TODO: Save the unique state of the page here.
		}

		/// <summary>
		/// Adds an item to the list when the app bar button is clicked.
		/// </summary>
		//private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
		//{
		//    string groupName = this.pivot.SelectedIndex == 0 ? FirstGroupName : SecondGroupName;
		//    var group = this.DefaultViewModel[groupName] as SampleDataGroup;
		//    var nextItemId = group.Items.Count + 1;
		//    var newItem = new SampleDataItem(
		//        string.Format(CultureInfo.InvariantCulture, "Group-{0}-Item-{1}", this.pivot.SelectedIndex + 1, nextItemId),
		//        string.Format(CultureInfo.CurrentCulture, this.resourceLoader.GetString("NewItemTitle"), nextItemId),
		//        string.Empty,
		//        string.Empty,
		//        this.resourceLoader.GetString("NewItemDescription"),
		//        string.Empty);

		//    group.Items.Add(newItem);

		//    // Scroll the new item into view.
		//    var container = this.pivot.ContainerFromIndex(this.pivot.SelectedIndex) as ContentControl;
		//    var listView = container.ContentTemplateRoot as ListView;
		//    listView.ScrollIntoView(newItem, ScrollIntoViewAlignment.Leading);
		//}

		private async void OtherAppGrid_Tapped(object sender, TappedRoutedEventArgs e)
		{
			await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:search?publisher=崔超"));
		}

		//private void setAppBarButtonIsEnable()
		//{
		//	Binding isEnabledBinding = new Binding();
		//	isEnabledBinding.Source = CollectionList.SelectedItems;
		//	isEnabledBinding.Path = new PropertyPath("Count")
		//	isEnabledBinding.Mode = BindingMode.TwoWay;
		//	isEnabledBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
		//	BindingOperations.SetBinding(txtText, TextBox.TextProperty, isEnabledBinding);
		//}
		private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            cancelSelectionMode();
            this.BottomAppBar.Visibility = pivot.SelectedIndex == 2?Visibility.Visible: Visibility.Collapsed;            
        }

		private async void SearchIcon_Tapped(object sender, TappedRoutedEventArgs e)
		{
			var commands = await Command.GetRelativeCommandsAsync(SearchTextBox.Text);
			this.DefaultViewModel["RelativeCommands"] = commands;
			if (commands.Count==0)
			{
				ResultsTextBlock.Visibility = Visibility.Visible;
			}
			else
			{
				ResultsTextBlock.Visibility = Visibility.Collapsed;
			}
		}

		private void SearchResultItemView_ItemClick(object sender, ItemClickEventArgs e)
		{
			var command = (Command)e.ClickedItem;
			if (!Frame.Navigate(typeof(CommandDetail), command.ClassName + command.CommandName))
			{
				throw new Exception();
			}
		}

		private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			SearchTextBox.SelectAll();
		}

		private void SearchTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Enter)
			{
				TappedRoutedEventArgs TappedArgs_e = new TappedRoutedEventArgs();
				TappedEventHandler handler = SearchIcon_Tapped;
				if (handler != null)
				{
					handler(this, TappedArgs_e);
				}
				//InputPane.GetForCurrentView().TryHide();
				//ResultsTextBlock.Focus(FocusState.Programmatic);
				//var items = await DataSource.GetRelativeItemsAsync(SearchTextBox.Text);
				//this.DefaultViewModel["RelativeItems"] = items;
				//if (items == null)
				//{
				//    ResultsTextBlock.Visibility = Visibility.Visible;
				//}
				//else
				//{
				//    ResultsTextBlock.Visibility = Visibility.Collapsed;
				//}
			}
		}

		//private async void SecondPivot_Loaded(object sender, RoutedEventArgs e)
		//{
		//	//var commands = await Command.GetCommandsAsync();
		//	//this.DefaultViewModel["RelativeCommands"] = commands;
		//}

		/// <summary>
		/// Loads the content for the second pivot item when it is scrolled into view.
		/// </summary>
		private void ThirdPivot_Loaded(object sender, RoutedEventArgs e)
		{
			updateDefaultViewModel();
		}

		private async void updateDefaultViewModel()
		{
			var collected = await Collection.getCollectionAsync();
			this.DefaultViewModel["Collected"] = await Command.GetCommandsAsync(collected);
            NoCollectionImage.Visibility = collected.Count == 0 ? Visibility.Visible:Visibility.Collapsed;
		}

		#region NavigationHelper registration

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			this.navigationHelper.OnNavigatedFrom(e);
			Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
		}

		/// <summary>
		/// The methods provided in this section are simply used to allow
		/// NavigationHelper to respond to the page's navigation methods.
		/// <para>
		/// Page specific logic should be placed in event handlers for the
		/// <see cref="NavigationHelper.LoadState"/>
		/// and <see cref="NavigationHelper.SaveState"/>.
		/// The navigation parameter is available in the LoadState method
		/// in addition to page state preserved during an earlier session.
		/// </para>
		/// </summary>
		/// <param name="e">Provides data for navigation methods and event
		/// handlers that cannot cancel the navigation request.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			this.navigationHelper.OnNavigatedTo(e);
			Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
		}
		private void cancelSelectionMode()
		{
			CollectionButton.Label = "选择";
			CollectionButton.Icon = new SymbolIcon(Symbol.List);
			CollectionList.SelectionMode = ListViewSelectionMode.None;
			CollectionList.IsItemClickEnabled = true;
		}

		private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
		{
			e.Handled = true;
			if (CollectionButton.Label == "删除")
			{
				cancelSelectionMode();
			}
			else
			{
				Application.Current.Exit();
			}
		}
		#endregion NavigationHelper registration
	}
}