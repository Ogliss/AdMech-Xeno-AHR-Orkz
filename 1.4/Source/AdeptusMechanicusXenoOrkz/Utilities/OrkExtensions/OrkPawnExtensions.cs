using RimWorld;
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
            return !pawn.def.defName.NullOrEmpty() && pawn.def.defName.StartsWith("OG_") && pawn.def.defName.Contains("Squig") && !pawn.def.defName.Contains("Squiggoth");
        }

        public static bool isSquiggoth(this Pawn pawn)
        {
            return !pawn.def.defName.NullOrEmpty() && pawn.def.defName.StartsWith("OG_") && pawn.def.defName.Contains("Squiggoth");
        }

        public static bool isSnotling(this Pawn pawn)
        {
            return !pawn.def.defName.NullOrEmpty() && pawn.def.defName.StartsWith("OG_") && pawn.def.defName.Contains("Snotling");
        }

        public static Orkoid Orkiness(this Pawn pawn)
        {
            if (pawn.isOrkoid())
            {
                LifeStageDef stage = pawn.ageTracker.CurLifeStage;
                if (pawn.RaceProps.Humanlike)
                {
                    if (pawn.isGrot())
                        return Orkoid.Grot;
                    if (pawn.isOrk())
                    {
                        if (stage.label.Contains("Runt") || stage.label.Contains("Whelp") || stage.label.Contains("Git"))
                            return Orkoid.Runt;
                        if (stage.label.Contains("Boss"))
                            return Orkoid.Warboss;
                        if (stage.label.Contains("Nob"))
                            return Orkoid.Nob;
                        return Orkoid.Ork;
                    }
                }
                else
                {
                    if (pawn.isSnotling())
                        return Orkoid.Snotling;
                    if (pawn.isSquiggoth())
                    {
                        if (stage.label.Contains("gargantuan"))
                            return Orkoid.GargantuanSquiggoth;
                        return Orkoid.Squiggoth;
                    }
                    if (pawn.isSquig())
                    {
                        if (stage.label.Contains("baby") || /*stage.label.Contains("juvenile") ||*/ pawn.def == AdeptusThingDefOf.OG_Squig_Eatin || pawn.def == AdeptusThingDefOf.OG_Squig_Oily)
                            return Orkoid.Squigling;
                        if (stage.label.Contains("big"))
                            return Orkoid.BigSquig;
                        if (stage.label.Contains("massive"))
                            return Orkoid.MassiveSquig;
                        if (stage.label.Contains("colossal"))
                            return Orkoid.ColossalSquig;
                        return Orkoid.Squig;
                    }
                }
            }
            return Orkoid.NonOrkoid;
        }

    }

}
