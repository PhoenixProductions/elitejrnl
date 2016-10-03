using System;
using ManyConsole;
using System.Collections.Generic;
namespace jrnl
{
	class MainClass
	{
		const string bardfile = "bard.bin";
		const string responsedir = "responses";
		const string journalfile = "E:\\Elite\\dev\\jrnl\\jrnl\\Journal.160922205901.01.log";
		public static int Main(string[] args)
		{
			
			var Commands = GetCommands ();
			return ConsoleCommandDispatcher.DispatchCommand (Commands, args, Console.Out);
		}

		public static IEnumerable<ConsoleCommand>GetCommands() {
			return ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof (MainClass));
		}
	}
}
