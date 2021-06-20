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
    [HarmonyPatch(typeof(Thing), "TakeDamage")]
    public static class Thing_TakeDamage_OrkoidFightyness_Patch
	{
		private static void Postfix(Thing __instance, DamageInfo dinfo)
		{
			if (dinfo.Def.ExternalViolenceFor(__instance))
			{
				if (dinfo.Instigator != null)
				{
					if (dinfo.Instigator is Pawn pawn && pawn != null && pawn.isOrk() && !pawn.Dead)
					{
						if (pawn.isOrk())
						{
							for (int i = 0; i < pawn.needs.AllNeeds.Count; i++)
							{
								if (pawn.needs.AllNeeds[i] is Need_Orkoid_Fightyness need_Violence)
								{
								//	Log.Message(pawn + "is Fightin!");
									float old = need_Violence.CurLevel;
									need_Violence.fought = true;
									need_Violence.foughtSocially = pawn.MentalState is MentalState_SocialFighting;
									//	need_Violence.CurLevel += totalDamageDealt;
									break;
								}
							}
						}
					}
					if (__instance is Pawn pawn2 && pawn2 != null && (pawn2.isGrot() || pawn2.isOrk()) && !pawn2.Dead)
					{
						for (int i = 0; i < pawn2.needs.AllNeeds.Count; i++)
						{
							if (pawn2.needs.AllNeeds[i] is Need_Orkoid_Fightyness need_Violence)
							{
							//	Log.Message(pawn2 + "is getting klobbered!");
								float old = need_Violence.CurLevel;
								need_Violence.fought = true;
								need_Violence.foughtSocially = pawn2.MentalState is MentalState_SocialFighting;
								//	need_Violence.CurLevel += totalDamageDealt;
								break;
							}
						}
					}
				}
			}
		}
	}
}
