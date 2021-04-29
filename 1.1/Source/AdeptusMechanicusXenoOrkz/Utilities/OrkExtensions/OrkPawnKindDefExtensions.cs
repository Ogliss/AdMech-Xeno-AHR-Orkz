using Verse;

namespace AdeptusMechanicus.ExtensionMethods
{
    public static class OrkPawnKindDefExtensions
    {
        public static bool isOrkoid(this PawnKindDef pawn)
        {
            return pawn.isOrk() || pawn.isGrot() || pawn.isSquig() || pawn.isSnotling() || pawn.RaceProps.BloodDef == OrkThingDefOf.OG_FilthBlood_Orkoid;
        }

        public static bool isOrk(this PawnKindDef pawn)
        {
            return pawn.race == OrkThingDefOf.OG_Alien_Ork || pawn.race.defName.Contains("Alien_Ork") || pawn.race.defName.Contains("Cybork");
        }

        public static bool isGrot(this PawnKindDef pawn)
        {
            return pawn.race == OrkThingDefOf.OG_Alien_Grot || pawn.race.defName.Contains("Alien_Grot");
        }

        public static bool isSquig(this PawnKindDef pawn)
        {
            return pawn.race.defName.Contains("OG_") && pawn.race.defName.Contains("Squig");
        }

        public static bool isSnotling(this PawnKindDef pawn)
        {
            return pawn.race.defName.Contains("OG_") && pawn.race.defName.Contains("Snotling");
        }

    }

}
