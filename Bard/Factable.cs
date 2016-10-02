using System;
using System.Collections.Generic;

namespace Bard
{
	[Serializable]
	public abstract class Factable
	{
		/// <summary>
		/// Simple string facts
		/// </summary>
		private Dictionary<string, Fact> facts;
		public Factable()
		{
			this.facts = new Dictionary<string, Fact>();
		}

		public Dictionary<string, Fact> Facts
		{
			get
			{
				return facts;
			}

		}

		public void AddFact(Fact f)
		{
			if (this.Facts.ContainsKey(f.Name.ToLower()))
			{
				// check if this fact is newer than what we already know.
				Fact existingFact = this.Facts[f.id];
				if (f.Created > existingFact.Created)
				{
					this.Facts[f.id] = f;   // Update what we know
														// TODO We should at some point persist what we *knew*
				}
			}
			else {
				this.Facts.Add(f.id, f);
			}
		}
	}
}
