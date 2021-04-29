using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using HarmonyLib;
using Verse.Sound;
using AdeptusMechanicus;
using AdeptusMechanicus.ExtensionMethods;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(Pawn_AgeTracker), "AgeTick")]
    public static class Pawn_AgeTracker_AgeTick_Orkoid_Patch
	{
		private static bool Prefix(Pawn_AgeTracker __instance, Pawn ___pawn, long ___ageBiologicalTicksInt)
		{
            if (!___pawn.Dead && ___pawn.isOrk())
			{
				for (int i = 0; i < ___pawn.needs.AllNeeds.Count; i++)
				{
					if (___pawn.needs.AllNeeds[i] is Need_Orkoid_Fightyness need_Violence)
					{
					//	need_Violence.fought = true;

					}
				}

			}
			return true;
		}
	}
}
