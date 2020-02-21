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
        }
    }
}