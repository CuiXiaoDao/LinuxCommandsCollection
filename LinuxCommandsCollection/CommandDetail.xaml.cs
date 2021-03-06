﻿using LinuxCommandsCollection.Common;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace LinuxCommandsCollection
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class CommandDetail : Page
	{
		private NavigationHelper navigationHelper;
		private ObservableDictionary defaultViewModel = new ObservableDictionary();

		private string commandName;

		public CommandDetail()
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
		private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
		{
			// TODO: Create an appropriate data model for your problem domain to replace the sample data.
			var commandInformation = (string)e.NavigationParameter;
			commandName = commandInformation.Substring(4);
			string commandClass = commandInformation.Substring(0, 4);

			CommandName.Text = commandName;
			CommandContent.Navigate(new Uri("ms-appx-web:///Commands/" + commandClass + "/" + commandName + ".html"));

			checkIfFavorite();
		}

		private async void checkIfFavorite()
		{
			bool collected = await Collection.isCollectedAsync(commandName);
			if (collected)
			{
				Favorite.Icon = new SymbolIcon(Symbol.UnFavorite);
				Favorite.Label = "取消收藏";
			}
			else
			{
				Favorite.Icon = new SymbolIcon(Symbol.Favorite);
				Favorite.Label = "收藏";
			}
			//switch (collected)
			//{
			//	case true:
			//		Favorite.Icon = new SymbolIcon(Symbol.UnFavorite);
			//		Favorite.Label = "取消收藏";
			//		break;
			//	case false:
			//		Favorite.Icon = new SymbolIcon(Symbol.Favorite);
			//		Favorite.Label = "收藏";
			//		break;
			//	default:
			//		collected = App.favoriteCommand.Contains(commandName);
			//		break;
			//}
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

		private async void Favorite_Click(object sender, RoutedEventArgs e)
		{
			await Collection.changeFavoriteAsync(commandName);
			checkIfFavorite();
		}
	}
}