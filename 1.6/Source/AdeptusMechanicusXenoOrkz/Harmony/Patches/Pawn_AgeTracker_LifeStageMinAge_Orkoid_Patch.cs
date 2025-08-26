using RimWorld;
using Verse;
using HarmonyLib;
using AdeptusMechanicus.ExtensionMethods;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(Pawn_AgeTracker), "LifeStageMinAge")]
    public static class Pawn_AgeTracker_LifeStageMinAge_Orkoid_Patch
    {
		private static void Prefix(Pawn_AgeTracker __instance, Pawn ___pawn, ref LifeStageDef lifeStage)
		{
            if (!___pawn.Dead && ___pawn.isOrkoid())
			{
				if (___pawn.isOrk())
                {
                    if (lifeStage == LifeStageDefOf.HumanlikeBaby)
                    {
                        lifeStage = OrkoidLifeStageDefOf.OG_Lifestage_Ork_Whelp;
                        return;
                    }
                    if (lifeStage == LifeStageDefOf.HumanlikeChild)
                    {
                        lifeStage = OrkoidLifeStageDefOf.OG_Lifestage_Ork_Runt;
                        return;
                    }
                    if (lifeStage == LifeStageDefOf.HumanlikeAdult)
                    {
                        lifeStage = OrkoidLifeStageDefOf.OG_Lifestage_Ork_Adult;
                        return;
                    }
                }
				if (___pawn.isGrot())
                {
                    if (lifeStage == LifeStageDefOf.HumanlikeBaby)
                    {
                        lifeStage = OrkoidLifeStageDefOf.OG_Lifestage_Grot_Whelp;
                        return;
                    }
                    if (lifeStage == LifeStageDefOf.HumanlikeChild)
                    {
                        lifeStage = OrkoidLifeStageDefOf.OG_Lifestage_Grot_Runt;
                        return;
                    }
                    if (lifeStage == LifeStageDefOf.HumanlikeAdult)
                    {
                        lifeStage = OrkoidLifeStageDefOf.OG_Lifestage_Grot_Adult;
                        return;
                    }
                }
			}
		}

	}
}
