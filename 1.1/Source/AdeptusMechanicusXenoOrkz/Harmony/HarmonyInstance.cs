using AlienRace;
using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus.HarmonyInstance
{
    [StaticConstructorOnStartup]
    static class HarmonyInstance
    {
        static HarmonyInstance()
        {
            Harmony harmony = new Harmony("rimworld.ogliss.adeptusmechanicus.orkz");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Type pawn_NativeVerbs = typeof(Pawn_NativeVerbs);
            if (AdeptusIntergrationUtility.enabled_SOS2)
            {
                HarmonyPatches.SOSConstructPatch();
            }
            HarmonyInstance._cachedVerbProperties = pawn_NativeVerbs.GetField("cachedVerbProperties", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.SetProperty);
            HarmonyInstance._pawnPawnNativeVerbs = pawn_NativeVerbs.GetField("pawn", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.SetProperty);
            if (Prefs.DevMode) Log.Message(string.Format("Adeptus Xenobiologis: Orkz: successfully completed {0} harmony patches.", harmony.GetPatchedMethods().Select(new Func<MethodBase, Patches>(Harmony.GetPatchInfo)).SelectMany((Patches p) => p.Prefixes.Concat(p.Postfixes).Concat(p.Transpilers)).Count((Patch p) => p.owner.Contains(harmony.Id))), false);
        }
        public static Pawn pawnPawnNativeVerbs(Pawn_NativeVerbs instance)
        {
            return (Pawn)HarmonyInstance._pawnPawnNativeVerbs.GetValue(instance);
        }

        public static List<VerbProperties> cachedVerbProperties(Pawn_NativeVerbs instance)
        {
            return (List<VerbProperties>)HarmonyInstance._cachedVerbProperties.GetValue(instance);
        }

        public static FieldInfo _pawnPawnNativeVerbs;
        public static FieldInfo _cachedVerbProperties;
    }
}