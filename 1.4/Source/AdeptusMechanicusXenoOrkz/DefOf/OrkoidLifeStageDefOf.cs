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
}
