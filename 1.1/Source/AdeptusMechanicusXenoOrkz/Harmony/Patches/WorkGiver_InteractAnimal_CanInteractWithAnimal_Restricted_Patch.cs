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
    [HarmonyPatch(typeof(WorkGiver_InteractAnimal), "CanInteractWithAnimal")]
    public static class WorkGiver_InteractAnimal_CanInteractWithAnimal_Restricted_Patch
    {
        /*
        [HarmonyPrefix]
        public static void Prefix(Pawn humanlike, Pawn animal, float baseChance, ref bool __result)
        {

        }
        */
        [HarmonyPostfix]
        public static void Postfix(Pawn pawn, Pawn animal, bool forced, ref bool __result)
        {
            if (animal.isOrkoid() && __result)
            {
                __result = pawn.isOrkoid();
                if (!__result)
                {
                    JobFailReason.Is("AMO_NonOrkoidTamer".Translate(animal.def.label), null);
                }
            }
        }
    }
}
