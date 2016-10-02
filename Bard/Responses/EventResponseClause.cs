using System;
namespace Bard
{
	/// <summary>
	/// A single response phrase that can be made for a given event
	/// </summary>
	[Serializable]
	public class EventResponseClause
	{
		/// <summary>
		/// Number of times this clause has been used
		/// </summary>
		private int usageCount = 0;

		/// <summary>
		/// When the clause was last used.
		/// </summary>
		private DateTime lastUsed;

		private string text;

		public DateTime LastUsed
		{
			get
			{
				return lastUsed;
			}

			set
			{
				lastUsed = value;
			}
		}

		public string Text
		{
			get
			{
				return text;
			}

			set
			{
				text = value;
			}
		}

		public int UsageCount
		{
			get
			{
				return usageCount;
			}

			set
			{
				usageCount = value;
			}
		}

		public EventResponseClause(string Text)
		{
			this.Text = Text;
			this.UsageCount = 0;
			this.LastUsed = DateTime.MinValue;
		}
	}
}
