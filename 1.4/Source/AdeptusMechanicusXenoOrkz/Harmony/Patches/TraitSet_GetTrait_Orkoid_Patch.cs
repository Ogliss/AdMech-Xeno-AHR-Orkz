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
    [HarmonyPatch(typeof(TraitSet), "GetTrait", new Type[] { typeof(TraitDef) })]
    [HarmonyPatch(typeof(TraitSet), "GetTrait", new Type[] { typeof(TraitDef), typeof(int) })]
    public static class TraitSet_GetTrait_Orkoid_Patch
    {
        public static List<Trait> traits = new List<Trait>()
        {
            new Trait(TraitDefOf.Bloodlust),
            new Trait(TraitDefOf.Psychopath),
            new Trait(TraitDefOf.Tough)
        };

        [HarmonyPostfix]
        public static void Postfix(TraitDef tDef, Pawn ___pawn, ref Trait __result)
        {
            if (___pawn != null)
            {
                if (___pawn.isOrkoid())
                {
                    foreach (var item in traits)
                    {
                        if (tDef == item.def && (tDef != TraitDefOf.Tough || ___pawn.Orkiness() >= Orkoid.Nob))
                        {
                            __result = item;
                        }
                    }
                }
            }
        }
    }
}
