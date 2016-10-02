using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;
namespace Bard
{
	[Serializable]
	public class Bard
	{
		private EntityInfo cmdr = new EntityInfo("commander");

		private const string ExpectedTokens = "commanderName,shipName,gender";

		private StringDictionary tokens;

		private ArrayList plotEvents;

		public Bard()
		{
			this.tokens = new StringDictionary();
			this.plotEvents = new ArrayList();
		}

		public Bard(StringDictionary tokens)
		{
			this.tokens = tokens;
			string[] mandatoryTokens = "commanderName,shipName,gender".Split(new char[]
			{
				','
			});
			ArrayList missing = new ArrayList();
			for (int i = 0; i < mandatoryTokens.Length; i++)
			{
				string s = mandatoryTokens[i];
				bool flag = !this.tokens.ContainsKey(s);
				if (flag)
				{
					missing.Add(string.Format("Mandatory token {0} was not defined", s));
				}
			}
			bool flag2 = missing.Count > 0;
			if (flag2)
			{
				throw new ArgumentException(string.Concat(missing.ToArray()));
			}
		}

		public void NoteEvent(PlotEvent e)
		{
			Debug.WriteLine(e.Event);
			bool flag = e.Facts["event"].Value.ToString().ToLower() == "fileheader";
			if (!flag)
			{
				bool flag2 = e.Facts["event"].Value.ToString().ToLower() == "loadgame";
				if (flag2)
				{
					this.cmdr.AddFact(e.Facts["commander"]);
					this.cmdr.AddFact(e.Facts["ship"]);
					this.cmdr.AddFact(e.Facts["credits"]);
					this.cmdr.AddFact(e.Facts["loan"]);
				}
				else
				{
					this.plotEvents.Add(e);
				}
			}
		}

		public void dump()
		{
			Console.WriteLine("Tokens:");
			foreach (string s in this.tokens)
			{
				Console.WriteLine(" {0} - {1}", s, this.tokens[s]);
			}
			Console.WriteLine("");
			Console.WriteLine("Commander Facts:");
			foreach (Fact f in this.cmdr.Facts.Values)
			{
				Console.WriteLine(" {0} - {1}", f.Name, f.Value);
			}
			Console.WriteLine("");
			Console.WriteLine("PlotEvents:");
			foreach (PlotEvent e in this.plotEvents)
			{
				Console.WriteLine("{0} - {1}", e.Facts["timestamp"].Value, e.Facts["event"].Value);
				Console.WriteLine("Event Facts: ");
				foreach (Fact f2 in e.Facts.Values)
				{
					Console.WriteLine(" {0} - {1} - {2}", f2.id, f2.Name, f2.Value);
				}
			}
			Console.WriteLine("*****");
			Console.WriteLine("");
		}

		public static void Save(Bard bard, string FilePath)
		{
			FileStream outfile = File.Create(FilePath);
			//System.Runtime.Serialization.Formatters.Soap.SoapFormatter sf = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(outfile, bard);
			//sf.Serialize(outfile, bard);
			outfile.Close();
		}

		public static Bard Load(string FilePath)
		{
			bool flag = File.Exists(FilePath);
			if (flag)
			{
				FileStream fs = File.OpenRead(FilePath);
				//System.Runtime.Serialization.Formatters.Soap.SoapFormatter sf = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
				BinaryFormatter bf = new BinaryFormatter();
				Bard bard = (Bard)bf.Deserialize(fs);
				//Bard bard = (Bard)sf.Deserialize(fs);
				fs.Close();
				return bard;
			}
			throw new ArgumentException("File does not exist");
		}
	}
}
