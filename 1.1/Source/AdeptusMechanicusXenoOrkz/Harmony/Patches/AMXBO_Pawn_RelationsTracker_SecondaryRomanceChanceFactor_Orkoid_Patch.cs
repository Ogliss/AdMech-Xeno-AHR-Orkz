using System;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using AdeptusMechanicus.ExtensionMethods;
using Verse;

namespace AdeptusMechanicus
{
    // Token: 0x020000CB RID: 203
    [HarmonyPatch(typeof(Pawn_RelationsTracker), "SecondaryRomanceChanceFactor", null)]
    public class AMXBO_Pawn_RelationsTracker_SecondaryRomanceChanceFactor_Orkoid_Patch
    {
        // Token: 0x06000346 RID: 838 RVA: 0x0001D994 File Offset: 0x0001BB94
        [HarmonyPostfix]
        public static void SecondaryRomanceChanceFactor(Pawn ___pawn, Pawn otherPawn, ref float __result)
        {
            bool flag = ___pawn != null && otherPawn != null;
            if (___pawn.isOrkoid() || otherPawn.isOrkoid())
            {
                float num = 0f;
                __result *= num;
            }
        }
    }
}
