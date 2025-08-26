using Verse;

namespace AdeptusMechanicus.ExtensionMethods
{
    public static class OrkThingDefExtensions
    {
        public static bool isOrkoid(this ThingDef pawn)
        {
            return pawn.isOrk() || pawn.isGrot() || pawn.isSquig() || pawn.isSnotling() || pawn.race.BloodDef == AdeptusThingDefOf.OG_FilthBlood_Orkoid;
        }

        public static bool isOrk(this ThingDef pawn)
        {
            return pawn == AdeptusThingDefOf.OG_Alien_Ork;
        }

        public static bool isGrot(this ThingDef pawn)
        {
            return pawn == AdeptusThingDefOf.OG_Alien_Grot;
        }

        public static bool isSquig(this ThingDef pawn)
        {
            return pawn.defName.Contains("OG_") && pawn.defName.Contains("Squig");
        }

        public static bool isSnotling(this ThingDef pawn)
        {
            return pawn.defName.Contains("OG_") && pawn.defName.Contains("Snotling");
        }
        public static bool isOrkoidFungus(this ThingDef def)
        {
            return def == AdeptusThingDefOf.OG_Plant_Orkoid_Fungus || def == AdeptusThingDefOf.OG_Plant_Orkoid_Cocoon;
        }

    }

}
