using RimWorld;
using Verse;
using HarmonyLib;
using System.Collections.Generic;
using System;
using AdeptusMechanicus.ExtensionMethods;
using System.Reflection;

namespace AdeptusMechanicus.HarmonyInstance
{
	/*
    [HarmonyPatch(typeof(Pawn_NativeVerbs), "CheckCreateVerbProperties")]
    public static class AMXBO_Pawn_NativeVerbs_CheckCreateVerbProperties_Patch
    {
        public static bool Prefix(ref Pawn_NativeVerbs __instance)
        {
            bool flag = Orkz.HarmonyInstance._cachedVerbProperties.GetValue(__instance) != null;
            bool result;
            if (flag)
            {
                result = true;
            }
            else
            {
                bool flag2 = Orkz.HarmonyInstance.pawnPawnNativeVerbs(__instance).isSnotling();
                if (flag2)
                {
                    Orkz.HarmonyInstance.cachedVerbProperties(__instance).Add(NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.BeatFire));
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }
    }
	*/

	[HarmonyPatch(typeof(Pawn_NativeVerbs))]
	[HarmonyPatch("CheckCreateVerbProperties")]
	public static class Pawn_NativeVerbs_CheckCreateVerbProperties_Patch
	{
		private static bool Prefix(ref Pawn_NativeVerbs __instance)
		{
			if (__instance.cachedVerbProperties == null)
			{
				if (__instance.pawn.isSnotling())
				{
					__instance.cachedVerbProperties = new List<VerbProperties>();
					__instance.cachedVerbProperties.Add(NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.BeatFire));
					return false;
				}
			}
			return true;
		}
	}
}