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
            List<string> blackTags = new List<string>() { "I", "C" };
            List<ResearchProjectDef> blackProjects = new List<ResearchProjectDef>();
            blackProjects.AddRange(ArmouryMain.ReseachImperial);
            blackProjects.AddRange(ArmouryMain.ReseachChaos);

            List<ResearchProjectDef> whiteProjects = new List<ResearchProjectDef>(OrkReseach);

            AlienRace.ThingDef_AlienRace Cybork = DefDatabase<ThingDef>.GetNamedSilentFail("OG_Alien_Cybork") as AlienRace.ThingDef_AlienRace;

            List<ThingDef> races = new List<ThingDef>() { AdeptusThingDefOf.OG_Alien_Ork, AdeptusThingDefOf.OG_Alien_Grot, AdeptusThingDefOf.OG_Snotling};
            if (Cybork != null) races.Add(Cybork);
            List<ThingDef> animals = DefDatabase<ThingDef>.AllDefsListForReading.Where(x => x.race != null && x.race.Animal && x.isOrkoid()).ToList();
            List<ThingDef> plants = new List<ThingDef>()
            {
                AdeptusThingDefOf.OG_Plant_Orkoid_Fungus,
                AdeptusThingDefOf.OG_Plant_Orkoid_Cocoon
            };
            AlienRaceUtility.DoRacialRestrictionsFor(races, "O", blackTags, whiteProjects, blackProjects, whitePlants: plants, whiteAnimals: animals);
        }
        
    }
}