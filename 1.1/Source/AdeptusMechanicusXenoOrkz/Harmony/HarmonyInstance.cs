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
            MethodInfo method = AccessTools.TypeByName("RimWorld.JobDriver_PlantWork").GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Instance).First().
                GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).
                MaxBy(mi => mi.GetMethodBody()?.GetILAsByteArray().Length ?? -1);
            bool flag1 = method == null;
            if (flag1)
            {
                Log.Warning("Target method null");
            }
            MethodInfo method2 = typeof(JobDriver_PlantWork_MakeNewToils_Fungus_Transpiler).GetMethod("Transpiler");
            bool flag2 = method2 == null;
            if (flag2)
            {
                Log.Warning("Patch method null");
            }
            
            harmony.Patch(method, transpiler: new HarmonyMethod(method2));
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Type pawn_NativeVerbs = typeof(Pawn_NativeVerbs);
            if (AdeptusIntergrationUtility.enabled_SOS2)
            {
                HarmonyPatches.SOSConstructPatch();
            }
            HarmonyInstanceOrkz._cachedVerbProperties = pawn_NativeVerbs.GetField("cachedVerbProperties", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.SetProperty);
            HarmonyInstanceOrkz._pawnPawnNativeVerbs = pawn_NativeVerbs.GetField("pawn", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.SetProperty);
            if (Prefs.DevMode) Log.Message(string.Format("Adeptus Xenobiologis: Orkz: successfully completed {0} harmony patches.", harmony.GetPatchedMethods().Select(new Func<MethodBase, Patches>(Harmony.GetPatchInfo)).SelectMany((Patches p) => p.Prefixes.Concat(p.Postfixes).Concat(p.Transpilers)).Count((Patch p) => p.owner.Contains(harmony.Id))), false);
        }


        public static Pawn pawnPawnNativeVerbs(Pawn_NativeVerbs instance)
        {
            return (Pawn)HarmonyInstanceOrkz._pawnPawnNativeVerbs.GetValue(instance);
        }

        public static List<VerbProperties> cachedVerbProperties(Pawn_NativeVerbs instance)
        {
            return (List<VerbProperties>)HarmonyInstanceOrkz._cachedVerbProperties.GetValue(instance);
        }

        public static FieldInfo _pawnPawnNativeVerbs;
        public static FieldInfo _cachedVerbProperties;
    }
}