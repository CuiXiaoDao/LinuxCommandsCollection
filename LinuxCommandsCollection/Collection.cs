using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace LinuxCommandsCollection
{
	internal class Collection
	{
		private static List<string> collection = new List<string>();

		public static async Task<List<string>> getCollectionAsync()
		{
			if (collection.Any())
			{
				return collection;
			}

			var roamingFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
			// Create the file in the local folder, or if it already exists, just open it
			Windows.Storage.StorageFile collectionFile =
						await roamingFolder.CreateFileAsync("Collected.txt", CreationCollisionOption.OpenIfExists);
			Stream collectionStream = await collectionFile.OpenStreamForReadAsync();
			using (StreamReader reader = new StreamReader(collectionStream))
			{
				string collectedCommand;
				while ((collectedCommand = reader.ReadLine()) != null)
				{
					collection.Add(collectedCommand);	// Add to list.
				}
			}
			return collection;
		}

		public static async Task<List<string>> saveCollectionAsync()
		{
			var roamingFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
			Windows.Storage.StorageFile collectionFile =
						await roamingFolder.CreateFileAsync("Collected.txt", CreationCollisionOption.ReplaceExisting);
			Stream collectionStream = await collectionFile.OpenStreamForWriteAsync();
			using (StreamWriter writer = new StreamWriter(collectionStream))
			{
				foreach (string collectedCommand in collection)
				{
					await writer.WriteAsync(collectedCommand + "\n");
				}
				//string collectedCommand;
				//while ((collectedCommand = reader.ReadLine()) != null)
				//{
				//	collection.Add(collectedCommand);	// Add to list.
				//}
			}
			return collection;
		}

		public static async Task<bool> isCollectedAsync(string commandName)
		{
			await getCollectionAsync();
			return collection.Contains(commandName);
		}

		public static async Task changeFavoriteAsync(string commandName)
		{
			await getCollectionAsync();
			if (collection.Contains(commandName))
			{
				collection.Remove(commandName);
			}
			else
			{
				collection.Add(commandName);
			}

			await saveCollectionAsync();
		}
	}
}