using System;
using ManyConsole;
using System.IO;

namespace jrnl
{
	public class TellCommand : ManyConsole.ConsoleCommand
	{
		string dir;
		string responsedir;
		string bardfile = "";
		Bard.Bard b; 
		bool saveToBardfile = false;
		public TellCommand () 
		{
			IsCommand ("tell", "Tell story");

			HasRequiredOption ("d|dir=", "The full path containing .log files", v => dir = v);

			HasRequiredOption ("r|responses=", "The full path containing response files", v => responsedir = v );

			HasOption ("b|bardfile=", "An existing bardfile", v => bardfile = v);

			HasOption ("s|save", "Save", v => saveToBardfile = v != null);
		}

		public override int Run(string[] remainingArguments) {
			if (bardfile == "") {
				b = new Bard.Bard ();
			} else {
				if (File.Exists (bardfile)) {
					b = Bard.Bard.Load (bardfile);
				} else {
					if (!saveToBardfile) {
						throw new Exception ("Specified bardfile does not exist");
					}
					b = new Bard.Bard ();	// Initialise a new blank one but we'll save to it later
				}
			}
			System.Collections.ArrayList logs = ReadDirectoryFiles (dir, ".log");
			System.Collections.ArrayList responses = ReadDirectoryFiles (responsedir, ".txt");
			int countBeforeLoad = b.ResponseManager.Responses.Keys.Count;
			foreach (string path in responses) {
				string fn = System.IO.Path.GetFileNameWithoutExtension (path);
				Console.Out.WriteLine(String.Format("Loading response \"{0}\"", fn));
				using (System.IO.StreamReader responsereader = new System.IO.StreamReader (path)) {
					string respline;
					while ((respline = responsereader.ReadLine ()) != null) {
						b.ResponseManager.AddResponse (fn.ToLower (), respline);
					}
				}
			}
			int countAfterLoad = b.ResponseManager.Responses.Keys.Count;
			Console.WriteLine ((countAfterLoad - countBeforeLoad )+ " responses loaded");
			foreach (string journalfile in logs) {
				var fn = Path.GetFileName (journalfile);
				Console.Out.WriteLine (String.Format ("Reading Journal \"{0}\"", fn));
				var fs = new System.IO.FileStream (journalfile, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
				var sr = new System.IO.StreamReader (fs);
				string line;
				while ((line = sr.ReadLine ()) != null) {
					Bard.PlotEvent plotevent = Bard.PlotEvent.FromEliteLogData (line);
					b.NoteEvent (plotevent);
				}
				sr.Close ();
			}
			Console.Out.WriteLine ("Telling Story");
			b.Tell("story.txt");
			if (saveToBardfile && bardfile != "") {
				Bard.Bard.Save (b, bardfile);
			}
			return 0;
		}
		public System.Collections.ArrayList ReadDirectoryFiles(string dir, string extension) {
			dir = Path.GetFullPath (dir);
			Console.WriteLine (dir);
			System.Collections.ArrayList files = new System.Collections.ArrayList ();
			if (Directory.Exists (dir)) {
				var dirinfo = new DirectoryInfo (dir);
				foreach (var e in dirinfo.GetFileSystemInfos()) {
					if (e is FileInfo) {
						var ext = Path.GetExtension(((FileInfo)e).FullName);
						Console.Error.WriteLine (ext);
						if (ext.ToLower () == extension.ToLower()) {
							Console.Error.WriteLine (string.Format ("Adding {0} to list", ((FileInfo)e).FullName));
							files.Add (((FileInfo)e).FullName);
						}
					}
				}
			}
			return files;
		}
	}
}

