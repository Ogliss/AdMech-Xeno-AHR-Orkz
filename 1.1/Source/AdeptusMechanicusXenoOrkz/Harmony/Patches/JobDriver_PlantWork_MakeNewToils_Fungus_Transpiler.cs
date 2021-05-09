using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using HarmonyLib;
using Verse.Sound;
using AdeptusMechanicus;
using AdeptusMechanicus.ExtensionMethods;
using UnityEngine;
using System.Reflection.Emit;
using AdeptusMechanicus.settings;

namespace AdeptusMechanicus.HarmonyInstance
{
    
 //   [HarmonyPatch(typeof(JobDriver_PlantWork), "MakeNewToils")]
    public static class JobDriver_PlantWork_MakeNewToils_Fungus_Transpiler
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var instructionsList = new List<CodeInstruction>(instructions);

            for (int i = 0; i < instructionsList.Count; i++)
            {
                CodeInstruction instruction = instructionsList[i];
                //    Log.Message(i + " opcode: " + instruction.opcode + " operand: " + instruction.operand);

                
                if (i > 1 && i < instructionsList.Count)
                {
                    /*
                    */

                    if (instructionsList[index: i].OperandIs(AccessTools.Method(type: typeof(ThingMaker), name: nameof(ThingMaker.MakeThing), parameters: new[] { typeof(ThingDef), typeof(ThingDef) })))
                    {
                    //    if (Prefs.DevMode) Log.Message((i) + " opcode: " + instruction.opcode + " operand: " + instruction.operand);

                        yield return new CodeInstruction(opcode: OpCodes.Ldloc_0);             // Pawn
                        yield return new CodeInstruction(opcode: OpCodes.Ldloc_2);             // Plant
                        instruction = new CodeInstruction(opcode: OpCodes.Call, operand: typeof(JobDriver_PlantWork_MakeNewToils_Fungus_Transpiler).GetMethod("FungusHarvest"));
                    }
                    if (instructionsList[index: i].OperandIs(AccessTools.Method(type: typeof(GenPlace), name: nameof(GenPlace.TryPlaceThing), parameters: new[] { typeof(Thing), typeof(IntVec3), typeof(Map), typeof(ThingPlaceMode), typeof(Action<Thing, int>), typeof(Predicate<IntVec3>), typeof(Rot4) })))
                    {
                        yield return new CodeInstruction(opcode: OpCodes.Ldloc_0);             // Pawn
                        yield return new CodeInstruction(opcode: OpCodes.Ldloc_2);             // Plant
                        instruction = new CodeInstruction(opcode: OpCodes.Call, operand: typeof(JobDriver_PlantWork_MakeNewToils_Fungus_Transpiler).GetMethod("FungusHarvested"));
                    }
                }
                
                yield return instruction;
                /*
                if (i > 1 && i + 1 < instructionsList.Count)
                {
                    if (instructionsList[index: i - 1].Calls(AccessTools.Method(type: typeof(GenPlace), name: nameof(GenPlace.TryPlaceThing))))
                    {

                        Log.Message("Boss, wez found it!");
                        Log.Message(i + " opcode: " + instruction.opcode + " operand: " + instruction.operand);
                    }
                    if (instructionsList[index: i - 1].OperandIs(AccessTools.Method(type: typeof(GenPlace), name: nameof(GenPlace.TryPlaceThing), parameters: new[] { typeof(Thing), typeof(IntVec3), typeof(ThingPlaceMode), typeof(Action<Thing, int>), typeof(Predicate<IntVec3>), typeof(Rot4) })))
                    {
                        Log.Message("Boss, wez found it!");
                        Log.Message(i + " opcode: " + instruction.opcode + " operand: " + instruction.operand);
                    }
                 }
                 */
                /*
                if (i > 1 && i < instructionsList.Count)
                {
                    if (instructionsList[index: i].OperandIs(AccessTools.Method(type: typeof(QuestManager), name: nameof(QuestManager.Notify_PlantHarvested), parameters: new[] { typeof(Pawn), typeof(Thing) })))
                    {
                        Log.Message("Boss, wez found QuestManager.Notify_PlantHarvested!");
                        Log.Message((i) + " opcode: " + instruction.opcode + " operand: " + instruction.operand);
                        yield return new CodeInstruction(opcode: OpCodes.Ldloc_0);             // Pawn
                        yield return new CodeInstruction(opcode: OpCodes.Ldloc_2);             // Plant
                        yield return new CodeInstruction(opcode: OpCodes.Call, operand: typeof(JobDriver_PlantWork_MakeNewToils_Fungus_Transpiler).GetMethod("FungusSpawner"));

                    }
                }
                */
            }

        }


        public static ThingDef FungalMeds = DefDatabase<ThingDef>.GetNamedSilentFail("OG_Orkoid_MedicineFungal");

        public static Thing FungusHarvest(ThingDef harvested, ThingDef stuff, Pawn pawn, Plant plant)
        {
            ThingDef harvestedThingDef = harvested;
            OrkoidFungus fungus = plant as OrkoidFungus;
            if (fungus != null && (pawn != null && pawn.isOrkoid()))
            {
            //    Log.Message(pawn.Name+" 'ez arvestin " + plant.LabelCap + " boss");

                Rand.PushState(plant.thingIDNumber);

                if (FungalMeds != null && Rand.ChanceSeeded(0.01f, plant.thingIDNumber))
                {
                    harvestedThingDef = FungalMeds;
                //    Log.Message("we'ez found some meds boss!");
                }

                Rand.PopState();
            }
            return ThingMaker.MakeThing(harvestedThingDef);
        }


        public static bool FungusHarvested(Thing thing, IntVec3 center, Map map, ThingPlaceMode mode, Action<Thing, int> placedAction, Predicate<IntVec3> nearPlaceValidator, Rot4 rot, Pawn pawn, Plant plant)
        {
            OrkoidFungus fungus = plant as OrkoidFungus;
            if (fungus != null && (pawn != null && pawn.isOrkoid()))
            {
                ThingDef harvestedThingDef = plant.def.plant.harvestedThingDef;
                if (FungalMeds != null && thing.def == FungalMeds)
                {
                    Rand.PushState();
                    thing.stackCount = Rand.RangeInclusive(1, Math.Max(thing.stackCount / 4, 1));
                    Rand.PopState();
                }
                if (thing.def == plant.def.plant.harvestedThingDef)
                {
                    if (fungus.TrySpawnPawns(pawn))
                    {
                        return true;
                    }
                }
            //    Log.Message(pawn.Name + " 'ez arvested " + thing.stackCount + " " + thing.LabelCap + " from " + plant.LabelCap + " boss!");
            }
            return GenPlace.TryPlaceThing(thing, center, map, ThingPlaceMode.Near, null, null, default(Rot4));
        }

        private static IEnumerable<Pawn> Squigs
        {
            get
            {
                return from p in Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer)
                       where p.isSquig()
                       select p;
            }
        }
        private static IEnumerable<Pawn> Snots
        {
            get
            {
                return from p in Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer)
                       where p.isSnotling()
                       select p;
            }
        }
        private static IEnumerable<Pawn> Grots
        {
            get
            {
                return from p in Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer)
                       where p.isGrot()
                       select p;
            }
        }
        private static IEnumerable<Pawn> Orks
        {
            get
            {
                return from p in Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer)
                       where p.isOrk()
                       select p;
            }
        }
        private static List<Pair<PawnKindDef, float>> Pairs (float squigChance, float snotlingChance, float grotChance, float orkChance)
        {
            return new List<Pair<PawnKindDef, float>>()
            {
                new Pair<PawnKindDef, float>(AdeptusPawnKindDefOf.OG_Squig, squigChance * (OrkoidFungalUtility.GrotSpawnCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent + Squigs.Count())* Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor)),
                new Pair<PawnKindDef, float>(AdeptusPawnKindDefOf.OG_Ork_Snotling, snotlingChance * (OrkoidFungalUtility.GrotSpawnCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent + Snots.Count())* Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor)),
                new Pair<PawnKindDef, float>(AdeptusPawnKindDefOf.OG_Grot_Wild, grotChance * (OrkoidFungalUtility.GrotSpawnCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent + Grots.Count())* Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor)),
                new Pair<PawnKindDef, float>(AdeptusPawnKindDefOf.OG_Ork_Wild, orkChance * OrkoidFungalUtility.OrkSpawnCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent + Orks.Count())* Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor)
            };
        }


    }

}
