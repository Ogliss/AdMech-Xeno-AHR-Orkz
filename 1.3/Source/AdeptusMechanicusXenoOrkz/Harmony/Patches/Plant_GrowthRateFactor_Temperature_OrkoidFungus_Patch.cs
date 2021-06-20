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
    [HarmonyPatch(typeof(Plant), "get_GrowthRateFactor_Temperature")]
    public static class Plant_GrowthRateFactor_Temperature_OrkoidFungus_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(Plant __instance, ref float __result)
        {
            if (__instance.def.isOrkoidFungus())
            {
                float num;
                if (!GenTemperature.TryGetTemperatureForCell(__instance.Position, __instance.Map, out num)) __result = 1f;
                else
                if (num < 0f)
                {
                    if (__instance.def == OGOrkThingDefOf.OG_Plant_Orkoid_Cocoon) __result = Mathf.InverseLerp(-50f, -10f, num);
                    else  __result = Mathf.InverseLerp(-30f, 0f, num);
                }
                else
                if (num > 62f) __result = Mathf.InverseLerp(116f, 62f, num);
                else __result = 1f;
                return false;
            }
            return true;
        }
    }
}
