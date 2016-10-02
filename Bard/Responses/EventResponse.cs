using System;
using System.Security.Cryptography;
using System.Collections;
namespace Bard
{
	/// <summary>
	/// Represents ways to response to an event
	/// </summary>
	[Serializable]
	public class EventResponse
	{
		private string eventId;

		private double signficance;

		private System.Collections.Hashtable responses;

		public Hashtable Responses
		{
			get
			{
				return responses;
			}

		}

		public EventResponse(string eventId) : this(eventId, 0.0)
		{
			this.eventId = eventId.ToLower();

			this.responses = new System.Collections.Hashtable();
		}
		public EventResponse(string eventId, double signficance)
		{
			this.signficance = signficance;
		}

		public void AddResponseText(string responseText)
		{
			string hash = EventResponse.GetMd5Hash(responseText);
			if (!Responses.ContainsKey(hash))
			{
				System.Diagnostics.Debug.WriteLine("Adding new response text");
				EventResponseClause rc = new EventResponseClause(responseText);
				this.Responses.Add(hash, rc);
			}
			else {
				System.Diagnostics.Debug.WriteLine("Duplicate response text "+ responseText + " for event " + this.eventId);
				throw new ResponseKnownException();
			}
		}
		/// <summary>
		/// Select a random response clause
		/// </summary>
		public EventResponseClause Select()
		{
			int limit = this.Responses.Count;
			var keys = new System.Collections.ArrayList(Responses.Keys);
			var r = new Random();
			System.Diagnostics.Debug.WriteLine("Limit " + (keys.Count));
			int ri = r.Next(keys.Count);
			var k = (string)keys[ri];

			return (EventResponseClause)this.Responses[k];
		}
		protected static string GetMd5Hash(string input)
		{
			using (MD5 hash = MD5.Create())
			{
				byte[] data = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				for (int i = 0; i < data.Length; i++)
				{
					sb.Append(data[i].ToString("x2"));
				}
				return sb.ToString();
			}
		}
	}
}
