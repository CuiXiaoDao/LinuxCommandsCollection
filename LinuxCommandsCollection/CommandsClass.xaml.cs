using LinuxCommandsCollection.Common;
using LinuxCommandsCollection.Data;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace LinuxCommandsCollection
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class CommandsClass : Page
	{
		private NavigationHelper navigationHelper;
		private ObservableDictionary defaultViewModel = new ObservableDictionary();

		public CommandsClass()
		{
			this.InitializeComponent();

			this.navigationHelper = new NavigationHelper(this);
			this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
			this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
		}

		/// <summary>
		/// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
		/// </summary>
		public NavigationHelper NavigationHelper
		{
			get { return this.navigationHelper; }
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
		/// Populates the page with content passed during navigation.  Any saved state is also
		/// provided when recreating a page from a prior session.
		/// </summary>
		/// <param name="sender">
		/// The source of the event; typically <see cref="NavigationHelper"/>
		/// </param>
		/// <param name="e">Event data that provides both the navigation parameter passed to
		/// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
		/// a dictionary of state preserved by this page during an earlier
		/// session.  The state will be null the first time a page is visited.</param>
		private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
		{
			string className = e.NavigationParameter.ToString();
			ClassNameTextBlock.Text = getClassName(className);
			ClassIcon.Source = new BitmapImage(new Uri("ms-appx:///Assets/" + className + ".png"));

			List<Command> commands = await Command.GetCommandsAsync(className);
			this.DefaultViewModel["Commands"] = commands;
		}

		private string getClassName(string className)
		{
			string title;
			switch (className)
			{
				case "wjgl":
					title = "文件管理";
					break;

				case "cpgl":
					title = "磁盘管理";
					break;

				case "wdbj":
					title = "文档编辑";
					break;

				case "wjcs":
					title = "文件传输";
					break;

				case "cpwh":
					title = "磁盘维护";
					break;

				case "wltx":
					title = "网络通讯";
					break;

				case "xtgl":
					title = "系统管理";
					break;

				case "xtsz":
					title = "系统设置";
					break;

				case "bfys":
					title = "备份压缩";
					break;

				case "sbgl":
					title = "设备管理";
					break;

				default:
					title = "分类出错烦请反馈";
					break;
			}
			return title;
		}

		/// <summary>
		/// Preserves state associated with this page in case the application is suspended or the
		/// page is discarded from the navigation cache.  Values must conform to the serialization
		/// requirements of <see cref="SuspensionManager.SessionState"/>.
		/// </summary>
		/// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
		/// <param name="e">Event data that provides an empty dictionary to be populated with
		/// serializable state.</param>
		private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
		{
		}

		#region NavigationHelper registration

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
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			this.navigationHelper.OnNavigatedFrom(e);
		}

		#endregion NavigationHelper registration

		private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
		{
			var command = (Command)e.ClickedItem;
			if (!Frame.Navigate(typeof(CommandDetail), command.ClassName + command.CommandName))
			{
				throw new Exception();
			}
		}
	}
}