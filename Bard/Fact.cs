using System;
namespace Bard
{
	/// <summary>
	/// Represents a single fact about something.
	/// </summary>
	[Serializable]
	public class Fact
	{
		DateTime _created;
		string _name;
		object _fact;

		public string id
		{
			get
			{
				return _name.ToLower();
			}
		}
		public string Name
		{
			get
			{
				return _name;
			}

			set
			{
				_name = value;
			}
		}

		public object Value
		{
			get
			{
				return _fact;
			}

			set
			{
				_fact = value;
			}
		}

		public DateTime Created
		{
			get
			{
				return _created;
			}

			set
			{
				_created = value;
			}
		}

		public Fact(string Name, object Fact)
		{
			this._name= Name;
			this._fact = Fact;
			this._created = DateTime.Now;
		}
	}
}
