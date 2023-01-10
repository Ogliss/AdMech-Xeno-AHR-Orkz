using RimWorld;
using System;
using Verse;

namespace AdeptusMechanicus
{
    [DefOf]
    public static class OrkoidLifeStageDefOf
    {
        static OrkoidLifeStageDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OrkoidLifeStageDefOf));
        }

        public static LifeStageDef OG_Lifestage_Ork_Whelp;
        public static LifeStageDef OG_Lifestage_Ork_Runt;
        public static LifeStageDef OG_Lifestage_Ork_Adult;
        public static LifeStageDef OG_Lifestage_Ork_Nob;
        public static LifeStageDef OG_Lifestage_Ork_BigNob;
        public static LifeStageDef OG_Lifestage_Ork_Boss;
        public static LifeStageDef OG_Lifestage_Ork_WarBoss;

        public static LifeStageDef OG_Lifestage_Grot_Whelp;
        public static LifeStageDef OG_Lifestage_Grot_Runt;
        public static LifeStageDef OG_Lifestage_Grot_Adult;
    }

    [DefOf]
    public static class OrkoidThoughtDefOf
    {
        static OrkoidThoughtDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OrkoidThoughtDefOf));
        }

        public static ThoughtDef OG_Ork_LostScrap;
        public static ThoughtDef OG_Ork_WonScrap;
        public static ThoughtDef OG_Ork_WonScrap_Downed;
        public static ThoughtDef OG_Ork_WonScrap_Killed;
        
    }
}
