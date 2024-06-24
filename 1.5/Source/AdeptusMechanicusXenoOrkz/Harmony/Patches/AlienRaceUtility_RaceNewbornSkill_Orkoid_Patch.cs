using Verse;
using HarmonyLib;
using AdeptusMechanicus.ExtensionMethods;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(AlienRaceUtility), "RaceNewbornSkill")]
    public static class AlienRaceUtility_RaceNewbornSkill_Orkoid_Patch
    {
        [HarmonyPostfix]
        public static bool Postfix(bool __result, Pawn pawn)
        {
            if (__result && pawn != null && pawn.RaceProps.Humanlike && pawn.isOrkoid())
            {
            //    if (AMAMod.Dev) Log.Message($"{pawn.Name} Using Orkoid AgeSkillMaxFactorCurve");
                return false;
            }
            return __result;
        }

    }
}
