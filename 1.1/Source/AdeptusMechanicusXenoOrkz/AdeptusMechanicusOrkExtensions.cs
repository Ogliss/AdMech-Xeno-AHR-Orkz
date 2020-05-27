using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace AdeptusMechanicus.ExtensionMethods
{

    public static class AdeptusMechanicusOrkExtensions
    {
        public static bool isOrkoid(this PawnKindDef pawn)
        {
            return pawn.isOrk() || pawn.isGrot() || pawn.isSquig() || pawn.isSnotling() || pawn.RaceProps.BloodDef == OGOrkThingDefOf.OG_FilthBlood_Orkoid;
        }

        public static bool isOrk(this PawnKindDef pawn)
        {
            return pawn.race == OGOrkThingDefOf.OG_Alien_Ork || pawn.race.defName.Contains("Alien_Ork") || pawn.race.defName.Contains("Cybork");
        }

        public static bool isGrot(this PawnKindDef pawn)
        {
            return pawn.race == OGOrkThingDefOf.OG_Alien_Grot || pawn.race.defName.Contains("Alien_Grot");
        }

        public static bool isSquig(this PawnKindDef pawn)
        {
            return pawn.race.defName.Contains("OG_") && pawn.race.defName.Contains("Squig");
        }

        public static bool isSnotling(this PawnKindDef pawn)
        {
            return pawn.race.defName.Contains("OG_") && pawn.race.defName.Contains("Snotling");
        }

        public static bool isOrkoid(this Pawn pawn)
        {
            return pawn.isOrk() || pawn.isGrot() || pawn.isSquig() || pawn.isSnotling() || pawn.RaceProps.BloodDef == OGOrkThingDefOf.OG_FilthBlood_Orkoid;
        }

        public static bool isOrk(this Pawn pawn)
        {
            return pawn.def == OGOrkThingDefOf.OG_Alien_Ork;
        }

        public static bool isGrot(this Pawn pawn)
        {
            return pawn.def == OGOrkThingDefOf.OG_Alien_Grot;
        }

        public static bool isSquig(this Pawn pawn)
        {
            return pawn.def.defName.Contains("OG_") && pawn.def.defName.Contains("Squig");
        }

        public static bool isSnotling(this Pawn pawn)
        {
            return pawn.def.defName.Contains("OG_") && pawn.def.defName.Contains("Snotling");
        }

        public static bool isOrkoid(this ThingDef pawn)
        {
            return pawn.isOrk() || pawn.isGrot() || pawn.isSquig() || pawn.isSnotling() || pawn.race.BloodDef == OGOrkThingDefOf.OG_FilthBlood_Orkoid;
        }

        public static bool isOrk(this ThingDef pawn)
        {
            return pawn == OGOrkThingDefOf.OG_Alien_Ork;
        }

        public static bool isGrot(this ThingDef pawn)
        {
            return pawn == OGOrkThingDefOf.OG_Alien_Grot;
        }

        public static bool isSquig(this ThingDef pawn)
        {
            return pawn.defName.Contains("OG_") && pawn.defName.Contains("Squig");
        }

        public static bool isSnotling(this ThingDef pawn)
        {
            return pawn.defName.Contains("OG_") && pawn.defName.Contains("Snotling");
        }

    }

}
