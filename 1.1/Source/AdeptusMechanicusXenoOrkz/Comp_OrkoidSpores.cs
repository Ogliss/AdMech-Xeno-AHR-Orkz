﻿using RimWorld;
using Verse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlienRace;

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

        public Plant plant
        {
            get
            {
                return base.parent as Plant;
            }
        }

        public bool canspawn
        {
            get
            {
                return plant.HarvestableNow && Props.canspawn;
            }
        }

        public bool spawnwild
        {
            get
            {
                return Props.spawnwild;
            }
        }

        public float spawnChance
        {
            get
            {
                return Props.spawnChance;
            }
        }

        public float snotlingChance
        {
            get
            {
                return Props.snotlingChance;
            }
        }

        public float grotChance
        {
            get
            {
                return Props.grotChance;
            }
        }

        public float orkChance
        {
            get
            {
                return Props.orkChance;
            }
        }

        public float age
        {
            get
            {
                 return 0f;
            }
            set
            {

            }
        }

        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);
            if (canspawn == true)
            {
                var spawnRoll = Rand.Value;
                if (spawnRoll < (spawnChance*plant.Growth))
                {
                    spawnRoll = Rand.Value;
                    if (spawnRoll < snotlingChance & spawnRoll > grotChance)
                    {
                        pawnKindDef = OGOrkPawnKindDefOf.Snotling;
                    }
                    else if (spawnRoll < grotChance & spawnRoll > orkChance)
                    {
                        pawnKindDef = OGOrkPawnKindDefOf.WildGrot;
                    }
                    else if (spawnRoll < orkChance)
                    {
                        pawnKindDef = OGOrkPawnKindDefOf.WildOrk;
                    }
                    else
                    {
                        pawnKindDef = OGOrkPawnKindDefOf.Squig;
                    }
                    if (spawnwild)
                    {
                        faction = null;
                        generationContext = PawnGenerationContext.NonPlayer;
                    }
                    else
                    {
                        faction = Faction.OfPlayer;
                        generationContext = PawnGenerationContext.PlayerStarter;
                    }
                    if (pawnKindDef.RaceProps.Humanlike)
                    {
                    }
                    PawnGenerationRequest pawnGenerationRequest = new PawnGenerationRequest(pawnKindDef, faction, generationContext, -1, true, false, false, false, true, true, 20f, fixedGender: Gender.Male, fixedBiologicalAge: age, fixedChronologicalAge: age);
                    Pawn pawn = PawnGenerator.GeneratePawn(pawnGenerationRequest);
                    
                    if (pawn.kindDef==OGOrkPawnKindDefOf.WildOrk)
                    {
                        pawn.story.childhood.identifier = "Ork_Base_Child";
                    }
                    else if (pawn.kindDef==OGOrkPawnKindDefOf.WildGrot)
                    {
                        pawn.story.childhood.identifier = "Grot_Base_Child";
                    }
                    if (spawnwild && pawnKindDef != OGOrkPawnKindDefOf.Snotling && pawnKindDef != OGOrkPawnKindDefOf.Squig)
                    {
                            pawn.ChangeKind(PawnKindDefOf.WildMan);
                    }
                    else if (!spawnwild && Faction.OfPlayer.def == OGOrkFactionDefOf.OrkPlayerColonyTribal && pawnKindDef != OGOrkPawnKindDefOf.Snotling && pawnKindDef != OGOrkPawnKindDefOf.Squig)
                    {
                        pawn.ChangeKind(PawnKindDefOf.Colonist);
                    }
                    GenSpawn.Spawn(pawn, base.parent.Position, map, 0);
                }
            }
        }

        public PawnKindDef pawnKindDef;

        public Faction faction;

        public PawnGenerationContext generationContext;
    }
 
}