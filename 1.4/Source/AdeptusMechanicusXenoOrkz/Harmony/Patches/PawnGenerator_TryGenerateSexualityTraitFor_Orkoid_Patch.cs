using Verse;
using HarmonyLib;
using AdeptusMechanicus.ExtensionMethods;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(PawnGenerator), "TryGenerateSexualityTraitFor")]
    public static class PawnGenerator_TryGenerateSexualityTraitFor_Orkoid_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(Pawn pawn)
        {
            if (pawn != null && pawn.RaceProps.Humanlike && pawn.isOrkoid())
            {
                return false;
            }
            return true;
        }

    }
}
