using AdeptusMechanicus.ExtensionMethods;
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
        public float optionalChance = 0.01f;
        public List<OptionalThings> optionals = new List<OptionalThings>();
        
        public struct OptionalThings
        {
            public ThingDef def;
            public float weight;
        }
    }


    // AdeptusMechanicus.OrkoidFungus
    public class OrkoidFungus : Plant
    {
        public OrkoidFungalProps FungalProps => this.def.plant as OrkoidFungalProps;
        public new bool HasEnoughLightToGrow => true;
        public float SpawnChance => this.def.defName.Contains("Cocoon") ? AMAMod.settings.CocoonSpawnChance : AMAMod.settings.FungusSpawnChance;

        public float SquigChance => this.def.defName.Contains("Cocoon") ? AMAMod.settings.CocoonSquigChance : AMAMod.settings.FungusSquigChance;
        public float SnotlingChance => this.def.defName.Contains("Cocoon") ? AMAMod.settings.CocoonSnotChance : AMAMod.settings.FungusSnotChance;
        public float GrotChance => this.def.defName.Contains("Cocoon") ? AMAMod.settings.CocoonGrotChance : AMAMod.settings.FungusGrotChance;
        public float OrkChance => this.def.defName.Contains("Cocoon") ? AMAMod.settings.CocoonOrkChance : AMAMod.settings.FungusOrkChance;

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
        public override bool DyingBecauseExposedToLight
        {
            get
            {
                return this.def.plant.cavePlant && base.Spawned && base.Map.glowGrid.GameGlowAt(base.Position, true) > 0f;
            }
        }
        public new float GrowthRateFactor_Light
        {
            get
            {
                float num = base.Map.glowGrid.GameGlowAt(base.Position, false);
                if (num >= this.def.plant.growMinGlow && num <= this.def.plant.growOptimalGlow  )
                {
                    return 1f;
                }
                if (num > this.def.plant.growOptimalGlow)
                {
                    return GenMath.InverseLerp(this.def.plant.growOptimalGlow, this.def.plant.growOptimalGlow  * 2, num);
                }
                return GenMath.InverseLerp(this.def.plant.growMinGlow, this.def.plant.growOptimalGlow, num);
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
        public static float Inverse(float val) => 1f / val;
        public virtual bool TrySpawnPawns(Pawn harvester = null)
        {
            if (this.HarvestableNow)
            {
                int squigs = Squigs.Count();
                int snots = Snots.Count();
                int grots = Grots.Count();
                int orks = Orks.Count();
                int greenskins = squigs + snots + grots + orks;
                Rand.PushState(this.thingIDNumber);
                var spawnRoll = Rand.ValueSeeded(this.thingIDNumber);
                Rand.PopState();
                if (spawnRoll < (SpawnChance * (/*HealthTuning.DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent + greenskins) **/ Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor) * this.Growth))
                {
                    PawnKindDef pawnKindDef;
                    bool OrkoidHarvester = harvester == null || harvester.isOrkoid();
                    if (OrkoidHarvester)
                    {
                        List<PawnGenOption> options = new List<PawnGenOption>()
                        {
                            new PawnGenOption(OGOrkPawnKindDefOf.OG_Squig, SquigChance * (OrkoidFungualUtility.SquigSpawnCurve.Evaluate(squigs))),
                            new PawnGenOption(OGOrkPawnKindDefOf.OG_Snotling, SnotlingChance * (OrkoidFungualUtility.SnotSpawnCurve.Evaluate(snots))),
                            new PawnGenOption(OGOrkPawnKindDefOf.OG_Grot_Wild, GrotChance * ((this.ageInt/this.def.plant.LifespanTicks) + OrkoidFungualUtility.GrotSpawnCurve.Evaluate(grots))),
                            new PawnGenOption(OGOrkPawnKindDefOf.OG_Ork_Wild, OrkChance *((this.ageInt/this.def.plant.LifespanTicks) + OrkoidFungualUtility.OrkSpawnCurve.Evaluate(orks)))
                        };
                        pawnKindDef = options.InRandomOrder().RandomElementByWeight(x => x.selectionWeight).kind;
                        Rand.PushState();
                        if (pawnKindDef == OGOrkPawnKindDefOf.OG_Squig)
                        {
                            Rand.PushState();
                            pawnKindDef = DefDatabase<PawnKindDef>.AllDefsListForReading.Where(x => x.RaceProps.Animal && x.defName.Contains("OG_") && x.defName.Contains("_Squig")).RandomElementByWeight(x => Inverse(x.race.BaseMarketValue));
                            Rand.PopState();
                        }
                        Rand.PopState(); 
                        StringBuilder builder = new StringBuilder();
                        builder.Append(pawnKindDef.LabelCap + " Spawned");
                        builder.AppendLine();

                        foreach (var item in options)
                        {
                            builder.Append(" " + item.kind.LabelCap + " weighted at " + item.selectionWeight);
                        }
                        if (Prefs.DevMode) Log.Message(builder.ToString());

                    }
                    else
                    {
                        pawnKindDef = OGOrkPawnKindDefOf.OG_Squig;
                    }
                    Faction faction = FungalProps.spawnwild || !OrkoidHarvester ? null : Faction.OfPlayer;
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
                        if (!FungalProps.spawnwild && (Faction.OfPlayer.def == OGOrkFactionDefOf.OG_Ork_PlayerTribe || Faction.OfPlayer.def == OGOrkFactionDefOf.OG_Ork_PlayerColony))
                        {
                            PawnKindDef pawnKind;
                            if (Faction.OfPlayer.def == OGOrkFactionDefOf.OG_Ork_PlayerTribe)
                            {
                                pawnKind = pawn.def.defName.Contains("Alien_Grot") ? OGOrkPawnKindDefOf.Tribesperson_OG_Grot : DefDatabase<PawnKindDef>.AllDefsListForReading.Where(x => x.defName.Contains("Tribesperson_OG_Ork")).RandomElementByWeight(x=> Inverse(x.combatPower));
                            }
                            else
                            {
                                pawnKind = pawn.def.defName.Contains("Alien_Grot") ? OGOrkPawnKindDefOf.Colonist_OG_Grot : DefDatabase<PawnKindDef>.AllDefsListForReading.Where(x => x.defName.Contains("Colonist_OG_Ork")).RandomElementByWeight(x => Inverse(x.combatPower));
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

                            if (need.def != NeedDefOf.Rest)
                            {
                                need.CurLevel = 0.1f;
                            }
                        }
                    }
                    else
                    {
                        float level = GrowthRateFactor_Fertility - 1f;
                        pawn.needs.food.CurLevel = level;
                        pawn.needs.rest.CurLevel = 1f;
                        if (pawn.RaceProps.Humanlike)
                        {
                            pawn.needs.mood.CurLevel = level;
                        }
                    }
                    Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.Malnutrition, pawn);
                    hediff.Severity = Math.Min(1f - GrowthRateFactor_Fertility, 0.8f);
                    pawn.health.AddHediff(hediff);
                    GenSpawn.Spawn(pawn, this.Position, this.Map, 0);
                    if (harvester!=null && harvester.Faction == Faction.OfPlayer)
                    {
                        TaggedString taggedString = "OGOrk_FungalHarvest_SuccessMessage".Translate(harvester.Name, pawn.Label);
                        Messages.Message(taggedString, pawn, MessageTypeDefOf.PositiveEvent, true);
                    }
                    return true;
                }
            }
            return false;
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
                            this.TrySpawnPawns();
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