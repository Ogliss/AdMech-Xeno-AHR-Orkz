using RimWorld;
using Verse;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using System;
using UnityEngine;
using AdeptusMechanicus.settings;
using System.Linq;

namespace AdeptusMechanicus
{
    [HarmonyPatch(typeof(AMAMod), "SettingsCategory")]
    public static class AMO_AMAMod_SettingsCategory_Patch
    {
        [HarmonyPostfix, HarmonyPriority(399)]
        public static void SettingsCategory_Postfix(ref AMAMod __instance, ref string __result)
        {
            __result += ", " + "AMO_ModName".Translate();
        }
    }
	/*
    [HarmonyPatch(typeof(AMAMod), "get_MenuLength")]
    public static class AMO_AMAMod_MenuLength_Patch
    {
        [HarmonyPostfix]
        public static void MenuLength_Postfix(ref float __result)
        {
            //    Log.Message(string.Format("PreModOptions_Prefix num2: {0}",  num2));
            __result += (AMSettings.Instance.ShowOrk ? (AdeptusIntergrationUtil.enabled_MagosXenobiologis ? 60f : 120f) : 0);

            //    Log.Message(string.Format("PreModOptions_Prefix num2: {0}", num2));
        }

    }
	*/
    [HarmonyPatch(typeof(AMMod), "OrkSettings")]
    public static class AMO_AMMod_PlayableOrkSettings_Patch
    {
        [HarmonyPrefix, HarmonyPriority(401)]
        public static bool OrkSettings_Prefix(ref Listing_Standard listing_Standard, Rect rect, Rect inRect, float num, float num2)
        {
            AMSettings AMAsettings = SettingsHelper.latest;
            listing_Standard.CheckboxLabeled("AMO_ModName".Translate() + " Settings", ref AMSettings.Instance.ShowOrk);
            if (AMAsettings.ShowOrk)
            {
                Rect rect2 = new Rect(rect.x, rect.y + 10, num, 120f);
                ShowOrk(ref listing_Standard, rect2, AMAsettings);
            }
            return false;
        }
        static void ShowOrk(ref Listing_Standard listing_Standard, Rect rect2, AMSettings settings)
        {
            listing_Standard.BeginSection(120f);
            Widgets.CheckboxLabeled(rect2.TopHalf().TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowOrkTek".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref settings.AllowOrkTek, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")));
            Widgets.CheckboxLabeled(rect2.TopHalf().BottomHalf().LeftHalf().ContractedBy(4), "AMXB_AllowOrkFeral".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref settings.AllowOrkFeral, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")));
            Widgets.CheckboxLabeled(rect2.TopHalf().TopHalf().RightHalf().ContractedBy(4), "AMXB_AllowOrkRok".Translate(), ref settings.AllowOrkRok, false);

            AMMod.TextFieldNumericLabeled<float>(rect2.BottomHalf().TopHalf().TopHalf().LeftHalf().LeftHalf(), "AMO_FungusSpawnChance".Translate() + " " + "AMO_Chance".Translate(), ref settings.FungusSpawnChance, ref settings.FungusSpawnChanceBuffer, 0f, 1f);
            AMMod.TextFieldNumericLabeled<float>(rect2.BottomHalf().TopHalf().TopHalf().LeftHalf().RightHalf(), "AMO_Snot".Translate() + " " + "AMO_Chance".Translate(), ref settings.FungusSnotChance, ref settings.FungusSnotChanceBuffer, 0f, 1f);
            AMMod.TextFieldNumericLabeled<float>(rect2.BottomHalf().TopHalf().TopHalf().RightHalf().RightHalf(), "AMO_Grot".Translate() + " " + "AMO_Chance".Translate(), ref settings.FungusGrotChance, ref settings.FungusGrotChanceBuffer, 0f, 1f);
            AMMod.TextFieldNumericLabeled<float>(rect2.BottomHalf().TopHalf().TopHalf().RightHalf().LeftHalf(), "AMO_Ork".Translate() + " " + "AMO_Chance".Translate(), ref settings.FungusOrkChance, ref settings.FungusOrkChanceBuffer, 0f, 1f);

            AMMod.TextFieldNumericLabeled<float>(rect2.BottomHalf().BottomHalf().TopHalf().LeftHalf().LeftHalf(), "AMO_CocoonSpawnChance".Translate() + " " + "AMO_Chance".Translate(), ref settings.CocoonSpawnChance, ref settings.CocoonSpawnChanceBuffer, 0f, 1f);
            AMMod.TextFieldNumericLabeled<float>(rect2.BottomHalf().BottomHalf().TopHalf().LeftHalf().RightHalf(), "AMO_Snot".Translate() + " " + "AMO_Chance".Translate(), ref settings.CocoonSnotChance, ref settings.CocoonSnotChanceBuffer, 0f, 1f);
            AMMod.TextFieldNumericLabeled<float>(rect2.BottomHalf().BottomHalf().TopHalf().RightHalf().RightHalf(), "AMO_Grot".Translate() + " " + "AMO_Chance".Translate(), ref settings.CocoonGrotChance, ref settings.CocoonGrotChanceBuffer, 0f, 1f);
            AMMod.TextFieldNumericLabeled<float>(rect2.BottomHalf().BottomHalf().TopHalf().RightHalf().LeftHalf(), "AMO_Ork".Translate() + " " + "AMO_Chance".Translate(), ref settings.CocoonOrkChance, ref settings.CocoonOrkChanceBuffer, 0f, 1f);

            listing_Standard.EndSection(listing_Standard);
        }
    }
    
    [HarmonyPatch(typeof(AMAMod), "PostModOptions")]
    public static class AMO_AMAMod_PostModOptions_Patch
    {
        [HarmonyPostfix, HarmonyPriority(399)]
        public static void PostModOptions_Prefix(ref Listing_Standard listing_Standard, Rect inRect, ref float num, ref float num2)
        {
            AMSettings settings = AMSettings.Instance;
            settings.Write();
        }

    }
}