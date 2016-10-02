using System;
using System.Collections.Generic;
namespace Bard
{
	[Serializable]
	public class EntityInfo : Factable
	{
		public const string COMMANDER = "commander";

		private string id;

		public EntityInfo(string id)
		{
			this.id = id;

		}
	}
}
