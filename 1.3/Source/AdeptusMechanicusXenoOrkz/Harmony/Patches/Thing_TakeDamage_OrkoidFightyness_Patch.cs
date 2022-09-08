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
				Pawn damaged = __instance as Pawn;
				Pawn instigator = dinfo.Instigator as Pawn;
                if (instigator != null && instigator.isOrk())
                {
					for (int i = 0; i < instigator.needs.AllNeeds.Count; i++)
					{
						if (instigator.needs.AllNeeds[i] is Need_Orkoid_Fightyness need_Violence)
						{
						//	Log.Message(instigator + " is Fightin!");
							float old = need_Violence.CurLevel;
							need_Violence.Fought = true;
							need_Violence.foughtSocially = instigator.MentalState is MentalState_SocialFighting;
							need_Violence.CurLevel -= dinfo.Amount * 0.005f;
							if (need_Violence.CurLevel >= 0.9f) instigator.ageTracker.growth += dinfo.Amount * 0.005f;
							break;
						}
					}
				}
				if (damaged != null && (damaged.isGrot() || damaged.isOrk()) && !damaged.Dead)
				{
					for (int i = 0; i < damaged.needs.AllNeeds.Count; i++)
					{
						if (damaged.needs.AllNeeds[i] is Need_Orkoid_Fightyness need_Violence)
						{
							//	Log.Message(pawn2 + "is getting klobbered!");
							float old = need_Violence.CurLevel;
							need_Violence.Fought = true;
							need_Violence.foughtSocially = damaged.MentalState is MentalState_SocialFighting;
							need_Violence.CurLevel -= dinfo.Amount * 0.0025f;
							if (damaged.isOrk() && need_Violence.CurCategory >= RowdynessCategory.Free) damaged.ageTracker.growth += dinfo.Amount * 0.005f;
							break;
						}
					}
				}
			}
		}
	}
}
