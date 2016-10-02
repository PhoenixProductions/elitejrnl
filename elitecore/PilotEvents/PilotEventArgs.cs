using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace elitecore
{
	public class PilotEventArgs : EventArgs
	{
		public string rawEventLog;

		public DateTime Timestamp;

		public string Event;

		public Dictionary<string, string> data;

		public PilotEventArgs(string LogData)
		{
			this.data = new Dictionary<string, string>();
			this.rawEventLog = LogData;
			JsonTextReader tr = new JsonTextReader(new StringReader(LogData));
			string lastPropName = "";
			while (tr.Read())
			{
				bool flag = tr.TokenType == JsonToken.String && lastPropName != "";
				if (flag)
				{
					Debug.WriteLine("Got string value and propname");
					this.data.Add(lastPropName, tr.Value.ToString());
					lastPropName = "";
				}
				else
				{
					bool flag2 = tr.TokenType == JsonToken.PropertyName;
					if (flag2)
					{
						Debug.WriteLine("Setting propname");
						lastPropName = tr.Value.ToString();
					}
				}
			}
		}
	}
}