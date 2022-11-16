using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using HarmonyLib;
using Verse.Sound;
using AdeptusMechanicus;
using AdeptusMechanicus.ExtensionMethods;

namespace AdeptusMechanicus.HarmonyInstance
{

	[HarmonyPatch(typeof(Pawn_AgeTracker), "RecalculateLifeStageIndex")]
    public static class Pawn_AgeTracker_RecalculateLifeStageIndex_Orkoid_Patch
	{
		private static void Prefix(Pawn_AgeTracker __instance, Pawn ___pawn, ref float ___growth)
		{
			
            if (!___pawn.Dead && ___pawn.isOrk())
			{
				/*
				if (___pawn.def is AlienRace.ThingDef_AlienRace alienDef && Find.Selector.SingleSelectedThing == ___pawn)
				{
					Log.Message($"Prefix RecalculateLifeStageIndex {alienDef.LabelCap} minAgeForAdulthood: {alienDef.alienRace.generalSettings.minAgeForAdulthood} Growth: {___growth}");
				}
				*/

			}
		}

		private static void Postfix(Pawn_AgeTracker __instance, Pawn ___pawn, ref float ___growth)
		{
			
            if (!___pawn.Dead && ___pawn.isOrk())
			{
				/*
				if (___pawn.def is AlienRace.ThingDef_AlienRace alienDef && Find.Selector.SingleSelectedThing == ___pawn)
				{
					Log.Message($"Postfix RecalculateLifeStageIndex {alienDef.LabelCap} minAgeForAdulthood: {alienDef.alienRace.generalSettings.minAgeForAdulthood} Growth: {___growth}");
				}
				*/
			}
		}
	}
}
