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
using UnityEngine;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(Plant), "get_GrowthRate")]
    public static class Plant_GrowthRate_OrkoidFungus_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(Plant __instance, ref float __result)
        {
            if (__instance.def.isOrkoidFungus())
            {
                if (__instance.Blighted) __result = 0f;
            //    if (__instance.Spawned && !PlantUtility.GrowthSeasonNow(__instance.Position, __instance.Map, false)) __result = 0f;
                else __result = __instance.GrowthRateFactor_Fertility * __instance.GrowthRateFactor_Temperature * __instance.GrowthRateFactor_Light;
                return false;
            }
            return true;
        }
    }
}
