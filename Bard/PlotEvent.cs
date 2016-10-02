using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bard
{
	[Serializable]
	public class PlotEvent : Factable	
	{
		public DateTime TimeStamp;
		public string Event;
		/// <summary>
		/// How importance is this plot event
		/// </summary>
		public double Significance = 0.0;  // Middle to -1>0>1

		public PlotEvent()
		{
			TimeStamp = DateTime.Now;
		}
		public PlotEvent(DateTime timestamp) : this()
		{
			this.TimeStamp = timestamp;
		}

		/// <summary>
		/// Construct a Plot Event based on Elite Journal Format
		/// </summary>
		/// <returns>The log data.</returns>
		/// <param name="LogData">Log data.</param>
		public static PlotEvent FromEliteLogData(string LogData)
		{

			JsonTextReader tr = new JsonTextReader(new System.IO.StringReader(LogData));
			string lastPropName = "";
			System.Collections.ArrayList tmpFacts = new System.Collections.ArrayList();
			string EventName ="";
			DateTime LogTimeStamp = DateTime.MinValue;
			while (tr.Read())
			{
				if ((
					tr.TokenType == JsonToken.String
					||
					tr.TokenType == JsonToken.Integer
					||
					tr.TokenType == JsonToken.Boolean
					||
					tr.TokenType == JsonToken.Float
				    )
				    && lastPropName != "")
				{
					if (lastPropName == "event")
					{
						EventName = tr.Value.ToString();
					}
					//System.Diagnostics.Debug.WriteLine("Got string value and propname");
					Fact f = new Fact(lastPropName, tr.Value);
					tmpFacts.Add(f);
					lastPropName = "";
				}
				else if (tr.TokenType == JsonToken.Date)
				{
					if (lastPropName == "timestamp")
					{
						if (DateTime.TryParse(tr.Value.ToString(), out LogTimeStamp))
						{
							Fact f = new Fact(lastPropName, tr.Value.ToString());
							tmpFacts.Add(f);
						}
						lastPropName = "";
					}
				}
				else if (tr.TokenType == JsonToken.PropertyName)
				{
					//System.Diagnostics.Debug.WriteLine("Setting propname");
					lastPropName = tr.Value.ToString();
				}
			}

			PlotEvent pe = new PlotEvent(LogTimeStamp);
			pe.Event = EventName;
			pe.TimeStamp = LogTimeStamp;
			foreach (Fact f in tmpFacts)
			{
				f.Created = LogTimeStamp;
				pe.AddFact(f);
			}

			return pe;
		}
	}
}
