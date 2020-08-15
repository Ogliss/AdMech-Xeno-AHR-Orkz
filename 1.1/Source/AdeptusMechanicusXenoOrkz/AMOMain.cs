using System;
using System.Collections.Generic;
using System.Linq;
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
            AlienRace.ThingDef_AlienRace ork = OGOrkThingDefOf.OG_Alien_Ork as AlienRace.ThingDef_AlienRace;
            AlienRace.ThingDef_AlienRace grot = OGOrkThingDefOf.OG_Alien_Grot as AlienRace.ThingDef_AlienRace;
            foreach (ResearchProjectDef def in OrkReseach)
            {
                if (!AlienRace.RaceRestrictionSettings.researchRestrictionDict.ContainsKey(key: def))
                    AlienRace.RaceRestrictionSettings.researchRestrictionDict.Add(key: def, value: new List<AlienRace.ThingDef_AlienRace>());
                AlienRace.RaceRestrictionSettings.researchRestrictionDict[key: def].Add(item: ork);
                AlienRace.RaceRestrictionSettings.researchRestrictionDict[key: def].Add(item: grot);
            }

            HarmonyPatches.TryAddRacialRestrictions(ork, "O");
            HarmonyPatches.TryAddRacialRestrictions(grot, "O");
        }
        
    }
}