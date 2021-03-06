﻿using LinuxCommandsCollection.Common;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Pivot Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace LinuxCommandsCollection
{
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	public sealed partial class App : Application
	{
		private TransitionCollection transitions;

		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			this.InitializeComponent();
			this.Suspending += this.OnSuspending;
            this.RequestedTheme = ApplicationTheme.Dark;
            copyDatabase();
			//loadFavorite();
		}

		//private void loadFavorite()
		//{
		//	Windows.Storage.ApplicationDataContainer localsettings = Windows.Storage.ApplicationData.Current.LocalSettings;
		//	if (localsettings.Values.ContainsKey("Favorite"))
		//	{
		//		favoriteCommand = (List<string>)localsettings.Values["Favorite"];
		//	}
		//	else
		//	{
		//		localsettings.Values["Favorite"] = favoriteCommand;
		//          }
		//}

		/// <summary>
		/// Invoked when the application is launched normally by the end user.  Other entry points
		/// will be used when the application is launched to open a specific file, to display
		/// search results, and so forth.
		/// </summary>
		/// <param name="e">Details about the launch request and process.</param>
		protected override async void OnLaunched(LaunchActivatedEventArgs e)
		{
#if DEBUG
			if (System.Diagnostics.Debugger.IsAttached)
			{
				this.DebugSettings.EnableFrameRateCounter = true;
			}
#endif

			Frame rootFrame = Window.Current.Content as Frame;

			// Do not repeat app initialization when the Window already has content,
			// just ensure that the window is active.
			if (rootFrame == null)
			{
				// Create a Frame to act as the navigation context and navigate to the first page.
				rootFrame = new Frame();

				// Associate the frame with a SuspensionManager key.
				SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

				// TODO: Change this value to a cache size that is appropriate for your application.
				rootFrame.CacheSize = 1;

				// Set the default language
				rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{
					// Restore the saved session state only when appropriate.
					try
					{
						await SuspensionManager.RestoreAsync();
					}
					catch (SuspensionManagerException)
					{
						// Something went wrong restoring state.
						// Assume there is no state and continue.
					}
				}

				// Place the frame in the current Window.
				Window.Current.Content = rootFrame;
			}

			if (rootFrame.Content == null)
			{
				// Removes the turnstile navigation for startup.
				if (rootFrame.ContentTransitions != null)
				{
					this.transitions = new TransitionCollection();
					foreach (var c in rootFrame.ContentTransitions)
					{
						this.transitions.Add(c);
					}
				}

				rootFrame.ContentTransitions = null;
				rootFrame.Navigated += this.RootFrame_FirstNavigated;

				// When the navigation stack isn't restored navigate to the first page,
				// configuring the new page by passing required information as a navigation
				// parameter.
				if (!rootFrame.Navigate(typeof(PivotPage), e.Arguments))
				{
					throw new Exception("Failed to create initial page");
				}
			}

			// Ensure the current window is active.
			Window.Current.Activate();
		}

		/// <summary>
		/// Restores the content transitions after the app has launched.
		/// </summary>
		private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
		{
			var rootFrame = sender as Frame;
			rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
			rootFrame.Navigated -= this.RootFrame_FirstNavigated;
		}

		/// <summary>
		/// Invoked when application execution is being suspended.  Application state is saved
		/// without knowing whether the application will be terminated or resumed with the contents
		/// of memory still intact.
		/// </summary>
		/// <param name="sender">The source of the suspend request.</param>
		/// <param name="e">Details about the suspend request.</param>
		private async void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();
			await SuspensionManager.SaveAsync();
			deferral.Complete();
		}

		private async void copyDatabase()
		{
			Windows.Storage.ApplicationDataContainer localsettings = Windows.Storage.ApplicationData.Current.LocalSettings;
			if (!(localsettings.Values.ContainsKey("dbVersion") && (int)localsettings.Values["dbVersion"] == 2))
			{
				//var installationfolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
				var dataModelfolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("DataModel");
				var dataFile = await dataModelfolder.GetFileAsync("LinuxCommands.db");
				await dataFile.CopyAsync(Windows.Storage.ApplicationData.Current.LocalFolder, "LinuxCommands.db", NameCollisionOption.ReplaceExisting);
				localsettings.Values["dbVersion"] = 2;
			}
		}
	}
}