using AdeptusMechanicus.settings;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.OrkoidFungusExtension
    public class OrkoidFungusExtension : DefModExtension
    {
        public bool canspawn = true;
        public bool spawnwild = true;
    }
    // AdeptusMechanicus.OrkoidFungus
    public class OrkoidFungus : Plant
    {
        public OrkoidFungusExtension FungalProps => this.def.GetModExtension<OrkoidFungusExtension>() ?? new OrkoidFungusExtension();
        public new bool HasEnoughLightToGrow => true;
        public float spawnChance => this.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonSpawnChance : AMMod.Instance.settings.FungusSpawnChance;

        public float squigChance => this.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonSquigChance : AMMod.Instance.settings.FungusSquigChance;
        public float snotlingChance => this.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonSnotChance : AMMod.Instance.settings.FungusSnotChance;
        public float grotChance => this.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonGrotChance : AMMod.Instance.settings.FungusGrotChance;
        public float orkChance => this.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonOrkChance : AMMod.Instance.settings.FungusOrkChance;

        public new float GrowthRateFactor_Temperature
        {
            get
            {
                float num;
                if (!GenTemperature.TryGetTemperatureForCell(base.Position, base.Map, out num))
                {
                    return 1f;
                }
                if (num < -10f)
                {
                    return Mathf.InverseLerp(-50f, -10f, num);
                }
                if (num > 62f)
                {
                    return Mathf.InverseLerp(116f, 62f, num);
                }
                return 1f;
            }
        }

        protected IEnumerable<Pawn> Pawns
        {
            get
            {
                return from p in Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer)
                       where p.RaceProps.Animal
                       select p;
            }
        }
        public List<Pair<PawnKindDef, float>> pairs
        {
            get
            {
                float animalschance = HealthTuning.DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve.Evaluate(Pawns.Count()) * Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor;
                float chance = HealthTuning.DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent) * Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor;
                return new List<Pair<PawnKindDef, float>>()
                {
                    new Pair<PawnKindDef, float>(OGOrkPawnKindDefOf.OG_Squig, squigChance * animalschance),
                    new Pair<PawnKindDef, float>(OGOrkPawnKindDefOf.OG_Ork_Snotling, snotlingChance * animalschance),
                    new Pair<PawnKindDef, float>(OGOrkPawnKindDefOf.OG_Grot_Wild, grotChance * chance),
                    new Pair<PawnKindDef, float>(OGOrkPawnKindDefOf.OG_Ork_Wild, orkChance * chance)
                };
            }
        }
        public override void PlantCollected()
        {
            if (this.HarvestableNow)
            {
                Rand.PushState();
                var spawnRoll = Rand.Value;
                Rand.PopState();
                if (spawnRoll < (spawnChance * this.Growth))
                {
                    PawnKindDef pawnKindDef = pairs.RandomElementByWeight(x => x.Second).First;
                    Faction faction = FungalProps.spawnwild ? null : Faction.OfPlayer;
                    PawnGenerationRequest pawnGenerationRequest = new PawnGenerationRequest(pawnKindDef, faction, PawnGenerationContext.NonPlayer, -1, true, true, false, false, true, true, 0f, fixedGender: Gender.None, fixedBiologicalAge: Age, fixedChronologicalAge: Age);
                    Log.Message("Spawning "+ pawnKindDef);
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
                        if (!FungalProps.spawnwild && (Faction.OfPlayer.def == OGOrkFactionDefOf.OG_Ork_PlayerTribe || Faction.OfPlayer.def == OGOrkFactionDefOf.OG_Ork_PlayerColony))
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
                    if (GrowthRateFactor_Fertility < 1f)
                    {
                        foreach (Need need in pawn.needs.AllNeeds)
                        {
                            need.CurLevel = 0f;
                        }
                        Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.Malnutrition, pawn);
                        hediff.Severity = Math.Min(1f - GrowthRateFactor_Fertility, 0.75f);
                        pawn.health.AddHediff(hediff);
                    }
                    else
                    {
                        foreach (Need need in pawn.needs.AllNeeds)
                        {
                            need.CurLevel = GrowthRateFactor_Fertility - 1f;
                        }
                    }
                    GenSpawn.Spawn(pawn, this.Position, this.Map, 0);
                }
                base.PlantCollected();
            }

        }
    }
}