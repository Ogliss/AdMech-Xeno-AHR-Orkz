using Verse;
using Verse.AI;

namespace AdeptusMechanicus.ExtensionMethods
{
    public static class OrkPawnExtensions
    {
        public static bool isOrkoid(this Pawn pawn)
        {
            return pawn.isOrk() || pawn.isGrot() || pawn.isSquig() || pawn.isSnotling() || pawn.RaceProps.BloodDef == AdeptusThingDefOf.OG_FilthBlood_Orkoid;
        }

        public static bool isOrk(this Pawn pawn)
        {
            return pawn.def == AdeptusThingDefOf.OG_Alien_Ork;
        }

        public static bool isGrot(this Pawn pawn)
        {
            return pawn.def == AdeptusThingDefOf.OG_Alien_Grot;
        }

        public static bool isSquig(this Pawn pawn)
        {
            return pawn.def.defName.Contains("OG_") && pawn.def.defName.Contains("Squig");
        }

        public static bool isSnotling(this Pawn pawn)
        {
            return pawn.def.defName.Contains("OG_") && pawn.def.defName.Contains("Snotling");
        }

        public static Orkoid Orkiness(this Pawn pawn)
        {
            if (pawn.isOrkoid())
            {
                if (pawn.def.defName.Contains("Gargantuan"))
                {
                    return Orkoid.GargantuanSquiggoth;
                }
                else
                if (pawn.def.defName.Contains("Squiggoth"))
                {
                    return Orkoid.Squiggoth;
                }
                else
                if (pawn.isOrk())
                {
                    if (pawn.kindDef.defName.Contains("Warboss"))
                    {
                        return Orkoid.Warboss;
                    }
                    else
                    if (pawn.kindDef.defName.Contains("Nob"))
                    {
                        return Orkoid.Nob;
                    }
                    else
                    if (pawn.kindDef.defName.Contains("Runt"))
                    {
                        return Orkoid.Runt;
                    }
                    else
                        return Orkoid.Ork;
                }
                else
                if (pawn.isGrot())
                {
                    return Orkoid.Grot;
                }
                else
                if (pawn.isSnotling())
                {
                    return Orkoid.Snotling;
                }
                else
                if (pawn.isSquig())
                {
                    return Orkoid.Squig;
                }
            }
            return Orkoid.NonOrkoid;
        }

    }

}
