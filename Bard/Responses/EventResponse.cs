using System;
using System.Security.Cryptography;
using System.Collections;
namespace Bard
{
	/// <summary>
	/// Represents ways to response to an event
	/// </summary>
	public class EventResponse
	{
		private string eventId;

		private double signficance;

		private System.Collections.Hashtable responses;
		public EventResponse(string eventId) : this(eventId, 0.0)
		{
			this.eventId = eventId.ToLower();

			this.responses = new System.Collections.Hashtable();
		}
		public EventResponse(string eventId, double signficance)
		{
			this.signficance = signficance;
		}

		public void AddResponse(string responseText)
		{
			string hash = EventResponse.GetMd5Hash(responseText);
			if (!responses.ContainsKey(hash))
			{
				EventResponseClause rc = new EventResponseClause(responseText);
				this.responses.Add(hash, rc);
			}
		}
		/// <summary>
		/// Select a random response clause
		/// </summary>
		public EventResponseClause Select()
		{
			int limit = this.responses.Count;
			ArrayList keys = new System.Collections.ArrayList(responses.Keys);
			Random r = new Random();
			int ri = r.Next();
			string k = (string)keys[ri];

			return (EventResponseClause)this.responses[k];

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
