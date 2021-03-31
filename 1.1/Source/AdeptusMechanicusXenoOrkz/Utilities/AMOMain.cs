using System;
using System.Collections.Generic;
using System.Linq;
using AdeptusMechanicus.ExtensionMethods;
using AdeptusMechanicus.HarmonyInstance;
using AdeptusMechanicus.settings;
using RimWorld;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
    [StaticConstructorOnStartup]
    public class AMOMain
    {
        public static List<ResearchProjectDef> OrkReseach => DefDatabase<ResearchProjectDef>.AllDefs.Where(x=> x.defName.Contains("OG_Ork_Tech_")).ToList();
        static AMOMain()
        {
            AlienRace.ThingDef_AlienRace Ork = OGOrkThingDefOf.OG_Alien_Ork as AlienRace.ThingDef_AlienRace;
            AlienRace.ThingDef_AlienRace Grot = OGOrkThingDefOf.OG_Alien_Grot as AlienRace.ThingDef_AlienRace;
            AlienRace.ThingDef_AlienRace Cybork = DefDatabase<ThingDef>.GetNamedSilentFail("OG_Alien_Cybork") as AlienRace.ThingDef_AlienRace;

            List<ThingDef> races = new List<ThingDef>() { Ork, Grot, OGOrkThingDefOf.OG_Snotling};
            if (Cybork != null) races.Add(Cybork);
            List<ThingDef> animals = DefDatabase<ThingDef>.AllDefsListForReading.Where(x => x.race != null && x.race.Animal && x.isOrkoid()).ToList();
            List<ThingDef> plants = new List<ThingDef>()
            {
                OGOrkThingDefOf.OG_Plant_Orkoid_Fungus,
                OGOrkThingDefOf.OG_Plant_Orkoid_Cocoon
            };
            AlienRaceUtility.DoRacialRestrictionsFor(races, "O", OrkReseach, null, null, plants, animals);
        }
        
    }
}