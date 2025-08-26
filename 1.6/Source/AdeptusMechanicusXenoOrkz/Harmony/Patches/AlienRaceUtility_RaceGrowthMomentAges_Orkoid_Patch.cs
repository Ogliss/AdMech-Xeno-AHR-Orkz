using Verse;
using HarmonyLib;
using AdeptusMechanicus.ExtensionMethods;
using AdeptusMechanicus.settings;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(AlienRaceUtility), "RaceGrowthMomentAges")]
    public static class AlienRaceUtility_RaceGrowthMomentAges_Orkoid_Patch
    {
        [HarmonyPostfix]
        public static int[] Postfix(int[] __result, Pawn pawn)
        {
            if (pawn != null && pawn.RaceProps.Humanlike && pawn.isOrkoid())
            {
                if (pawn.isOrk())
                {
                    if (AMAMod.Dev) Log.Message($"{pawn.Name} Using Orkoid GrowthMomentAgesOrk");
                    return GrowthUtility_Orkoid.GrowthMomentAgesOrk;
                }
                if (pawn.isGrot())
                {
                    if (AMAMod.Dev) Log.Message($"{pawn.Name} Using Orkoid GrowthMomentAgesGrot");
                    return GrowthUtility_Orkoid.GrowthMomentAgesGrot;
                }
            }
            return __result;
        }
    }
}
