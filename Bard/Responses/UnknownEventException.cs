using System;
using System.Runtime.Serialization;

namespace Bard
{
	[Serializable]
	class UnknownEventException : Exception
	{
		string eventId;
		public UnknownEventException()
		{
		}

		public UnknownEventException(string message, string eventId) : base(message)
		{
			this.eventId = eventId;
		}

		public UnknownEventException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected UnknownEventException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public string EventID
		{
			get
			{
				return eventId;
			}

			set
			{
				eventId = value;
			}
		}
	}
}