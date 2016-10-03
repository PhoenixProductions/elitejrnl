using System;
using ManyConsole;
using System.IO;

namespace jrnl
{
	public class GenerateEventsCommand : JrnlCommand
	{
		protected string eventId;
		protected string responsedir = "./responses";
		protected string dir = "./";
		public GenerateEventsCommand()
		{
			IsCommand("generateresponses", "Generate response stubs based on parsing a journal");
			HasOption("d|dir=", "The full path containing .log files", v => dir = v);
			HasOption("r|responses=", "The full path containing response files", v => responsedir = v);
		}
		public override int Run(string[] remainingArguments)
		{
			Console.WriteLine(responsedir);
			System.Collections.ArrayList logs = this.ReadDirectoryFiles(dir, ".log");
			foreach (string journalfile in logs)
			{
				var fn = Path.GetFileName(journalfile);
				Console.Out.WriteLine(String.Format("Reading Journal \"{0}\"", fn));
				var fs = new System.IO.FileStream(journalfile, FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
				var sr = new System.IO.StreamReader(fs);
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					Bard.PlotEvent plotevent = Bard.PlotEvent.FromEliteLogData(line);
					string eventresponsepath = Path.Combine(responsedir, plotevent.Event.ToLower() + ".txt");

					if (!File.Exists(eventresponsepath))
					{
						Console.WriteLine(plotevent.Event + " already exists");
					}else 
					{
						Console.WriteLine("Generating " + eventresponsepath);
						File.CreateText(eventresponsepath).Close();
					}
				}
				sr.Close();
			}
			return 0;
		}

	}
}
