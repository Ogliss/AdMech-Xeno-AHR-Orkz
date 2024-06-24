using Verse;
using HarmonyLib;
using AdeptusMechanicus.ExtensionMethods;

namespace AdeptusMechanicus.HarmonyInstance
{

    [HarmonyPatch(typeof(AlienRaceUtility), "RaceAgeSkillFactor")]
    public static class AlienRaceUtility_RaceAgeSkillFactor_Orkoid_Patch
    {
        [HarmonyPostfix]
        public static SimpleCurve Postfix(SimpleCurve __result, Pawn pawn)
        {
            if (pawn != null && pawn.RaceProps.Humanlike && pawn.isOrkoid())
            {
            //    if (AMAMod.Dev) Log.Message($"{pawn.Name} Using Orkoid AgeSkillMaxFactorCurve");
                return AgeSkillFactor;
            }
            return __result;
        }

        private static readonly SimpleCurve AgeSkillFactor = new SimpleCurve
        {
            {
                new CurvePoint(0f, 0.5f),
                true
            },
            {
                new CurvePoint(2f, 1f),
                true
            }
        };
    }
}
