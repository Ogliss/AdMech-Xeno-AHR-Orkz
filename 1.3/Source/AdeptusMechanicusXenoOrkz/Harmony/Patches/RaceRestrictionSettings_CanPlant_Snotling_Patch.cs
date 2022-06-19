using HarmonyLib;
using Verse;

namespace AdeptusMechanicus.HarmonyInstance
{
    // RaceRestrictionSettings.CanPlant
    //    [HarmonyPatch(typeof(AlienRace.HarmonyPatches), "HasJobOnCellHarvestPostfix")]
    [HarmonyPatch(typeof(AlienRace.RaceRestrictionSettings), "CanPlant")]
    public static class RaceRestrictionSettings_CanPlant_Snotling_Patch
    {
        [HarmonyPostfix]
        public static void post(ThingDef plant, ThingDef race, ref bool __result)
        {
            if (race == AdeptusThingDefOf.OG_Snotling && plant == AdeptusThingDefOf.OG_Plant_Orkoid_Fungus)
            {
                __result = true;
            }
        }
    }
}