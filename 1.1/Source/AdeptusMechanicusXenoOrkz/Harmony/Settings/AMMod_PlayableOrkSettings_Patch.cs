using RimWorld;
using Verse;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using System;
using UnityEngine;
using AdeptusMechanicus.settings;
using System.Linq;
using AdeptusMechanicus.ExtensionMethods;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(AMAMod), "ModLoaded")]
    public static class MAMod_SettingsCategory_Patch
    {
        [HarmonyPostfix]
        public static void ModsLoaded(ref AMAMod __instance, ref string __result)
        {
            __result += ", " + "AMO_ModName".Translate();
        }
    }

    [HarmonyPatch(typeof(AMAMod), "OrkSettings")]
    public static class AMMod_PlayableOrkSettings_Patch
    {
        [HarmonyPrefix, HarmonyPriority(401)]
        public static bool OrkSettings_Prefix(ref AMAMod __instance, ref Listing_Standard listing_Main, Rect rect, Rect inRect, float num, float num2)
        {
            AMSettings settings = __instance.settings;
            bool showRaces = settings.ShowAllowedRaceSettings;
            bool setting = settings.ShowAllowedRaceSettings && settings.ShowOrk;
            float lineheight = (Text.LineHeight + listing_Main.verticalSpacing);
            float w = rect.width * 0.480f;
            int Options = 6;
            float RaceSettings = __instance.Length(setting, Options, lineheight, 48, showRaces ? 1 : 0); //(settings.ShowImperium ? (lineheight * 2) : (lineheight * 1)) + (settings.ShowImperium ? 10 : 0);
            float options = __instance.Length(setting, Options - 1, lineheight, 0, 0);

            Listing_Standard listing_Race = listing_Main.BeginSection_NewTemp(RaceSettings);
            listing_Race.CheckboxLabeled("AMXB_ShowOrk".Translate() + " Settings" + (Prefs.DevMode && SteamUtility.SteamPersonaName.Contains("Ogliss") ? " Menu Length: " + RaceSettings : "") + (Prefs.DevMode && SteamUtility.SteamPersonaName.Contains("Ogliss") && setting ? " options length: " + options : ""), ref settings.ShowOrk, null, false, true);

            if (setting)
            {
                Listing_Standard listing_General = listing_Race.BeginSection(__instance.Length(setting, 1, lineheight, 0, 0), true);
                listing_General.ColumnWidth *= 0.32f;
                listing_General.CheckboxLabeled("AMXB_AllowOrkTek".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()),
                    ref settings.AllowOrkTek,
                    null,
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) || !settings.AllowOrkWeapons,
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) && settings.AllowOrkWeapons);
                listing_General.NewColumn();
                listing_General.CheckboxLabeled("AMXB_AllowOrkFeral".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()),
                    ref settings.AllowOrkFeral,
                    null,
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")) || !settings.AllowOrkWeapons,
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")) && settings.AllowOrkWeapons);
                listing_General.NewColumn();
                listing_General.CheckboxLabeled("AMXB_AllowOrkRok".Translate(),
                    ref settings.AllowOrkRok,
                    null,
                    !settings.AllowOrkTek || !settings.AllowOrkWeapons,
                    settings.AllowOrkTek && settings.AllowOrkWeapons);
                listing_Race.EndSection(listing_General);
                Listing_Standard listing_FungalLabel = listing_Race.BeginSection(__instance.Length(setting, 1, lineheight, 0, 0), true);
                listing_FungalLabel.ColumnWidth *= 0.32f;
                listing_FungalLabel.TextFieldNumericLabeled<float>("AMO_FungusOptions".Translate(), ref settings.FungusSpawnChance, ref settings.FungusSpawnChanceBuffer, 0f, 1f, "AMO_FungusOptionsToolTip".Translate(), 0.75f, 0.25f);
                listing_FungalLabel.NewColumn();
                listing_FungalLabel.TextFieldNumericLabeled<float>("AMO_CocoonOptions".Translate(), ref settings.CocoonSpawnChance, ref settings.CocoonSpawnChanceBuffer, 0f, 1f, "AMO_CocoonOptionsToolTip".Translate(), 0.75f, 0.25f);
                listing_FungalLabel.NewColumn();
                if (listing_FungalLabel.ButtonTextLine("Defaults"))
                {
                    settings.FungusSpawnChance = 0.05f;
                    settings.FungusSpawnChanceBuffer = settings.FungusSpawnChance.ToString();
                    settings.FungusSquigChance = 1f;
                    settings.FungusSquigChanceBuffer = settings.FungusSquigChance.ToString();
                    settings.FungusSnotChance = 0.75f;
                    settings.FungusSnotChanceBuffer = settings.FungusSnotChance.ToString();
                    settings.FungusGrotChance = 0.2f;
                    settings.FungusGrotChanceBuffer = settings.FungusGrotChance.ToString();
                    settings.FungusOrkChance = 0.1f;
                    settings.FungusOrkChanceBuffer = settings.FungusOrkChance.ToString();

                    settings.CocoonSpawnChance = 0.25f;
                    settings.CocoonSpawnChanceBuffer = settings.CocoonSpawnChance.ToString();
                    settings.CocoonSquigChance = 0.15f;
                    settings.CocoonSquigChanceBuffer = settings.CocoonSquigChance.ToString();
                    settings.CocoonSnotChance = 0.2f;
                    settings.CocoonSnotChanceBuffer = settings.CocoonSnotChance.ToString();
                    settings.CocoonGrotChance = 0.35f;
                    settings.CocoonGrotChanceBuffer = settings.CocoonGrotChance.ToString();
                    settings.CocoonOrkChance = 0.3f;
                    settings.CocoonOrkChanceBuffer = settings.CocoonOrkChance.ToString();
                }
                listing_Race.EndSection(listing_FungalLabel);

                Listing_Standard listing_Fungus = listing_Race.BeginSection(__instance.Length(setting, 4, lineheight, 0, 0), true);
                listing_Fungus.ColumnWidth *= 0.32f;
                listing_Fungus.TextFieldNumericLabeled<float>("AMO_Squig".Translate(), ref settings.FungusSquigChance, ref settings.FungusSquigChanceBuffer, 0f, 1f, "AMO_SquigToolTip".Translate(), 0.75f, 0.25f);
                listing_Fungus.TextFieldNumericLabeled<float>("AMO_Snot".Translate(), ref settings.FungusSnotChance, ref settings.FungusSnotChanceBuffer, 0f, 1f, "AMO_SnotToolTip".Translate(), 0.75f, 0.25f);
            //    listing_Fungus.NewColumn();
                listing_Fungus.TextFieldNumericLabeled<float>("AMO_Grot".Translate(), ref settings.FungusGrotChance, ref settings.FungusGrotChanceBuffer, 0f, 1f, "AMO_GrotToolTip".Translate(), 0.75f, 0.25f);
                listing_Fungus.TextFieldNumericLabeled<float>("AMO_Ork".Translate(), ref settings.FungusOrkChance, ref settings.FungusOrkChanceBuffer, 0f, 1f, "AMO_OrkToolTip".Translate(), 0.75f, 0.25f);
                listing_Fungus.NewColumn();
                listing_Fungus.TextFieldNumericLabeled<float>("AMO_Squig".Translate(), ref settings.CocoonSquigChance, ref settings.CocoonSquigChanceBuffer, 0f, 1f, "AMO_SquigToolTip".Translate(), 0.75f, 0.25f);
                listing_Fungus.TextFieldNumericLabeled<float>("AMO_Snot".Translate(), ref settings.CocoonSnotChance, ref settings.CocoonSnotChanceBuffer, 0f, 1f, "AMO_SnotToolTip".Translate(), 0.75f, 0.25f);
            //    listing_Fungus.NewColumn();
                listing_Fungus.TextFieldNumericLabeled<float>("AMO_Grot".Translate(), ref settings.CocoonGrotChance, ref settings.CocoonGrotChanceBuffer, 0f, 1f, "AMO_GrotToolTip".Translate(), 0.75f, 0.25f);
                listing_Fungus.TextFieldNumericLabeled<float>("AMO_Ork".Translate(), ref settings.CocoonOrkChance, ref settings.CocoonOrkChanceBuffer, 0f, 1f, "AMO_OrkToolTip".Translate(), 0.75f, 0.25f);
                listing_Race.EndSection(listing_Fungus);
            }
            listing_Main.EndSection(listing_Race);
            __instance.XenobiologisOrkMenuLength = RaceSettings;

            return false;
        }
    }
    
    [HarmonyPatch(typeof(AMAMod), "PostModOptions")]
    public static class AMAMod_PostModOptions_Patch
    {
        [HarmonyPostfix, HarmonyPriority(399)]
        public static void PostModOptions_Prefix(ref Listing_Standard listing_Main, Rect inRect, float width, float menuLength)
        {
            AMSettings settings = AMSettings.Instance;
            settings.Write();
        }

    }
}