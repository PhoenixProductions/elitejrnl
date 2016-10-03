using System;
using System.Collections.Generic;
namespace Bard
{
	/// <summary>
	/// Manages the responses for events
	/// </summary>
	[Serializable]
	public class ResponseManager
	{
		private Dictionary<string, EventResponse> responses;
		public ResponseManager()
		{
			this.responses = new Dictionary<string, EventResponse>();
		}

		public Dictionary<string, EventResponse> Responses
		{
			get
			{
				return responses;
			}
		}
		/// <summary>
		/// Adds the response.
		/// </summary>
		/// <returns>The response.</returns>
		/// <param name="eventId">Event identifier.</param>
		/// <param name="ResponseText">Response text.</param>
		public EventResponse AddResponse(string eventId, string ResponseText)
		{
			EventResponse r;
			if (this.responses.ContainsKey(eventId.ToLower()))
			{
				System.Diagnostics.Debug.WriteLine("Response already known for thisevent");
				r = this.responses[eventId.ToLower()];
			}
			else {
				System.Diagnostics.Debug.WriteLine("Adding new response for this event");
				r = new EventResponse(eventId.ToLower());
				this.responses.Add(eventId.ToLower(), r);
			}
			try
			{
				r.AddResponseText(ResponseText);
			}
			catch (ResponseKnownException)
			{
			}

			return r;
		}

		/// <summary>
		/// Select a response for the event type
		/// </summary>
		/// <param name="eventId">Event identifier.</param>
		public EventResponse Respond(string eventId)
		{
			if (this.responses.ContainsKey(eventId.ToLower()))
			{
				EventResponse r;
				r = this.responses[eventId.ToLower()];
				return r;
			}
			throw new UnknownEventException(String.Format("{0} has no responses", eventId), eventId);
		}
	}
}
