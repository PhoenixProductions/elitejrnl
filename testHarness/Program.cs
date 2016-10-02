using System;

namespace testHarness
{
	class MainClass
	{
		static string lineformat = "{{\"timestamp\":\"{0}\", \"event\":\"{1}\",{2}}}";
		static elitecore.PilotLogManager p;
		static Bard.Bard b;
		static string demoFilePath = "E:\\Elite\\dev\\jrnl\\jrnl\\Journal.160922205901.01.log";
		public static void Main(string[] args)
		{
			readExisting(demoFilePath);
		}
		protected static void readExisting(string filename)
		{
			//var b = new Bard.Bard();
			if (System.IO.File.Exists("bard.xml"))
			{
				System.Diagnostics.Debug.WriteLine("Loading existing bard");
				b = Bard.Bard.Load("bard.xml");
			}
			else {
				System.Diagnostics.Debug.WriteLine("Creating New bard");
				b = new Bard.Bard();
			}
			var fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
			var sr = new System.IO.StreamReader(fs);
			string line;
			while((line = sr.ReadLine()) != null) {
				Console.WriteLine(line);
				Bard.PlotEvent plotevent = Bard.PlotEvent.FromEliteLogData(line);
				b.NoteEvent(plotevent);
			}
			sr.Close();
			b.dump();
			while (Console.ReadKey().Key != ConsoleKey.Escape)
			{
			}
			Bard.Bard.Save(b, "bard.xml");
		}
		protected static void Synthesis() {
			b = new Bard.Bard();
			p = new elitecore.PilotLogManager("./");
			p.PilotEvent += P_PilotEvent;
			p.startWatching();
			while (Console.ReadKey().Key != ConsoleKey.Escape)
			{

				string line = string.Format(lineformat, DateTime.Now.ToUniversalTime(), "Docked", "\"StationName\":\"SomeStation\"");

				Console.WriteLine(line);
				var sw = new System.IO.StreamWriter("./Journal.log", true);
				sw.WriteLine(line);
				sw.Close();
			}
			p.stopWatching();

			Console.WriteLine("This is what I know:");
			b.dump();
			while (Console.ReadKey().Key != ConsoleKey.Escape)
			{
			}

		}

		static void P_PilotEvent(object sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("Pilot Event captured");
			var pe = (elitecore.PilotEventArgs)e;
			Bard.PlotEvent plotevent = Bard.PlotEvent.FromEliteLogData(pe.rawEventLog);
				/*new Bard.PlotEvent(pe.Timestamp);
			foreach (string prop in pe.data.Keys)
			{
				Bard.Fact f = new Bard.Fact(prop, pe.data[prop]);
				plotevent.AddFact(f);
			}
			System.Diagnostics.Debug.WriteLine(plotevent);
			*/
			b.NoteEvent(plotevent);
		}
	}
}
