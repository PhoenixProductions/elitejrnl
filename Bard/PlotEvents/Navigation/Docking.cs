using System;
namespace Bard
{
	[Serializable]
	public class Docking : PlotEvent
	{
		public Docking()
		{
		}

		public bool CanHandle(string eventId)
		{
			switch(eventId.ToLower()) {
				case "docked":
				case "undocked":
					return true;
				default:
					return false;
		}
	}
}
