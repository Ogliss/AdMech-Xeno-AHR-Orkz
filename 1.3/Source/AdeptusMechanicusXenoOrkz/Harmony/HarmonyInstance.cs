using AlienRace;
using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus.HarmonyInstance
{
    [StaticConstructorOnStartup]
    static class HarmonyInstanceOrkz
    {
        static HarmonyInstanceOrkz()
        {
            Harmony harmony = new Harmony("rimworld.ogliss.adeptusmechanicus.orkz");
            MethodInfo targetmethod1 = AccessTools.TypeByName("RimWorld.JobDriver_PlantWork").GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Instance).First().
                GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).
                MaxBy(mi => mi.GetMethodBody()?.GetILAsByteArray().Length ?? -1);
            bool t1 = targetmethod1 == null;
            if (t1)
            {
                Log.Warning("Target method null");
            }
            MethodInfo patchmethod1 = typeof(JobDriver_PlantWork_MakeNewToils_Fungus_Transpiler).GetMethod("Transpiler");
            bool p1 = patchmethod1 == null;
            if (p1)
            {
                Log.Warning("Patch method null");
            }
            harmony.Patch(targetmethod1, transpiler: new HarmonyMethod(patchmethod1));
            /*
            MethodInfo targetmethod2 = AccessTools.TypeByName("AnimalGear.AlienChildrenCompatibilityHarmony.Alien_DrawAddons_Patch")?.GetMethod("DrawAddons_Pre");
            bool t2 = targetmethod2 != null;
            if (t2)
            {
                Log.Warning("Patch AnimalGear.AlienChildrenCompatibilityHarmony.Alien_DrawAddons_Patch method Found!!!");
            }
            */
            /*
            MethodInfo method3 = AccessTools.TypeByName("RBB_Code.JobDriver_CatchFish").GetMethod("MakeNewToils", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo method4 = typeof(JobDriver_CatchFish_MakeNewToils_Snotling_Patch).GetMethod("Prefix");
            bool flag3 = method3 == null;
            if (flag3)
            {
                Log.Warning("RBB_Code.JobDriver_CatchFish method null");
            }
            else
            {
                if (method4 == null)
                {
                    Log.Warning("JobDriver_CatchFish_MakeNewToils_Snotling_Patch Prefix method null");
                }
                else
                    harmony.Patch(method3, prefix: new HarmonyMethod(method4));
            }
            MethodInfo method5 = AccessTools.TypeByName("Quarry.WorkGiver_MineQuarry").GetMethod("JobOnThing");
            MethodInfo method6 = typeof(WorkGiver_MineQuarry_JobOnThing_Snotling_Patch).GetMethod("Prefix");
            bool flag4 = method5 == null;
            if (flag4)
            {
                Log.Warning("Quarry.WorkGiver_MineQuarry method null");
            }
            else
            {
                if (method4 == null)
                {
                    Log.Warning("WorkGiver_MineQuarry_JobOnThing_Snotling_Patch Prefix method null");
                }
                else
                    harmony.Patch(method5, prefix: new HarmonyMethod(method6));
            }
            */
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Type pawn_NativeVerbs = typeof(Pawn_NativeVerbs);
            /*
            if (AdeptusIntergrationUtility.enabled_SOS2)
            {
                HarmonyPatches.SOSConstructPatch();
            }
            */
            if (Prefs.DevMode) Log.Message(string.Format("Adeptus Xenobiologis: Orkz: successfully completed {0} harmony patches.", harmony.GetPatchedMethods().Select(new Func<MethodBase, Patches>(Harmony.GetPatchInfo)).SelectMany((Patches p) => p.Prefixes.Concat(p.Postfixes).Concat(p.Transpilers)).Count((Patch p) => p.owner.Contains(harmony.Id))));
        }


    }
}