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
                yield return instruction;
                /*
                if (i > 1 && i + 1 < instructionsList.Count)
                {
                    if (instructionsList[index: i - 1].Calls(AccessTools.Method(type: typeof(GenPlace), name: nameof(GenPlace.TryPlaceThing))))
                    {

                        Log.Message("Boss, wez found it!");
                        Log.Message(i + " opcode: " + instruction.opcode + " operand: " + instruction.operand);
                    }
                    if (instructionsList[index: i - 1].OperandIs(AccessTools.Method(type: typeof(GenPlace), name: nameof(GenPlace.TryPlaceThing), parameters: new[] { typeof(Thing), typeof(Vector3), typeof(ThingPlaceMode), typeof(Action<Thing, int>), typeof(Predicate<IntVec3>), typeof(Rot4) })))
                    {
                        Log.Message("Boss, wez found it!");
                        Log.Message(i + " opcode: " + instruction.opcode + " operand: " + instruction.operand);
                    }
                 }
                 */
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
            }

        }
        
        public static void FungusSpawner(Pawn pawn, Plant plant)
        {
            return;
            if (pawn.isOrkoid() && plant.isOrkoidFungus())
            {
                bool canspawn = plant.HarvestableNow;
                bool spawnwild = plant.def.defName.Contains("Cocoon");// Props.spawnwild; 
                float Age = plant.Age;
                float Fertility = plant.GrowthRateFactor_Fertility;
                /*
                Comp_OrkoidSpores spores = plant.TryGetComp<Comp_OrkoidSpores>();
                if (spores != null)
                {
                    spores.SpawnPawns(pawn, plant);
                }
                */

                if (canspawn)
                {
                    Rand.PushState();
                    var spawnRoll = Rand.Value;
                    Rand.PopState();
                    if (spawnRoll < (plant.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonSpawnChance : AMMod.Instance.settings.FungusSpawnChance * plant.Growth))
                    {
                        string msg = string.Empty;
                        StringBuilder builder = new StringBuilder();
                        builder.AppendLine("Possible Spawns:");
                        List<Pair<PawnKindDef, float>> pairs = Pairs(
                            plant.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonSquigChance : AMMod.Instance.settings.FungusSquigChance,
                            plant.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonSnotChance : AMMod.Instance.settings.FungusSnotChance,
                            plant.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonGrotChance : AMMod.Instance.settings.FungusGrotChance,
                            plant.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonOrkChance : AMMod.Instance.settings.FungusOrkChance);
                        foreach (var item in pairs)
                        {
                            builder.Append(" " + item.First.LabelCap + " weighted at " + item.Second);
                        }

                        Pair<PawnKindDef, float> pair = pairs.RandomElementByWeight(x => x.Second);
                        PawnKindDef pawnKindDef = pair.First;
                        builder.Append(" " + "Spawning " + pawnKindDef.LabelCap);
                        Log.Message(builder.ToString());
                        PawnGenerationRequest pawnGenerationRequest = new PawnGenerationRequest(pawnKindDef, spawnwild ? null : Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, true, true, false, false, true, true, 0f, fixedGender: Gender.None, fixedBiologicalAge: Age, fixedChronologicalAge: Age);

                        Pawn spawned = PawnGenerator.GeneratePawn(pawnGenerationRequest);

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
                                    pawnKind = spawned.def.defName.Contains("Alien_Grot") ? OGOrkPawnKindDefOf.Tribesperson_OG_Grot : DefDatabase<PawnKindDef>.AllDefsListForReading.Where(x => x.defName.Contains("Tribesperson_OG_Ork")).RandomElement();
                                }
                                else
                                {
                                    pawnKind = spawned.def.defName.Contains("Alien_Grot") ? OGOrkPawnKindDefOf.Colonist_OG_Grot : DefDatabase<PawnKindDef>.AllDefsListForReading.Where(x => x.defName.Contains("Colonist_OG_Ork")).RandomElement();
                                }
                                spawned.ChangeKind(pawnKind);
                            }
                            else
                            {
                                spawned.ChangeKind(PawnKindDefOf.WildMan);
                            }
                            spawned.story.bodyType = spawned.story.childhood.BodyTypeFor(spawned.gender);
                        }
                        if (Fertility < 1f)
                        {
                            foreach (Need need in spawned.needs.AllNeeds)
                            {
                                need.CurLevel = 0f;
                            }
                            Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.Malnutrition, spawned);
                            hediff.Severity = Math.Min(1f - Fertility, 0.75f);
                            spawned.health.AddHediff(hediff);
                        }
                        else
                        {
                            foreach (Need need in spawned.needs.AllNeeds)
                            {
                                need.CurLevel = Fertility - 1f;
                            }
                        }
                        GenSpawn.Spawn(spawned, plant.Position, plant.Map, 0);
                    }
                }
            }

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
                new Pair<PawnKindDef, float>(OGOrkPawnKindDefOf.OG_Squig, squigChance * (OrkoidFungualUtility.GrotSpawnCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent + Squigs.Count())* Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor)),
                new Pair<PawnKindDef, float>(OGOrkPawnKindDefOf.OG_Ork_Snotling, snotlingChance * (OrkoidFungualUtility.GrotSpawnCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent + Snots.Count())* Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor)),
                new Pair<PawnKindDef, float>(OGOrkPawnKindDefOf.OG_Grot_Wild, grotChance * (OrkoidFungualUtility.GrotSpawnCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent + Grots.Count())* Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor)),
                new Pair<PawnKindDef, float>(OGOrkPawnKindDefOf.OG_Ork_Wild, orkChance * OrkoidFungualUtility.OrkSpawnCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent + Orks.Count())* Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor)
            };
        }


    }

}
