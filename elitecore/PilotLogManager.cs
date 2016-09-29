using System;
using System.IO;

namespace elitecore
{
	/// <summary>
	/// Handles monitoring the Pilot Log and emitting events as necessary
	/// </summary>
	public class PilotLogManager
	{
		private string journalDirectory;
		public PilotLogManager(string JournalDirectory)
		{
			this.journalDirectory = JournalDirectory;
			System.Diagnostics.Debug.WriteLine("Watching {0}", this.journalDirectory);
			this.jdFileWatchers = new System.Collections.Hashtable();
		}

		private System.IO.FileSystemWatcher jdWatcher;
		private System.Collections.Hashtable jdFileWatchers; 
		public void startWatching()
		{
			System.Diagnostics.Debug.WriteLine("Starting Journal Watching: {0}", this.journalDirectory);
			if (this.jdWatcher == null)
			{
				System.Diagnostics.Debug.WriteLine("Initialising watching");
				this.jdWatcher = new System.IO.FileSystemWatcher(this.journalDirectory);
				this.jdWatcher.Changed += new FileSystemEventHandler(FileSystemEvent);
				this.jdWatcher.Created += new FileSystemEventHandler(FileSystemEvent);

			}
			this.jdWatcher.EnableRaisingEvents = true;
		}
		public void stopWatching()
		{
		}

		void FileSystemEvent(object sender, FileSystemEventArgs e)
		{
			if (e.ChangeType == WatcherChangeTypes.Created)
			{
				// Check if it's a journal log and start watching it

				string fn = System.IO.Path.GetFileNameWithoutExtension(e.FullPath);
				System.Diagnostics.Debug.WriteLine("{0} created", fn);
				openJournalStream(e.FullPath);
			}
			if (e.ChangeType == WatcherChangeTypes.Changed)
			{
				if (!this.jdFileWatchers.ContainsKey(e.FullPath.ToLower()))
				{
					openJournalStream(e.FullPath);
				}
				if (this.jdFileWatchers.ContainsKey(e.FullPath.ToLower()))
				{
					System.Diagnostics.Debug.WriteLine("{0} changed", System.IO.Path.GetFileNameWithoutExtension(e.FullPath));
					// if it doesn't exist in the fileWatchers keys we're not interested in it
					this.readJournalStream((System.IO.StreamReader)this.jdFileWatchers[e.FullPath.ToLower()]);
				}
			}

		}
		/// <summary>
		/// Attempts to open the journal stream and record that we're interested in it.
		/// </summary>
		/// <param name="filePath">File path.</param>
		void openJournalStream(string filePath)
		{
			string fn = System.IO.Path.GetFileName(filePath);
			if (fn.ToLower().StartsWith("journal", StringComparison.Ordinal))
			{
				System.Diagnostics.Debug.WriteLine("Now monitoring {0}", filePath);
				System.IO.FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				System.IO.StreamReader sr = new StreamReader(fs);
				this.jdFileWatchers.Add(filePath.ToLower(), sr);
			} 
		}

		/// <summary>
		/// Reads the journal stream.
		/// </summary>
		/// <param name="fs">Fs.</param>
		void readJournalStream(System.IO.StreamReader sr)
		{
			//while (sr.Peek() >= 0)
			{
				this.parseLine(sr.ReadLine());
			}
		}

		/// <summary>
		/// Parses the line.
		/// </summary>
		/// <param name="line">Line.</param>
		void parseLine(string line)
		{
			System.Diagnostics.Debug.WriteLine("Parsing: " + line);
			PilotEvent e = new PilotEvent(System.Web.Helpers.Json.Decode(line));

		}


	}
}
