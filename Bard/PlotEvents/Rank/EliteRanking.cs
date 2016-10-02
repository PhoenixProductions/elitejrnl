﻿using System;
namespace Bard
{
	/// <summary>
	/// Event for Reaching Elite Status
	/// </summary>
	[Serializable]
	public class EliteRanking : PlotEvent
	{
		
		public EliteRanking() : base()
		{
			base.Significance = 0.9;	// pretty significant
		}
	}
}
