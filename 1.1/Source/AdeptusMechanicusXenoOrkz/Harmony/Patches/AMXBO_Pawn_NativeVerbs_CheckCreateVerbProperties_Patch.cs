using RimWorld;
using Verse;
using HarmonyLib;
using System.Collections.Generic;
using System;
using AdeptusMechanicus.ExtensionMethods;
using System.Reflection;

namespace AdeptusMechanicus
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

	// Token: 0x0200005B RID: 91
	[HarmonyPatch(typeof(Pawn_NativeVerbs))]
	[HarmonyPatch("CheckCreateVerbProperties")]
	public static class AMXBO_Pawn_NativeVerbs_CheckCreateVerbProperties_Patch
	{
		// Token: 0x06000194 RID: 404 RVA: 0x0000DA4C File Offset: 0x0000BC4C
		private static Pawn pawn(Pawn_NativeVerbs instance)
		{
			return (Pawn)AMXBO_Pawn_NativeVerbs_CheckCreateVerbProperties_Patch.FI_pawn.GetValue(instance);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000DA70 File Offset: 0x0000BC70
		private static List<VerbProperties> cachedVerbProperties(Pawn_NativeVerbs instance)
		{
			return (List<VerbProperties>)AMXBO_Pawn_NativeVerbs_CheckCreateVerbProperties_Patch.FI_cachedVerbProperties.GetValue(instance);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000DA94 File Offset: 0x0000BC94
		private static bool Prefix(ref Pawn_NativeVerbs __instance)
		{
			bool flag = AMXBO_Pawn_NativeVerbs_CheckCreateVerbProperties_Patch.cachedVerbProperties(__instance) == null;
			if (flag)
			{
				bool flag2 = AMXBO_Pawn_NativeVerbs_CheckCreateVerbProperties_Patch.pawn(__instance).isSnotling();
				if (flag2)
				{
					AMXBO_Pawn_NativeVerbs_CheckCreateVerbProperties_Patch.FI_cachedVerbProperties.SetValue(__instance, new List<VerbProperties>());
					AMXBO_Pawn_NativeVerbs_CheckCreateVerbProperties_Patch.cachedVerbProperties(__instance).Add(NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.BeatFire));
					return false;
				}
			}
			return true;
		}

		// Token: 0x040000C7 RID: 199
		private static FieldInfo FI_pawn = typeof(Pawn_NativeVerbs).GetField("pawn", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.SetProperty);

		// Token: 0x040000C8 RID: 200
		private static FieldInfo FI_cachedVerbProperties = typeof(Pawn_NativeVerbs).GetField("cachedVerbProperties", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.SetProperty);
	}
}