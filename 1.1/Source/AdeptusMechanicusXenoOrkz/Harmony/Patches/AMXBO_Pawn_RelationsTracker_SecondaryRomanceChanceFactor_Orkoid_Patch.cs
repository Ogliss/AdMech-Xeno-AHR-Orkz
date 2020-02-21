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
        public static void SecondaryRomanceChanceFactor(Pawn_RelationsTracker __instance, Pawn otherPawn, ref float __result)
        {
            Traverse traverse = Traverse.Create(__instance);
            Pawn pawn = (Pawn)AMXBO_Pawn_RelationsTracker_SecondaryRomanceChanceFactor_Orkoid_Patch.pawn.GetValue(__instance);
            bool flag = pawn != null && otherPawn != null;
            if (pawn.isOrkoid() || otherPawn.isOrkoid())
            {
                float num = 0f;
                __result *= num;
            }
        }
        public static FieldInfo pawn = typeof(Pawn_RelationsTracker).GetField("pawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
    }
}
