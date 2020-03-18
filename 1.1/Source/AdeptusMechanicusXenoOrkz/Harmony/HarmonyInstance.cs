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

namespace AdeptusMechanicus.Orkz
{
    [StaticConstructorOnStartup]
    static class HarmonyInstance
    {
        static HarmonyInstance()
        {
            Harmony harmony = new Harmony("rimworld.ogliss.adeptusmechanicus.orkz");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            if (Prefs.DevMode) Log.Message(string.Format("Adeptus Mecanicus: Orkz: successfully completed {0} harmony patches.", harmony.GetPatchedMethods().Select(new Func<MethodBase, Patches>(Harmony.GetPatchInfo)).SelectMany((Patches p) => p.Prefixes.Concat(p.Postfixes).Concat(p.Transpilers)).Count((Patch p) => p.owner.Contains(harmony.Id))), false);
        }
    }
}