using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinuxCommandsCollection.Data
{
	[Table("LinuxCommands")]
	internal class CommandData
	{
		public string CommandName { get; set; }

		public string ClassName { get; set; }
	}

	internal class Command
	{
		private static Dictionary<string, List<Command>> commands = new Dictionary<string, List<Command>>();

		public string CommandName { get; set; }

		public string ClassName { get; set; }

		public Command(string commandName, string className)
		{
			this.CommandName = commandName;
			this.ClassName = className;
		}

		//public static async Task<List<Command>> GetCommandsAsync()
		//{
		//	List<Command> allCommands = new List<Command>();
		//          string[] allClassName = { "wjgl", "cpgl", "wdbj", "wjcs", "cpwh", "wltx", "xtgl", "xtsz", "bfys", "sbgl" };
		//	SQLiteAsyncConnection conn = new SQLiteAsyncConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\LinuxCommands.db");

		//	foreach (string className in allClassName)
		//	{
		//		if (!commands.ContainsKey(className))
		//		{
		//			commands[className] = new List<Command>();
		//			var querry = await (conn.Table<CommandData>().Where(command => command.ClassName == className)).ToListAsync();
		//			foreach (var command in querry)
		//			{
		//				commands[className].Add(new Command(command.CommandName, command.ClassName));
		//			}
		//		}

		//		allCommands.AddRange(commands[className]);
		//	}
		//	return allCommands;
		//}

		public static async Task<List<Command>> GetCommandsAsync(string className)
		{
			if (commands.ContainsKey(className))
				return commands[className];

			commands[className] = new List<Command>();
			SQLiteAsyncConnection conn = new SQLiteAsyncConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\LinuxCommands.db");

			var querry = await (conn.Table<CommandData>().Where(command => command.ClassName == className)).ToListAsync();
			foreach (var command in querry)
			{
				commands[className].Add(new Command(command.CommandName, command.ClassName));
			}

			return commands[className];
		}

		public static async Task<List<Command>> GetCommandsAsync(List<string> commandNames)
		{
			List<Command> commandsLists = new List<Command>();
			SQLiteAsyncConnection conn = new SQLiteAsyncConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\LinuxCommands.db");

			var querry = await (conn.Table<CommandData>().Where(command => commandNames.Contains(command.CommandName))).ToListAsync();
			foreach (var command in querry)
			{
				commandsLists.Add(new Command(command.CommandName, command.ClassName));
			}

			return commandsLists;
		}

		public static async Task<List<Command>> GetRelativeCommandsAsync(string keyWord)
		{
			var commands = new List<Command>();
			SQLiteAsyncConnection conn = new SQLiteAsyncConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\LinuxCommands.db");

			var querry = await (conn.Table<CommandData>().Where(command => command.CommandName.Contains(keyWord))).ToListAsync();
			foreach (var command in querry)
			{
				commands.Add(new Command(command.CommandName, command.ClassName));
			}

			return commands;
		}
	}
}