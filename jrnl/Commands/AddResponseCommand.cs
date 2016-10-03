using System;
using ManyConsole;
using System.IO;

namespace jrnl
{
	public class AddResponseCommand : ManyConsole.ConsoleCommand
	{
		protected string eventId;
		protected string responsedir = "./responses";
		public AddResponseCommand()
		{
			IsCommand("addresponse", "A response to an event");
			HasOption("r|responses=", "The full path containing response files", v => responsedir = v);
			this.HasRequiredOption("e|event=", "Journal event", (string obj) => eventId = obj.ToLower());
			this.AllowsAnyAdditionalArguments("Text to be used as the response");
		}
		public override int Run(string[] remainingArguments)
		{
			string eventfilename = Path.Combine(responsedir, eventId + ".txt");
			using (StreamWriter sw = new StreamWriter(Path.GetFullPath(eventfilename), true))
			{
				string responseText = String.Join(" ", remainingArguments);
				if (responseText != "")
				{
					sw.WriteLine(responseText);
				}
			}

			return 0;
		}
	
	}
}
