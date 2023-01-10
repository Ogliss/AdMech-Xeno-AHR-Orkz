using Verse;
using HarmonyLib;
using RimWorld;
using AdeptusMechanicus.ExtensionMethods;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(MeditationFocusTypeAvailabilityCache), "PawnCanUseInt")]
    public static class MeditationFocusTypeAvailabilityCache_PawnCanUseInt_Orkoid_Patch
    {
        [HarmonyPostfix]
        public static bool Postfix(bool __result, Pawn p, MeditationFocusDef type)
        {
            if (p.isOrkoid())
            {
                foreach (var item in TraitSet_GetTrait_Orkoid_Patch.traits)
                {
                    if (item.CurrentData.allowedMeditationFocusTypes.NotNullAndContains(type) && (item.def != TraitDefOf.Tough || p.Orkiness() >= Orkoid.Nob))
                    {
                        __result = true;
                    }
                    if (item.CurrentData.disallowedMeditationFocusTypes.NotNullAndContains(type) && (item.def != TraitDefOf.Tough || p.Orkiness() >= Orkoid.Nob))
                    {
                        __result = false;
                    }
                }
            }
            return __result;
        }

    }
}
