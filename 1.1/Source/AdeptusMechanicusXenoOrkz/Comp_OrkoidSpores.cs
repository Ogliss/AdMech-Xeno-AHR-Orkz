using RimWorld;
using Verse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlienRace;
using AdeptusMechanicus.settings;
using AdeptusMechanicus.ExtensionMethods;

namespace AdeptusMechanicus
{

    public class CompProperties_OrkoidSpores : CompProperties
    {
        public CompProperties_OrkoidSpores()
        {
            this.compClass = typeof(Comp_OrkoidSpores);
        }

        public bool canspawn = true;
        public bool spawnwild = true;
        public float spawnChance = 0.075f;
        public float snotlingChance = 0.035f;
        public float grotChance = 0.025f;
        public float orkChance = 0.015f;
    }

    public class Comp_OrkoidSpores : ThingComp
    {
        public CompProperties_OrkoidSpores Props
        {
            get
            {
                return (CompProperties_OrkoidSpores)this.props;
            }
        }

        public Plant plant => base.parent as Plant;

        public bool canspawn => plant.HarvestableNow && Props.canspawn;

        public bool spawnwild => Props.spawnwild;
        public float spawnChance => parent.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonSpawnChance : AMMod.Instance.settings.FungusSpawnChance;

        public float squigChance => parent.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonSquigChance : AMMod.Instance.settings.FungusSquigChance;
        public float snotlingChance => parent.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonSnotChance : AMMod.Instance.settings.FungusSnotChance;
        public float grotChance => parent.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonGrotChance : AMMod.Instance.settings.FungusGrotChance;
        public float orkChance => parent.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonOrkChance : AMMod.Instance.settings.FungusOrkChance;

        private float age = 0;
        public float Age
        {
            get
            {
                if (plant != null)
                {
                    age = plant.Age;
                }
                return age;
            }
        }
        private float fertility = 0;
        public float Fertility
        {
            get
            {
                if (plant != null)
                {
                    if (plant.Map!=null)
                    {
                        fertility = plant.GrowthRateFactor_Fertility;
                    }
                }
                return fertility;
            }
        }

        protected IEnumerable<Pawn> Squigs
        {
            get
            {
                return from p in Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer)
                       where p.isSquig()
                       select p;
            }
        }
        protected IEnumerable<Pawn> Snots
        {
            get
            {
                return from p in Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer)
                       where p.isSnotling()
                       select p;
            }
        }
        protected IEnumerable<Pawn> Grots
        {
            get
            {
                return from p in Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer)
                       where p.isGrot()
                       select p;
            }
        }
        protected IEnumerable<Pawn> Orks
        {
            get
            {
                return from p in Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer)
                       where p.isOrk()
                       select p;
            }
        }
        public List<Pair<PawnKindDef, float>> pairs
        {
            get
            {
                return new List<Pair<PawnKindDef, float>>()
                {
                    new Pair<PawnKindDef, float>(OGOrkPawnKindDefOf.OG_Squig, squigChance * (OrkoidFungualUtility.GrotSpawnCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent + Squigs.Count())* Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor)),
                    new Pair<PawnKindDef, float>(OGOrkPawnKindDefOf.OG_Ork_Snotling, snotlingChance * (OrkoidFungualUtility.GrotSpawnCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent + Snots.Count())* Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor)),
                    new Pair<PawnKindDef, float>(OGOrkPawnKindDefOf.OG_Grot_Wild, grotChance * (OrkoidFungualUtility.GrotSpawnCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent + Grots.Count())* Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor)),
                    new Pair<PawnKindDef, float>(OGOrkPawnKindDefOf.OG_Ork_Wild, orkChance * OrkoidFungualUtility.OrkSpawnCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent + Orks.Count())* Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor)
                };
            }
        }

        public void SpawnPawns(Pawn harvester, Plant plant)
        {

            if (canspawn)
            {
                Rand.PushState();
                var spawnRoll = Rand.Value;
                Rand.PopState();
                if (spawnRoll < (spawnChance * plant.Growth))
                {
                    string msg = string.Empty;
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine("Possible Spawns:");
                    foreach (var item in pairs)
                    {
                        builder.Append(" " + item.First.LabelCap + " weighted at " + item.Second);
                    }

                    Pair<PawnKindDef, float> pair = pairs.RandomElementByWeight(x => x.Second);
                    pawnKindDef = pair.First;
                    builder.Append(" " + "Spawning " + pawnKindDef.LabelCap);
                    Log.Message(builder.ToString());
                    faction = spawnwild ? null : Faction.OfPlayer;
                    PawnGenerationRequest pawnGenerationRequest = new PawnGenerationRequest(pawnKindDef, faction, PawnGenerationContext.NonPlayer, -1, true, true, false, false, true, true, 0f, fixedGender: Gender.None, fixedBiologicalAge: Age, fixedChronologicalAge: Age);

                    Pawn pawn = PawnGenerator.GeneratePawn(pawnGenerationRequest);

                    if (pawnKindDef.RaceProps.Humanlike)
                    {
                        /*
                        if (pawn.kindDef == OGOrkPawnKindDefOf.OG_Ork_Wild)
                        {
                            pawn.story.childhood.identifier = "Ork_Base_Child";
                        }
                        else if (pawn.kindDef == OGOrkPawnKindDefOf.OG_Grot_Wild)
                        {
                            pawn.story.childhood.identifier = "Grot_Base_Child";
                        }
                        */
                        if (!spawnwild && (Faction.OfPlayer.def == OGOrkFactionDefOf.OG_Ork_PlayerTribe || Faction.OfPlayer.def == OGOrkFactionDefOf.OG_Ork_PlayerColony))
                        {
                            PawnKindDef pawnKind;
                            if (Faction.OfPlayer.def == OGOrkFactionDefOf.OG_Ork_PlayerTribe)
                            {
                                pawnKind = pawn.def.defName.Contains("Alien_Grot") ? OGOrkPawnKindDefOf.Tribesperson_OG_Grot : DefDatabase<PawnKindDef>.AllDefsListForReading.Where(x => x.defName.Contains("Tribesperson_OG_Ork")).RandomElement();
                            }
                            else
                            {
                                pawnKind = pawn.def.defName.Contains("Alien_Grot") ? OGOrkPawnKindDefOf.Colonist_OG_Grot : DefDatabase<PawnKindDef>.AllDefsListForReading.Where(x => x.defName.Contains("Colonist_OG_Ork")).RandomElement();
                            }
                            pawn.ChangeKind(pawnKind);
                        }
                        else
                        {
                            pawn.ChangeKind(PawnKindDefOf.WildMan);
                        }
                        pawn.story.bodyType = pawn.story.childhood.BodyTypeFor(pawn.gender);
                    }
                    if (Fertility < 1f)
                    {
                        foreach (Need need in pawn.needs.AllNeeds)
                        {
                            need.CurLevel = 0f;
                        }
                        Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.Malnutrition, pawn);
                        hediff.Severity = Math.Min(1f - Fertility, 0.75f);
                        pawn.health.AddHediff(hediff);
                    }
                    else
                    {
                        foreach (Need need in pawn.needs.AllNeeds)
                        {
                            need.CurLevel = Fertility - 1f;
                        }
                    }
                    GenSpawn.Spawn(pawn, base.parent.Position, base.parent.Map, 0);
                }
            }
        }

        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);
        }

        public PawnKindDef pawnKindDef;

        public Faction faction;

        public PawnGenerationContext generationContext;

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref this.age, "PlantAge");
            Scribe_Values.Look(ref this.fertility, "PlantFertility");
        }
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            if (age == 0)
            {
                age = Age;
            }
            if (fertility == 0)
            {
                fertility = Fertility;
            }
        }
    }
 
}
