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
		private static Pawn pawn(Pawn_NativeVerbs instance)
		{
			return (Pawn)Pawn_NativeVerbs_CheckCreateVerbProperties_Patch.FI_pawn.GetValue(instance);
		}

		private static List<VerbProperties> cachedVerbProperties(Pawn_NativeVerbs instance)
		{
			return (List<VerbProperties>)Pawn_NativeVerbs_CheckCreateVerbProperties_Patch.FI_cachedVerbProperties.GetValue(instance);
		}

		private static bool Prefix(ref Pawn_NativeVerbs __instance)
		{
			bool flag = Pawn_NativeVerbs_CheckCreateVerbProperties_Patch.cachedVerbProperties(__instance) == null;
			if (flag)
			{
				bool flag2 = Pawn_NativeVerbs_CheckCreateVerbProperties_Patch.pawn(__instance).isSnotling();
				if (flag2)
				{
					Pawn_NativeVerbs_CheckCreateVerbProperties_Patch.FI_cachedVerbProperties.SetValue(__instance, new List<VerbProperties>());
					Pawn_NativeVerbs_CheckCreateVerbProperties_Patch.cachedVerbProperties(__instance).Add(NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.BeatFire));
					return false;
				}
			}
			return true;
		}
		private static FieldInfo FI_pawn = typeof(Pawn_NativeVerbs).GetField("pawn", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.SetProperty);
		private static FieldInfo FI_cachedVerbProperties = typeof(Pawn_NativeVerbs).GetField("cachedVerbProperties", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.SetProperty);
	}
}