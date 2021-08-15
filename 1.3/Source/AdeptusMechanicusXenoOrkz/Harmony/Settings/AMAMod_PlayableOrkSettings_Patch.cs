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
    public static class AMAMod_SettingsCategory_Patch
    {
        [HarmonyPostfix]
        public static void ModsLoaded(ref string __result)
        {
            __result += ", " + "AdeptusMechanicus.Ork.ModName".Translate();
        }
    }
    [HarmonyPatch(typeof(AMAMod), "OrkSettings")]
    public static class AMAMod_PlayableOrkSettings_Patch
    {
        private static AMSettings settings = AMAMod.settings;
        private static AMAMod mod = AMAMod.Instance;
        private static float lineheight = AMAMod.lineheight;

        private static bool Dev => AMAMod.Dev;
        private static bool Xenobiologis => AdeptusIntergrationUtility.enabled_MagosXenobiologis;
        private static bool ShowXB => settings.ShowXenobiologisSettings;
        private static bool ShowRaces => (Xenobiologis && settings.ShowAllowedRaceSettings && ShowXB) || (!Xenobiologis && settings.ShowOrk);
        private static bool Setting => ShowRaces && settings.ShowOrk;

        private static int Options = 3;
        private static float RaceSettings => mod.Length(Setting, Options, lineheight, 8, ShowRaces ? 1 : 0);

        public static float MainMenuLength = 0;
        public static float MenuLength = 0;
        private static float inc = 0;
        [HarmonyPrefix]
        public static void OrkSettings_Prefix(ref AMAMod __instance, ref Listing_StandardExpanding listing_Main, ref float num2)
        {
            string label = "AdeptusMechanicus.Xenobiologis.ShowOrk".Translate() + " Settings";
            string tooltip = string.Empty;
            if (Dev)
            {
                label += " Main Length: " + MainMenuLength + " SubLength: " + MenuLength + " Passed: " + num2 + " Inc: " + inc;
            }
            if (!Xenobiologis)
            {
                if (!listing_Main.ButtonText(label, ref settings.ShowOrk, Dev, ref inc))
                {
                    return;
                }
            }
            if (ShowRaces)
            {
                Listing_StandardExpanding listing_Race = listing_Main.BeginSection((num2 != 0 ? num2 : RaceSettings) + inc, false, 3, 4, 0);
                if (Xenobiologis)
                {
                    listing_Race.CheckboxLabeled(label, ref settings.ShowOrk, Dev, ref inc, tooltip, false, true, ArmouryMain.collapseTex, ArmouryMain.expandTex);
                }
                if (settings.ShowOrk)
                {
                    Listing_StandardExpanding listing_General = listing_Race.BeginSection(MenuLength, true);
                    listing_General.ColumnWidth *= 0.32f;
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowOrkTek".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.Faction".Translate()),
                        ref settings.AllowOrkTek,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) || !settings.AllowOrkWeapons,
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) && settings.AllowOrkWeapons);
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Ork.AllowOrkoidFightyness".Translate(),
                        ref settings.OrkoidFightyness,
                        "AdeptusMechanicus.Ork.AllowOrkoidFightynessToolTip".Translate(),
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) || !settings.AllowOrkWeapons,
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) && settings.AllowOrkWeapons);
                    listing_General.NewColumn();
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowOrkFeral".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.Faction".Translate()),
                        ref settings.AllowOrkFeral,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")) || !settings.AllowOrkWeapons,
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")) && settings.AllowOrkWeapons);
                    listing_General.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.FightynessStatisfied".Translate(), ref settings.OrkoidFightynessStatisfied, ref settings.OrkoidFightynessStatisfiedBuffer, 0, int.MaxValue, "AdeptusMechanicus.Ork.FightynessStatisfiedToolTip".Translate(), 0.75f, 0.25f);
                    listing_General.NewColumn();
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowOrkRok".Translate(),
                        ref settings.AllowOrkRok,
                        null,
                        !settings.AllowOrkTek || !settings.AllowOrkWeapons,
                        settings.AllowOrkTek && settings.AllowOrkWeapons);
                    listing_Race.EndSection(listing_General);
                    MenuLength = Math.Max(listing_General.CurHeight, listing_General.MaxColumnHeightSeen);// listing_General.CurHeight > 0 ? listing_General.CurHeight : listing_General.MaxColumnHeightSeen;
                    Listing_StandardExpanding listing_FungalLabel = listing_Race.BeginSection(__instance.Length(Setting, 1, lineheight, 0, 0), true);
                    listing_FungalLabel.ColumnWidth *= 0.32f;
                    listing_FungalLabel.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.FungusOptions".Translate(), ref settings.FungusSpawnChance, ref settings.FungusSpawnChanceBuffer, 0f, 1f, "AdeptusMechanicus.Ork.FungusOptionsToolTip".Translate(), 0.75f, 0.25f);
                    listing_FungalLabel.NewColumn();
                    listing_FungalLabel.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.CocoonOptions".Translate(), ref settings.CocoonSpawnChance, ref settings.CocoonSpawnChanceBuffer, 0f, 1f, "AdeptusMechanicus.Ork.CocoonOptionsToolTip".Translate(), 0.75f, 0.25f);
                    listing_FungalLabel.NewColumn();
                    if (listing_FungalLabel.ButtonTextLine("Defaults"))
                    {
                        ResetFungalSettings();
                    }
                    listing_Race.EndSection(listing_FungalLabel);

                    Listing_StandardExpanding listing_Fungus = listing_Race.BeginSection(__instance.Length(Setting, 4, lineheight, 0, 0), true);
                    listing_Fungus.ColumnWidth *= 0.32f;
                    listing_Fungus.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.Squig".Translate(), ref settings.FungusSquigChance, ref settings.FungusSquigChanceBuffer, 0f, 1f, "AdeptusMechanicus.Ork.SquigToolTip".Translate(), 0.75f, 0.25f);
                    listing_Fungus.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.Snot".Translate(), ref settings.FungusSnotChance, ref settings.FungusSnotChanceBuffer, 0f, 1f, "AdeptusMechanicus.Ork.SnotToolTip".Translate(), 0.75f, 0.25f);
                    //    listing_Fungus.NewColumn();
                    listing_Fungus.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.Grot".Translate(), ref settings.FungusGrotChance, ref settings.FungusGrotChanceBuffer, 0f, 1f, "AdeptusMechanicus.Ork.GrotToolTip".Translate(), 0.75f, 0.25f);
                    listing_Fungus.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.Ork".Translate(), ref settings.FungusOrkChance, ref settings.FungusOrkChanceBuffer, 0f, 1f, "AdeptusMechanicus.Ork.OrkToolTip".Translate(), 0.75f, 0.25f);
                    listing_Fungus.NewColumn();
                    listing_Fungus.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.Squig".Translate(), ref settings.CocoonSquigChance, ref settings.CocoonSquigChanceBuffer, 0f, 1f, "AdeptusMechanicus.Ork.SquigToolTip".Translate(), 0.75f, 0.25f);
                    listing_Fungus.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.Snot".Translate(), ref settings.CocoonSnotChance, ref settings.CocoonSnotChanceBuffer, 0f, 1f, "AdeptusMechanicus.Ork.SnotToolTip".Translate(), 0.75f, 0.25f);
                    //    listing_Fungus.NewColumn();
                    listing_Fungus.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.Grot".Translate(), ref settings.CocoonGrotChance, ref settings.CocoonGrotChanceBuffer, 0f, 1f, "AdeptusMechanicus.Ork.GrotToolTip".Translate(), 0.75f, 0.25f);
                    listing_Fungus.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.Ork".Translate(), ref settings.CocoonOrkChance, ref settings.CocoonOrkChanceBuffer, 0f, 1f, "AdeptusMechanicus.Ork.OrkToolTip".Translate(), 0.75f, 0.25f);
                    listing_Fungus.NewColumn();
                    listing_Fungus.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.FungalMeds".Translate(), ref settings.FungusMedChance, ref settings.FungusMedChanceBuffer, 0f, 1f, "AdeptusMechanicus.Ork.FungalMedsToolTip".Translate(), 0.75f, 0.25f);
                    listing_Race.EndSection(listing_Fungus);
                }
                listing_Main.EndSection(listing_Race);
                MainMenuLength = listing_Race.CurHeight;
                num2 = MainMenuLength - inc;
            }
        }

        private static void ResetFungalSettings()
        {
            settings.FungusSpawnChance = 0.025f;
            settings.FungusSpawnChanceBuffer = settings.FungusSpawnChance.ToString();
            settings.FungusSquigChance = 1f;
            settings.FungusSquigChanceBuffer = settings.FungusSquigChance.ToString();
            settings.FungusSnotChance = 0.85f;
            settings.FungusSnotChanceBuffer = settings.FungusSnotChance.ToString();
            settings.FungusGrotChance = 0.1f;
            settings.FungusGrotChanceBuffer = settings.FungusGrotChance.ToString();
            settings.FungusOrkChance = 0.05f;
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
    }


    //[HarmonyPatch(typeof(AMAMod), "OrkSettings")]
    public static class AMAMod_PlayableOrkSettings_Patch_old
    {
        private static bool Xenobiologis = AdeptusIntergrationUtility.enabled_MagosXenobiologis;
        private static AMSettings settings = AMAMod.settings;
        [HarmonyPrefix, HarmonyPriority(401)]
        public static void Prefix(ref AMAMod __instance, ref Listing_StandardExpanding listing_Main, Rect rect)
        {
            bool showRaces = settings.ShowAllowedRaceSettings || !Xenobiologis;
            bool setting = showRaces && settings.ShowOrk;
            float lineheight = (Text.LineHeight + listing_Main.verticalSpacing);
            float w = rect.width * 0.480f;
            int Options = 6;
            float RaceSettings = __instance.Length(setting, Options, lineheight, 48, showRaces ? 1 : 0); //(settings.ShowImperium ? (lineheight * 2) : (lineheight * 1)) + (settings.ShowImperium ? 10 : 0);
            float options = __instance.Length(setting, Options - 1, lineheight, 0, 0);

            if (!Xenobiologis)
            {
                if (!listing_Main.ButtonText("AdeptusMechanicus.Ork.ModName".Translate() + " Options", ref settings.ShowOrk))
                {
                    return;
                }
            }
            Listing_StandardExpanding listing_Race = listing_Main.BeginSection(RaceSettings, false, 3, 4, 0);
            listing_Race.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.ShowOrk".Translate() + " Settings" , ref settings.ShowOrk, null, false, true, ArmouryMain.collapseTex, ArmouryMain.expandTex);

            if (setting)
            {
                Listing_StandardExpanding listing_General = listing_Race.BeginSection(__instance.Length(setting, 1, lineheight, 0, 0), true);
                listing_General.ColumnWidth *= 0.32f;
                listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowOrkTek".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.Faction".Translate()),
                    ref settings.AllowOrkTek,
                    null,
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) || !settings.AllowOrkWeapons,
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Tek")) && settings.AllowOrkWeapons);
                listing_General.NewColumn();
                listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowOrkFeral".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.Faction".Translate()),
                    ref settings.AllowOrkFeral,
                    null,
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")) || !settings.AllowOrkWeapons,
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Ork_Feral")) && settings.AllowOrkWeapons);
                listing_General.NewColumn();
                listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowOrkRok".Translate(),
                    ref settings.AllowOrkRok,
                    null,
                    !settings.AllowOrkTek || !settings.AllowOrkWeapons,
                    settings.AllowOrkTek && settings.AllowOrkWeapons);
                listing_Race.EndSection(listing_General);
                Listing_StandardExpanding listing_FungalLabel = listing_Race.BeginSection(__instance.Length(setting, 1, lineheight, 0, 0), true);
                listing_FungalLabel.ColumnWidth *= 0.32f;
                listing_FungalLabel.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.FungusOptions".Translate(), ref settings.FungusSpawnChance, ref settings.FungusSpawnChanceBuffer, 0f, 1f, "AdeptusMechanicus.Ork.FungusOptionsToolTip".Translate(), 0.75f, 0.25f);
                listing_FungalLabel.NewColumn();
                listing_FungalLabel.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.CocoonOptions".Translate(), ref settings.CocoonSpawnChance, ref settings.CocoonSpawnChanceBuffer, 0f, 1f, "AdeptusMechanicus.Ork.CocoonOptionsToolTip".Translate(), 0.75f, 0.25f);
                listing_FungalLabel.NewColumn();
                if (listing_FungalLabel.ButtonTextLine("Defaults"))
                {
                    settings.FungusSpawnChance = 0.025f;
                    settings.FungusSpawnChanceBuffer = settings.FungusSpawnChance.ToString();
                    settings.FungusSquigChance = 1f;
                    settings.FungusSquigChanceBuffer = settings.FungusSquigChance.ToString();
                    settings.FungusSnotChance = 0.85f;
                    settings.FungusSnotChanceBuffer = settings.FungusSnotChance.ToString();
                    settings.FungusGrotChance = 0.1f;
                    settings.FungusGrotChanceBuffer = settings.FungusGrotChance.ToString();
                    settings.FungusOrkChance = 0.05f;
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

                Listing_StandardExpanding listing_Fungus = listing_Race.BeginSection(__instance.Length(setting, 4, lineheight, 0, 0), true);
                listing_Fungus.ColumnWidth *= 0.32f;
                listing_Fungus.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.Squig".Translate(), ref settings.FungusSquigChance, ref settings.FungusSquigChanceBuffer, 0f, 1f, "AdeptusMechanicus.Ork.SquigToolTip".Translate(), 0.75f, 0.25f);
                listing_Fungus.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.Snot".Translate(), ref settings.FungusSnotChance, ref settings.FungusSnotChanceBuffer, 0f, 1f, "AdeptusMechanicus.Ork.SnotToolTip".Translate(), 0.75f, 0.25f);
            //    listing_Fungus.NewColumn();
                listing_Fungus.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.Grot".Translate(), ref settings.FungusGrotChance, ref settings.FungusGrotChanceBuffer, 0f, 1f, "AdeptusMechanicus.Ork.GrotToolTip".Translate(), 0.75f, 0.25f);
                listing_Fungus.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.Ork".Translate(), ref settings.FungusOrkChance, ref settings.FungusOrkChanceBuffer, 0f, 1f, "AdeptusMechanicus.Ork.OrkToolTip".Translate(), 0.75f, 0.25f);
                listing_Fungus.NewColumn();
                listing_Fungus.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.Squig".Translate(), ref settings.CocoonSquigChance, ref settings.CocoonSquigChanceBuffer, 0f, 1f, "AdeptusMechanicus.Ork.SquigToolTip".Translate(), 0.75f, 0.25f);
                listing_Fungus.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.Snot".Translate(), ref settings.CocoonSnotChance, ref settings.CocoonSnotChanceBuffer, 0f, 1f, "AdeptusMechanicus.Ork.SnotToolTip".Translate(), 0.75f, 0.25f);
            //    listing_Fungus.NewColumn();
                listing_Fungus.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.Grot".Translate(), ref settings.CocoonGrotChance, ref settings.CocoonGrotChanceBuffer, 0f, 1f, "AdeptusMechanicus.Ork.GrotToolTip".Translate(), 0.75f, 0.25f);
                listing_Fungus.TextFieldNumericLabeled<float>("AdeptusMechanicus.Ork.Ork".Translate(), ref settings.CocoonOrkChance, ref settings.CocoonOrkChanceBuffer, 0f, 1f, "AdeptusMechanicus.Ork.OrkToolTip".Translate(), 0.75f, 0.25f);
                listing_Race.EndSection(listing_Fungus);
            }
            listing_Main.EndSection(listing_Race);
        }
    }
    
    /*
    [HarmonyPatch(typeof(AMAMod), "PostModOptions")]
    public static class AMAMod_PostModOptions_Patch
    {
        [HarmonyPostfix, HarmonyPriority(399)]
        public static void PostModOptions_Prefix()
        {
            AMSettings settings = AMSettings.Instance;
            settings.Write();
        }

    }
    */
}