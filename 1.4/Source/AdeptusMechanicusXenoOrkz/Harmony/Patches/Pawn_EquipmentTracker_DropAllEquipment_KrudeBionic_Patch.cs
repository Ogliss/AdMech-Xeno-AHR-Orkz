using System;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using AdeptusMechanicus.ExtensionMethods;
using Verse;

namespace AdeptusMechanicus.HarmonyInstance
{
    // Token: 0x020000CB RID: 203
    [HarmonyPatch(typeof(Pawn_EquipmentTracker), "DropAllEquipment", null)]
    public class Pawn_EquipmentTracker_DropAllEquipment_KrudeBionic_Patch
    {
        // Token: 0x06000346 RID: 838 RVA: 0x0001D994 File Offset: 0x0001BB94
        [HarmonyPrefix]
        public static bool Patch_DropAllEquipment(Pawn ___pawn, ThingOwner<ThingWithComps> ___equipment)
        {
            Pawn pawn = ___pawn;
            if (pawn.health.Downed && pawn.health.hediffSet.hediffs.Any(hediff => hediff is Hediff_AddedPart addedPart && addedPart.def.defName.Contains("OG_KrudeBionikArm_RangedWeaponPlatform")) &&
                ___equipment.InnerListForReading.Any(thing => thing.def.IsRangedWeapon))
            {
                return false;
            }

            return true;
        }
    }
}
