﻿using AdeptusMechanicus.ExtensionMethods;
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
    // AdeptusMechanicus.OrkoidFungalProps
    public class OrkoidFungalProps : PlantProperties
    {
        public bool canspawn = true;
        public bool spawnwild = true;
        public FloatRange tempsOptimal = new FloatRange(10f, 42f);
        public FloatRange tempsLimits = new FloatRange(0f, 58f);
    }


    // AdeptusMechanicus.OrkoidFungus
    public class OrkoidFungus : Plant
    {
        public OrkoidFungalProps FungalProps => this.def.plant as OrkoidFungalProps;
        public new bool HasEnoughLightToGrow => true;
        public float spawnChance => this.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonSpawnChance : AMMod.Instance.settings.FungusSpawnChance;

        public float squigChance => this.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonSquigChance : AMMod.Instance.settings.FungusSquigChance;
        public float snotlingChance => this.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonSnotChance : AMMod.Instance.settings.FungusSnotChance;
        public float grotChance => this.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonGrotChance : AMMod.Instance.settings.FungusGrotChance;
        public float orkChance => this.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonOrkChance : AMMod.Instance.settings.FungusOrkChance;

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
        public new float GrowthRateFactor_Temperature
        {
            get
            {
                float num;
                if (!GenTemperature.TryGetTemperatureForCell(base.Position, base.Map, out num))
                {
                    return 1f;
                }
                if (num < FungalProps.tempsOptimal.min)
                {
                    return Mathf.InverseLerp(FungalProps.tempsLimits.min, FungalProps.tempsOptimal.min, num);
                }
                if (num > FungalProps.tempsOptimal.max) return Mathf.InverseLerp(FungalProps.tempsLimits.max, FungalProps.tempsOptimal.max, num);
                return 1f;
            }
        }
        /*
        public new float GrowthRateFactor_Temperature
        {
            get
            {
                float num;
                float num2 = (this.def == OGOrkThingDefOf.OG_Plant_Orkoid_Cocoon ? -10f : 0f);
                float num3 = (this.def == OGOrkThingDefOf.OG_Plant_Orkoid_Cocoon ? -50f : -30f);
                if (!GenTemperature.TryGetTemperatureForCell(this.Position, this.Map, out num)) return 1f;
                if (num < num2)
                {
                    if (this.def == OGOrkThingDefOf.OG_Plant_Orkoid_Cocoon) return Mathf.InverseLerp(num3, num2, num);
                    return Mathf.InverseLerp(num3, num2, num);
                }
                if (num > 62f) return Mathf.InverseLerp(116f, 62f, num);
                return 1f;

            }
        }
        */
        public override float GrowthRate
        {
            get
            {
                if (this.Blighted || this.Resting)
                {
                    return 0f;
                }
                /*
                if (base.Spawned && !PlantUtility.GrowthSeasonNow(base.Position, base.Map, false))
                {
                    return 0f;
                }
                */
                return this.GrowthRateFactor_Fertility * this.GrowthRateFactor_Temperature * this.GrowthRateFactor_Light;
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
                return new List<Pair<PawnKindDef, float>>()
                {
                    new Pair<PawnKindDef, float>(OGOrkPawnKindDefOf.OG_Squig, squigChance * (OrkoidFungualUtility.GrotSpawnCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent + Squigs.Count())* Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor)),
                    new Pair<PawnKindDef, float>(OGOrkPawnKindDefOf.OG_Ork_Snotling, snotlingChance * (OrkoidFungualUtility.GrotSpawnCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent + Snots.Count())* Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor)),
                    new Pair<PawnKindDef, float>(OGOrkPawnKindDefOf.OG_Grot_Wild, grotChance * (OrkoidFungualUtility.GrotSpawnCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent + Grots.Count())* Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor)),
                    new Pair<PawnKindDef, float>(OGOrkPawnKindDefOf.OG_Ork_Wild, orkChance * OrkoidFungualUtility.OrkSpawnCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent + Orks.Count())* Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor)
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
        // Token: 0x06005259 RID: 21081 RVA: 0x001BBF68 File Offset: 0x001BA168
        public override void TickLong()
        {
            if (base.Destroyed)
            {
                return;
            }
            if (this.AllComps != null)
            {
                int i = 0;
                int count = this.AllComps.Count;
                while (i < count)
                {
                    this.AllComps[i].CompTickLong();
                    i++;
                }
            }
            float num = this.growthInt;
            bool flag = this.LifeStage == PlantLifeStage.Mature;
            this.growthInt += this.GrowthPerTick * 2000f;
            if (this.growthInt > 1f)
            {
                this.growthInt = 1f;
            }
            if (((!flag && this.LifeStage == PlantLifeStage.Mature) || (int)(num * 10f) != (int)(this.growthInt * 10f)) && this.CurrentlyCultivated())
            {
                base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
            }
            this.unlitTicks = 0;
            this.ageInt += 2000;
            if (this.Dying)
            {
                Map map = base.Map;
                bool isCrop = this.IsCrop;
                bool harvestableNow = this.HarvestableNow;
                bool dyingBecauseExposedToLight = this.DyingBecauseExposedToLight;
                int num2 = Mathf.CeilToInt(this.CurrentDyingDamagePerTick * 2000f);
                base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)num2, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
                if (base.Destroyed)
                {
                    if (isCrop && this.def.plant.Harvestable && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfRot-" + this.def.defName, 240f))
                    {
                        string key;
                        if (harvestableNow)
                        {
                            key = "MessagePlantDiedOfRot_LeftUnharvested";
                        }
                        else if (dyingBecauseExposedToLight)
                        {
                            key = "MessagePlantDiedOfRot_ExposedToLight";
                        }
                        else
                        {
                            key = "MessagePlantDiedOfRot";
                        }
                        Messages.Message(key.Translate(this.GetCustomLabelNoCount(false)), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
                    }
                    return;
                }
            }
            this.cachedLabelMouseover = null;
        }
        // Token: 0x0600525E RID: 21086 RVA: 0x001BC6D4 File Offset: 0x001BA8D4
        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (this.LifeStage == PlantLifeStage.Growing)
            {
                stringBuilder.AppendLine("PercentGrowth".Translate(this.GrowthPercentString));
                stringBuilder.AppendLine("GrowthRate".Translate() + ": " + this.GrowthRate.ToStringPercent());
                if (!this.Blighted)
                {
                    if (this.Resting)
                    {
                        stringBuilder.AppendLine("PlantResting".Translate());
                    }
                    if (!this.HasEnoughLightToGrow)
                    {
                        stringBuilder.AppendLine("PlantNeedsLightLevel".Translate() + ": " + this.def.plant.growMinGlow.ToStringPercent());
                    }
                    float growthRateFactor_Temperature = this.GrowthRateFactor_Temperature;
                    if (growthRateFactor_Temperature < 0.99f)
                    {
                        if (growthRateFactor_Temperature < 0.01f)
                        {
                            stringBuilder.AppendLine("OutOfIdealTemperatureRangeNotGrowing".Translate());
                        }
                        else
                        {
                            stringBuilder.AppendLine("OutOfIdealTemperatureRange".Translate(Mathf.RoundToInt(growthRateFactor_Temperature * 100f).ToString()));
                        }
                    }
                }
            }
            else if (this.LifeStage == PlantLifeStage.Mature)
            {
                if (this.HarvestableNow)
                {
                    stringBuilder.AppendLine("ReadyToHarvest".Translate());
                }
                else
                {
                    stringBuilder.AppendLine("Mature".Translate());
                }
            }
            if (this.DyingBecauseExposedToLight)
            {
                stringBuilder.AppendLine("DyingBecauseExposedToLight".Translate());
            }
            if (this.Blighted)
            {
                stringBuilder.AppendLine("Blighted".Translate() + " (" + this.Blight.Severity.ToStringPercent() + ")");
            }
            string text = base.InspectStringPartsFromComps();
            if (!text.NullOrEmpty())
            {
                stringBuilder.Append(text);
            }
            return stringBuilder.ToString().TrimEndNewlines();
        }
        private string cachedLabelMouseover;
    }
}