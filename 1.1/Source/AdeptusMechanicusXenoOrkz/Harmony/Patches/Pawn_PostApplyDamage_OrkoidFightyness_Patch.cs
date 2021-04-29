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
    [HarmonyPatch(typeof(Pawn), "PostApplyDamage")]
    public static class Pawn_PostApplyDamage_OrkoidFightyness_Patch
	{
		private static void Postfix(Pawn __instance, DamageInfo dinfo, float totalDamageDealt)
		{
			if (dinfo.Instigator != null)
			{
				if (dinfo.Instigator is Pawn pawn && pawn != null && pawn.isOrk())
				{
                    if (pawn.isOrk())
					{
						for (int i = 0; i < pawn.needs.AllNeeds.Count; i++)
						{
							if (pawn.needs.AllNeeds[i] is Need_Orkoid_Fightyness need_Violence)
							{
								float old = need_Violence.CurLevel;
								need_Violence.fought = true;
								need_Violence.foughtSocially = pawn.MentalState is MentalState_SocialFighting;
								//	need_Violence.CurLevel += totalDamageDealt;
								break;
							}
						}
					}
				}
				if (__instance.isGrot())
				{
					for (int i = 0; i < __instance.needs.AllNeeds.Count; i++)
					{
						if (__instance.needs.AllNeeds[i] is Need_Orkoid_Fightyness need_Violence)
						{
							float old = need_Violence.CurLevel;
							need_Violence.fought = true;
							need_Violence.foughtSocially = __instance.MentalState is MentalState_SocialFighting;
							//	need_Violence.CurLevel += totalDamageDealt;
							break;
						}
					}
				}
			}
		}
	}
}
