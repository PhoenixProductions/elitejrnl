using System;
namespace Bard
{
	/// <summary>
	/// A single response phrase that can be made for a given event
	/// </summary>
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
		public EventResponseClause(string Text)
		{
			this.text = Text;
			this.usageCount = 0;
			this.lastUsed = DateTime.MinValue;
		}
	}
}
