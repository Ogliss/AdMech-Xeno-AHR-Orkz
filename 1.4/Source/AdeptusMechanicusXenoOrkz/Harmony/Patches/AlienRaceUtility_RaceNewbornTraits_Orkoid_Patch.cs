using Verse;
using HarmonyLib;
using AdeptusMechanicus.ExtensionMethods;
using AdeptusMechanicus.settings;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(AlienRaceUtility), "RaceNewbornTraits")]
    public static class AlienRaceUtility_RaceNewbornTraits_Orkoid_Patch
    {
        [HarmonyPostfix]
        public static bool Postfix(bool __result, Pawn pawn)
        {
            if (pawn != null && pawn.RaceProps.Humanlike && pawn.isOrkoid())
            {
                if (AMAMod.Dev) Log.Message($"{pawn.Name} Using Orkoid RaceNewbornTraits");
                return false;
            }
            return __result;
        }

    }

    [HarmonyPatch(typeof(AlienRaceUtility), "RaceMinTraitAge")]
    public static class AlienRaceUtility_RaceMinTraitAge_Orkoid_Patch
    {
        [HarmonyPostfix]
        public static int Postfix(int __result, Pawn pawn)
        {
            if (pawn != null && pawn.RaceProps.Humanlike && pawn.isOrkoid())
            {
                if (AMAMod.Dev) Log.Message($"{pawn.Name} Default: {__result} Using Orkoid RaceMinTraitAge 0");
                return 0;
            }
            return __result;
        }

    }
}
