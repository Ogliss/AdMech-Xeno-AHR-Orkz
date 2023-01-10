using RimWorld;
using Verse;
using HarmonyLib;
using AdeptusMechanicus.ExtensionMethods;
using UnityEngine;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(PawnUtility), "BodyResourceGrowthSpeed")]
	public static class Pawn_AgeTracker_BodyResourceGrowthSpeed_Orkoid_Patch
	{
		// PawnUtility.BodyResourceGrowthSpeed
		private static float Postfix(float __result, Pawn pawn)
		{
			float result = __result;
		//	bool selected = pawn != null && pawn.Map != null && Find.Selector.SingleSelectedThing == pawn;
			if (!pawn.Dead && pawn.isOrk())
			{
				if (pawn.def is AlienRace.ThingDef_AlienRace alienDef/* && selected*/)
				{
				//	Log.Message($"BodyResourceGrowthSpeed {alienDef.LabelCap} minAgeForAdulthood: {alienDef.alienRace.generalSettings.minAgeForAdulthood} <= {pawn.ageTracker.AgeBiologicalYearsFloat} == {alienDef.alienRace.generalSettings.minAgeForAdulthood <= pawn.ageTracker.AgeBiologicalYearsFloat}");
				}

				LifeStageAge adultOrk = null;
				int adultOrkInd = 0;
				for (int i = 0; i < pawn.def.race.lifeStageAges.Count; i++)
				{
					LifeStageAge lifeStage = pawn.def.race.lifeStageAges[i];
					if (lifeStage.def.reproductive)
					{
						adultOrk = lifeStage;
						adultOrkInd = i;
						break;
					}
				}
			//	if (selected) Log.Message($"BodyResourceGrowthSpeed {pawn.LabelCap} StageIndexForAdulthood: {adultOrk}");
				float need = 0f;
				for (int i = 0; i < pawn.needs.AllNeeds.Count; i++)
				{
					if (pawn.needs.AllNeeds[i] is Need_Orkoid_Fightyness need_Violence)
					{
						need = need_Violence.CurLevel;
						break;
					}
				}
			//	if (selected) Log.Message($"BodyResourceGrowthSpeed {pawn.LabelCap} need: {need}");
				if (pawn.ageTracker.CurLifeStageIndex >= adultOrkInd) result -= Mathf.Clamp(need, 0f, result);
			//	if (selected) Log.Message($"BodyResourceGrowthSpeed {pawn.LabelCap} result: {__result}, {result}");

			}
			return result;
		}
	}
}
