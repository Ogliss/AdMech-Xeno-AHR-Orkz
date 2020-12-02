using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000945 RID: 2373
	[DefOf]
	public static class OGOrkThingDefOf
	{
		// Token: 0x06003770 RID: 14192 RVA: 0x001A8272 File Offset: 0x001A6672
		static OGOrkThingDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(OGOrkThingDefOf));
		}

        // Token: 0x04001EE3 RID: 7907
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
        public static ThingDef OG_Alien_Ork;
        public static ThingDef OG_Alien_Grot;
        

        public static ThingDef OG_Snotling;
        public static ThingDef OG_Squig;
        public static ThingDef OG_Squig_Ork;
    }

    [DefOf]
    public static class OGOrkPawnKindDefOf
    {
        // Token: 0x0600377C RID: 14204 RVA: 0x001A83CC File Offset: 0x001A67CC
        static OGOrkPawnKindDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OGOrkPawnKindDefOf));
        }
        public class Player
        {
            public static PawnKindDef OG_Squig;
        }
        public static PawnKindDef OG_Squig;

        public static PawnKindDef OG_Squig_Ork;

        public static PawnKindDef OG_Ork_Snotling;

        public static PawnKindDef OG_Grot_Wild;
        public static PawnKindDef Tribesperson_OG_Grot;
        public static PawnKindDef Colonist_OG_Grot;

        public static PawnKindDef OG_Ork_Wild;
    //    public static PawnKindDef Tribesperson_OG_Ork;
    //    public static PawnKindDef Colonist_OG_Ork;
    }

    [DefOf]
    public static class OGOrkFactionDefOf
    {
        // Token: 0x06003770 RID: 14192 RVA: 0x001A8272 File Offset: 0x001A6672
        static OGOrkFactionDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OGOrkFactionDefOf));
        }

        public static FactionDef OG_Ork_PlayerColony;
        public static FactionDef OG_Ork_PlayerTribe;

        public static FactionDef OG_Ork_Rok;
        public static FactionDef OG_Ork_Hulk;

        public static FactionDef OG_Ork_Tek_Faction;
        public static FactionDef OG_Ork_Feral_Faction;
    }

    [DefOf]
    public static class OGOrkJobDefOf
    {
        // Token: 0x06003770 RID: 14192 RVA: 0x001A8272 File Offset: 0x001A6672
        static OGOrkJobDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OGOrkJobDefOf));
        }

        //    public static JobDef TakeGrogOutOfOrkFermentingBarrel;
    }
    [DefOf]
    public static class OGOrkIncidentDefOf
    {
        // Token: 0x06003770 RID: 14192 RVA: 0x001A8272 File Offset: 0x001A6672
        static OGOrkIncidentDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OGOrkIncidentDefOf));
        }

        public static IncidentDef OG_Ork_Rok_Crash;
    }
}
