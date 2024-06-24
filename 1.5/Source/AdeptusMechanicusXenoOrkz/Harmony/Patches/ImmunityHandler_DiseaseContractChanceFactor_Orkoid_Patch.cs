using System;
using Verse;
using HarmonyLib;
using AdeptusMechanicus.ExtensionMethods;
using RimWorld;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(ImmunityHandler), "DiseaseContractChanceFactor", new Type[] { typeof(HediffDef), typeof(BodyPartRecord)})]
    public static class ImmunityHandler_DiseaseContractChanceFactor_Orkoid_Patch
    {

        private static float Postfix(float __result, ImmunityHandler __instance, Pawn ___pawn, HediffDef diseaseDef, BodyPartRecord part)
        {

            if (diseaseDef==HediffDefOf.WoundInfection && !___pawn.Dead)
            {
                if (___pawn.isOrk()) __result *= 0.5f;
                if (___pawn.isGrot()) __result *= 0.75f;
            }
            return __result;
        }
    }
}
