using System;

namespace jrnl
{
	class MainClass
	{
		const string bardfile = "bard.bin";
		const string responsedir = "responses";
		const string journalfile = "E:\\Elite\\dev\\jrnl\\jrnl\\Journal.160922205901.01.log";
		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			Bard.Bard b = new Bard.Bard(); //Bard.Bard.Load(bardfile);
			foreach (string path in System.IO.Directory.GetFiles(responsedir))
			{
				string fn = System.IO.Path.GetFileNameWithoutExtension(path);
				using (System.IO.StreamReader responsereader = new System.IO.StreamReader(path))
				{
					string respline;
					while ((respline = responsereader.ReadLine()) != null)
					{
						b.ResponseManager.AddResponse(fn.ToLower(), respline);
					}
				}
			}

			var fs = new System.IO.FileStream(journalfile, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
			var sr = new System.IO.StreamReader(fs);
			string line;
			while ((line = sr.ReadLine()) != null)
			{
				Console.WriteLine(line);
				Bard.PlotEvent plotevent = Bard.PlotEvent.FromEliteLogData(line);
				Bard.NoteEvent(b, plotevent);
			}
			sr.Close();

			b.Tell("story.txt");
		}
	}
}
