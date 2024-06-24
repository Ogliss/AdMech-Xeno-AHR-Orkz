using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using HarmonyLib;
using Verse.Sound;
using AdeptusMechanicus;
using AdeptusMechanicus.ExtensionMethods;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(Command_SetPlantToGrow), "IsPlantAvailable")]
    public static class Command_SetPlantToGrow_IsPlantAvailable_Orkoid_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(ThingDef plantDef, Map map, ref bool __result)
        {
            if (plantDef == AdeptusThingDefOf.OG_Plant_Orkoid_Fungus)
            {
                if (map.mapPawns.SpawnedColonyAnimals.Any(x => x.isSnotling()) || map.mapPawns.SlavesOfColonySpawned.Any(x => x.isOrk()) || map.mapPawns.FreeColonists.Any(x => x.isOrk()))
                {
                    __result = true;
                    return false;
                }
            }
            return true;
        }
    }
}
