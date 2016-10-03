using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace Bard
{
	[Serializable]
	public class Bard
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private EntityInfo cmdr = new EntityInfo("commander");

		private const string ExpectedTokens = "commanderName,shipName,gender";

		private StringDictionary tokens;

		private ArrayList plotEvents;

		private ResponseManager rm;

		private bool autogenerateresponsefile = false;
		public Bard()
		{
			log.Info("Initialising new Bard");
			this.tokens = new StringDictionary();
			this.plotEvents = new ArrayList();
			this.rm = new ResponseManager();
		}

		public Bard(StringDictionary tokens)
		{
			log.Info("Initialising new Bard with tokens");
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
					log.Warn(string.Format("Mandatory token {0} was not defined", s));
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
					cmdr.AddFact(e.Facts["commander"]);
					cmdr.AddFact(e.Facts["ship"]);
					cmdr.AddFact(e.Facts["credits"]);
					cmdr.AddFact(e.Facts["loan"]);
				}
				else
				{
					plotEvents.Add(e);
					if (!ResponseManager.Responses.ContainsKey(e.Event.ToLower())) {
						System.Collections.ArrayList facts = new ArrayList();
						foreach (var fact in e.Facts)
						{
							facts.Add(fact.Key);
						}
						log.Warn(String.Format("Unknown Event noted: {0}, Facts: {1}", e.Event, string.Join(",", facts.ToArray())));
					}
				}
			}
		}

		/// <summary>
		/// Based on what I know, tell the story
		/// </summary>
		public void Tell(string FilePath)
		{
			bool writeUnknownEvents = false;
			//if (!File.Exists(FilePath))
			{
				using (StreamWriter sr = new StreamWriter(FilePath, false))
				{
					foreach (PlotEvent e in plotEvents)
					{
						try
						{
							EventResponse r = ResponseManager.Respond(e.Event);
							log.Info(string.Format("Numer of possible responses: {0}", r.Responses.Count));
							string line = "";
							for (int attempts = 0; attempts < r.Responses.Count; attempts++)
							{
								EventResponseClause rc = r.Select();
								log.Info(rc.Text);
								line = rc.Text;
								log.Info(string.Format("Last Used :{0}", rc.LastUsed));
								rc.UsageCount++;
								log.Info(string.Format("Times Used :{0}", rc.UsageCount));
								rc.LastUsed = DateTime.Now;

								// TODO match {} tokens with facts from the event
								foreach (string key in cmdr.Facts.Keys)
								{
									string token = string.Format("{{{0}}}", key);
									line = line.Replace(token, cmdr.Facts[key].Value.ToString());
								}
								foreach (string key in e.Facts.Keys)
								{
									string token = string.Format("{{{0}}}", key);
									string replacement = e.Facts[key].Value.ToString();
									line = line.Replace(token, replacement);
								}
								//Otherwise we go round until we either hit the limit or we complete the text
								if (line.IndexOf("{{", StringComparison.CurrentCultureIgnoreCase) >= 0)
								{
									// we have an unknown fact
									log.Warn("Unknown fact in " + line);
									line = "";
								}
								else {
									break;	// all subs made so exit loop
								}
							}
							log.Info(line);
							if (line != "")
							{
								sr.WriteLine(line);
							}
						}
						catch (UnknownEventException exception)
						{
							log.Warn("Undefined Event: " + exception.EventID);
							if (writeUnknownEvents)
							{
								sr.WriteLine(exception.Message);
							}
						}
					}
				}
			}
		}
		public void ClauseFacts()
		{

		}
		public void dump()
		{
			Console.WriteLine("Tokens:");
			foreach (string s in tokens)
			{
				Console.WriteLine(" {0} - {1}", s, this.tokens[s]);
			}
			Console.WriteLine("");
			Console.WriteLine("Commander Facts:");
			foreach (Fact f in cmdr.Facts.Values)
			{
				Console.WriteLine(" {0} - {1}", f.Name, f.Value);
			}
			Console.WriteLine("");
			Console.WriteLine("PlotEvents:");
			foreach (PlotEvent e in plotEvents)
			{
				Console.WriteLine("{0} - {1}", e.Facts["timestamp"].Value, e.Facts["event"].Value);
				Console.WriteLine("Event Facts: ");
				foreach (Fact f2 in e.Facts.Values)
				{
					Console.WriteLine(" {0} - {1} - {2}", f2.id, f2.Name, f2.Value);
				}
			}
			Console.WriteLine("Responses:");

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
			log.Info("Loading from bardfile: "+ FilePath);
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
			log.Error("Could not find specified bardfile");
			throw new ArgumentException("File does not exist");
		}
		/// <summary>
		/// Gets the rm.
		/// </summary>
		/// <value>The rm.</value>
		public ResponseManager ResponseManager
		{
			get
			{
				return rm;
			}

		}
	}
}
