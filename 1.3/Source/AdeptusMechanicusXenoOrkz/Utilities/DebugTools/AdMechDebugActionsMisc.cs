using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse.AI.Group;
using Verse.Profile;
using Verse.Sound;

namespace Verse
{
    // Token: 0x0200053D RID: 1341
    public static class AdMechDebugActionsMisc
	{
		// Verse.DebugToolsSpawning
		[DebugAction("Adeptus", "Mature All plants of Def...", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MatureAllPlantsOf()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Thing t in from kd in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).Where(x=> x is Plant).ToList<Thing>()
								orderby kd.def.defName
								select kd)
			{
				list.Add(new DebugMenuOption(t.def.LabelCap, DebugMenuOptionMode.Action, delegate ()
				{
					foreach (Plant thing in Find.CurrentMap.listerThings.AllThings.Where(x=> x is Plant).ToList<Thing>())
					{
						if (thing.def == t.def)
						{
							thing.Growth = 1f;
						}
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}


	}
}
