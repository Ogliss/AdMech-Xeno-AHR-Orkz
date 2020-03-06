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
        public static ThingDef OG_Ork_FermentingBarrel;

        // Item Defs
        public static ThingDef OG_Ork_Waart;
        public static ThingDef OG_Ork_Grog;

        // Backstory Defs
        public static AlienRace.BackstoryDef Ork_Base_Child;
        public static AlienRace.BackstoryDef Grot_Base_Child;

        // Humanlike Race Defs
        public static AlienRace.ThingDef_AlienRace OG_Alien_Ork;
        public static AlienRace.ThingDef_AlienRace OG_Cyborg_Ork;
        public static AlienRace.ThingDef_AlienRace OG_Alien_Grot;
        

        public static ThingDef OG_Ork_Snotling;
        public static ThingDef OG_Squig;
        public static ThingDef OG_Squig_Ork;
    }

    public static class OGOrkPawnKindDefOf
    {
        // Token: 0x0600377C RID: 14204 RVA: 0x001A83CC File Offset: 0x001A67CC
        static OGOrkPawnKindDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OGOrkPawnKindDefOf));
        }
        public class Player
        {
            public static PawnKindDef Squig;
        }
        public static PawnKindDef Squig;

        public static PawnKindDef Snotling;

        public static PawnKindDef WildGrot;

        public static PawnKindDef WildOrk;
    }

    public static class OGOrkFactionDefOf
    {
        // Token: 0x06003770 RID: 14192 RVA: 0x001A8272 File Offset: 0x001A6672
        static OGOrkFactionDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OGOrkFactionDefOf));
        }

        public static FactionDef OrkPlayerColony;
        public static FactionDef OrkPlayerColonyTribal;

        public static FactionDef RokOrkz;
        public static FactionDef HulkOrkz;

        public static FactionDef OrkFaction;
        public static FactionDef FeralOrkFaction;
    }

    public static class OGOrkJobDefOf
    {
        // Token: 0x06003770 RID: 14192 RVA: 0x001A8272 File Offset: 0x001A6672
        static OGOrkJobDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OGOrkFactionDefOf));
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
