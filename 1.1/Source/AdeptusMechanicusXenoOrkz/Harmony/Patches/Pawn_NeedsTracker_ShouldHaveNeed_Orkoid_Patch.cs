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
    [HarmonyPatch(typeof(Pawn_NeedsTracker), "ShouldHaveNeed")]
    public static class Pawn_NeedsTracker_ShouldHaveNeed_Orkoid_Patch
	{
		private static void Postfix(Pawn_NeedsTracker __instance, Pawn ___pawn, NeedDef nd, ref bool __result)
		{
            if (___pawn.isOrkoid() && nd == AdeptusNeedDefOf.Beauty)
			{
				__result = false;
			}
		}
	}
}
