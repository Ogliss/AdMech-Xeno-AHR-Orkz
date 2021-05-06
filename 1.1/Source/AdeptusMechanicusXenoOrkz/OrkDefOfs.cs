using System;
using Verse;
using RimWorld;

namespace AdeptusMechanicus
{
	[DefOf]
	public static class OrkThingDefOf
	{
		static OrkThingDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(OrkThingDefOf));
		}

        // Plant Defs
        public static ThingDef OG_Plant_Orkoid_Cocoon;
        public static ThingDef OG_Plant_Orkoid_Fungus;
        public static ThingDef OG_FilthBlood_Orkoid;

        // Building Defs
    //    public static ThingDef OG_Ork_FermentingBarrel;

        // Item Defs
    //    public static ThingDef OG_Ork_Waart;
    //    public static ThingDef OG_Ork_Grog;

        // Backstory Defs
        public static AlienRace.BackstoryDef OG_Ork_Base_Child;
        public static AlienRace.BackstoryDef OG_Grot_Base_Child;

        // Humanlike Race Defs
        public static AlienRace.ThingDef_AlienRace OG_Alien_Ork;
        public static AlienRace.ThingDef_AlienRace OG_Alien_Grot;
        

        public static ThingDef OG_Snotling;
        public static ThingDef OG_Squig;
        public static ThingDef OG_Squig_Ork;
    }

    [DefOf]
    public static class OrkPawnKindDefOf
    {
        static OrkPawnKindDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OrkPawnKindDefOf));
        }
        public class Player
        {
            public static PawnKindDef OG_Squig;
        }
        public static PawnKindDef OG_Squig;

        public static PawnKindDef OG_Squig_Ork;

        public static PawnKindDef OG_Snotling;

        [MayRequireXenobiologis]
        public static PawnKindDef OG_Ork_Snotling;

        public static PawnKindDef OG_Grot_Wild;
        public static PawnKindDef Tribesperson_OG_Grot;
        public static PawnKindDef Colonist_OG_Grot;

        public static PawnKindDef OG_Ork_Wild;
    //    public static PawnKindDef Tribesperson_OG_Ork;
    //    public static PawnKindDef Colonist_OG_Ork;
    }

    [DefOf]
    public static class OrkFactionDefOf
    {
        static OrkFactionDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OrkFactionDefOf));
        }

        public static FactionDef OG_Ork_PlayerColony;
        public static FactionDef OG_Ork_PlayerTribe;

        public static FactionDef OG_Ork_Rok;
        public static FactionDef OG_Ork_Hulk;

        public static FactionDef OG_Ork_Tek_Faction;
        public static FactionDef OG_Ork_Feral_Faction;
    }

    [DefOf]
    public static class OrkJobDefOf
    {
        static OrkJobDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OrkJobDefOf));
        }

        public static JobDef OGO_Orkoid_Hunt;
        public static JobDef OGO_Orkoid_Scrap;
        //    public static JobDef TakeGrogOutOfOrkFermentingBarrel;
    }
    
    [DefOf]
    public static class OrkMentalStateDefOf
    {
        static OrkMentalStateDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OrkMentalStateDefOf));
        }

        public static MentalStateDef OGO_Ork_Scrapping;
        public static MentalStateDef OGO_Orkoid_Starving;
        //    public static MentalStateDef ;
    }
    [DefOf]
    public static class OrkMentalBreakDefOf
    {
        static OrkMentalBreakDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OrkMentalBreakDefOf));
        }

        public static MentalBreakDef OGO_Orkoid_Starving;
        //    public static MentalBreakDef ;
    }

    [DefOf]
    public static class OrkIncidentDefOf
    {
        static OrkIncidentDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OrkIncidentDefOf));
        }

        public static IncidentDef OG_Ork_Rok_Crash;
    }

    [DefOf]
    public static class OrkNeedDefOf
    {
        static OrkNeedDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OrkNeedDefOf));
        }

        public static NeedDef OG_Ork_Fightyness;
        public static NeedDef Mood;
        public static NeedDef Outdoors;
        public static NeedDef Comfort;
        public static NeedDef Beauty;
    }
}
