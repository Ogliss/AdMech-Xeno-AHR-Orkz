using Verse;
using HarmonyLib;
using AdeptusMechanicus.ExtensionMethods;
using RimWorld;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(HediffComp_Infecter), "CheckMakeInfection")]
    public static class HediffComp_Infecter_CheckMakeInfection_Orkoid_Patch
    {
        private static bool Prefix(HediffComp_Infecter __instance)
        {

            if (!__instance.Pawn.Dead || __instance.Pawn.isGrot())
            {
                CheckMakeInfection_Ork(__instance);
                return false;
            }
            return true;
        }
        private static void CheckMakeInfection_Ork(HediffComp_Infecter __instance)
        {
            if (__instance.Pawn.health.immunity.DiseaseContractChanceFactor(HediffDefOf.WoundInfection, __instance.parent.Part) <= 0.001f)
            {
                __instance.ticksUntilInfect = -3;
                return;
            }
            float num = 1f;
            HediffComp_TendDuration hediffComp_TendDuration = __instance.parent.TryGetComp<HediffComp_TendDuration>();
            if (hediffComp_TendDuration != null && hediffComp_TendDuration.IsTended)
            {
                num *= __instance.infectionChanceFactorFromTendRoom * 0.25f;
                num *= HediffComp_Infecter.InfectionChanceFactorFromTendQualityCurve.Evaluate(hediffComp_TendDuration.tendQuality * 2f);
            }
            num *= HediffComp_Infecter.InfectionChanceFactorFromSeverityCurve.Evaluate(__instance.parent.Severity * 0.75f);
            if (__instance.Pawn.Faction == Faction.OfPlayer)
            {
                num *= Find.Storyteller.difficulty.playerPawnInfectionChanceFactor;
            }
            if (Rand.Value < num)
            {
                __instance.ticksUntilInfect = -4;
                __instance.Pawn.health.AddHediff(HediffDefOf.WoundInfection, __instance.parent.Part, null, null);
                return;
            }
            __instance.ticksUntilInfect = -3;
        }
    }
}
