using System;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using AdeptusMechanicus.ExtensionMethods;
using Verse;
using System.Collections.Generic;
using Verse.AI;
using RBB_Code;

namespace AdeptusMechanicus.HarmonyInstance
{
    // RBB_Code.JobDriver_CatchFish
    public class JobDriver_CatchFish_MakeNewToils_Snotling_Patch
    {
        public static bool Prefix(JobDriver_CatchFish __instance, ref IEnumerable<Toil> __result)
        {
            if (__instance.pawn.isSnotling())
            {
				__result = MakeNewToils(__instance, __instance.job.targetA.Thing);
				return false;
			}

			return true;
        }

		public static IEnumerable<Toil> MakeNewToils(JobDriver_CatchFish __instance, Thing ___TargetThingA)
		{
			int fishingDuration = 2000;
			Building_FishingSpot fishingSpot = ___TargetThingA as Building_FishingSpot;
			Passion passion = Passion.None;
			float skillGainFactor = 0f;
			__instance.AddEndCondition(delegate
			{
				Thing thing = __instance.pawn.jobs.curJob.GetTarget(__instance.fishingSpotIndex).Thing;
				if (thing is Building && !thing.Spawned)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			__instance.FailOnBurningImmobile(__instance.fishingSpotIndex);
			__instance.rotateToFace = TargetIndex.B;
			yield return Toils_Reserve.Reserve(__instance.fishingSpotIndex, 1, -1, null);
			float fishingSkillLevel = 0f;

			if (__instance.pawn.skills != null)
			{
				fishingSkillLevel = __instance.pawn.skills.AverageOfRelevantSkillsFor(RBB_Code.WorkTypeDefOf.Fishing);
			}
            else
            {
				fishingSkillLevel = 1f;
			}
			float num = fishingSkillLevel / 20f;
			fishingDuration = (int)(2000f * (1.5f - num));
			fishingDuration = (int)((float)fishingDuration / (Controller.Settings.fishingEfficiency / 100f));
			yield return Toils_Goto.GotoThing(__instance.fishingSpotIndex, fishingSpot.InteractionCell).FailOnDespawnedOrNull(__instance.fishingSpotIndex);
			Toil toil = new Toil
			{
				initAction = delegate ()
				{
					ThingDef def;
					if (fishingSpot.Rotation == Rot4.North)
					{
						def = ThingDef.Named("Mote_FishingRodNorth");
					}
					else if (fishingSpot.Rotation == Rot4.East)
					{
						def = ThingDef.Named("Mote_FishingRodEast");
					}
					else if (fishingSpot.Rotation == Rot4.South)
					{
						def = ThingDef.Named("Mote_FishingRodSouth");
					}
					else
					{
						def = ThingDef.Named("Mote_FishingRodWest");
					}
					__instance.fishingRodMote = (Mote)ThingMaker.MakeThing(def, null);
					__instance.fishingRodMote.exactPosition = fishingSpot.fishingSpotCell.ToVector3Shifted();
					__instance.fishingRodMote.Scale = 1f;
					GenSpawn.Spawn(__instance.fishingRodMote, fishingSpot.fishingSpotCell, fishingSpot.Map, WipeMode.Vanish);
					WorkTypeDef fishing = RBB_Code.WorkTypeDefOf.Fishing;
					if (__instance.pawn.skills != null)
					{
						passion = __instance.pawn.skills.MaxPassionOfRelevantSkillsFor(fishing);
					}
                    else
                    {
						passion = Passion.None;

					}
					if (passion == Passion.None)
					{
						skillGainFactor = 0.3f;
						return;
					}
					if (passion == Passion.Minor)
					{
						skillGainFactor = 1f;
						return;
					}
					skillGainFactor = 1.5f;
				},
				tickAction = delegate ()
				{
                    if (__instance.pawn.skills != null)
					{
						__instance.pawn.skills.Learn(SkillDefOf.Animals, 0.01f * skillGainFactor, false);
					}
					if (__instance.ticksLeftThisToil == 1)
					{
						if (__instance.fishingRodMote != null)
						{
							__instance.fishingRodMote.Destroy(DestroyMode.Vanish);
						}
						List<Thing> list = fishingSpot.Map.thingGrid.ThingsListAt(fishingSpot.fishingSpotCell);
						for (int i = 0; i < list.Count; i++)
						{
							if (list[i].def.defName.Contains("Mote_Fishing"))
							{
								list[i].DeSpawn(DestroyMode.Vanish);
							}
						}
					}
				},
				defaultDuration = fishingDuration,
				defaultCompleteMode = ToilCompleteMode.Delay
			};
			yield return toil.WithProgressBarToilDelay(__instance.fishingSpotIndex, false, -0.5f);
			Toil toil2 = new Toil
			{
				initAction = delegate ()
				{
					Job curJob = __instance.pawn.jobs.curJob;
					Thing thing = null;
					float num2 = Rand.Value;
					TerrainDef terrainDef = fishingSpot.Map.terrainGrid.TerrainAt(fishingSpot.fishingSpotCell);
					if (terrainDef.defName.Equals("Marsh") || fishingSpot.Map.Biome.defName.Contains("Swamp"))
					{
						num2 -= 0.0025f;
					}
					if ((double)num2 < 0.0025)
					{
						MoteMaker.ThrowMetaIcon(__instance.pawn.Position, fishingSpot.Map, ThingDefOf.Mote_IncapIcon);
						__instance.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
						string str;
						if (fishingSpot.Map.Biome.defName.Contains("Tropical") && !terrainDef.defName.Contains("Ocean") && (double)Rand.Value < 0.33)
						{
							str = __instance.pawn.Name.ToStringShort.CapitalizeFirst() + "RBB.TragedyPiranhaText".Translate();
							Find.LetterStack.ReceiveLetter("RBB.TragedyTitle".Translate(), str, LetterDefOf.NegativeEvent, __instance.pawn, null, null, null, null);
							int num3 = Rand.Range(3, 6);
							for (int i = 0; i < num3; i++)
							{
								__instance.pawn.TakeDamage(new DamageInfo(DamageDefOf.Bite, (float)Rand.Range(1, 4), 0f, -1f, __instance.pawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
							}
							return;
						}
						if (terrainDef.defName.Contains("Ocean") && (double)Rand.Value < 0.33)
						{
							str = __instance.pawn.Name.ToStringShort.CapitalizeFirst() + "RBB.TragedyJellyfishText".Translate();
							Find.LetterStack.ReceiveLetter("RBB.TragedyTitle".Translate(), str, LetterDefOf.NegativeEvent, __instance.pawn, null, null, null, null);
							__instance.pawn.TakeDamage(new DamageInfo(DamageDefOf.Burn, (float)Rand.Range(3, 8), 0f, -1f, __instance.pawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
							return;
						}
						str = __instance.pawn.Name.ToStringShort.CapitalizeFirst() + "RBB.TragedyText".Translate();
						Find.LetterStack.ReceiveLetter("RBB.TragedyTitle".Translate(), str, LetterDefOf.NegativeEvent, __instance.pawn, null, null, null, null);
						__instance.pawn.TakeDamage(new DamageInfo(DamageDefOf.Bite, (float)Rand.Range(3, 8), 0f, -1f, __instance.pawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
						return;
					}
					else
					{
						float num4 = 0.2f + fishingSkillLevel / 40f;
						num4 *= Controller.Settings.fishingEfficiency / 100f;
						if (fishingSpot.Map.Biome == BiomeDef.Named("AridShrubland") || fishingSpot.Map.Biome.defName.Contains("DesertArchi") || fishingSpot.Map.Biome.defName.Contains("Boreal"))
						{
							num4 -= 0.05f;
						}
						else if (fishingSpot.Map.Biome.defName.Contains("Oasis"))
						{
							num4 -= 0.075f;
						}
						else if (fishingSpot.Map.Biome.defName.Contains("ColdBog") || fishingSpot.Map.Biome == BiomeDef.Named("Desert") || fishingSpot.Map.Biome.defName.Contains("Tundra"))
						{
							num4 -= 0.1f;
						}
						else if (fishingSpot.Map.Biome.defName.Contains("Permafrost") || fishingSpot.Map.Biome.defName == "RRP_TemperateDesert")
						{
							num4 -= 0.125f;
						}
						else if (fishingSpot.Map.Biome == BiomeDef.Named("ExtremeDesert") || fishingSpot.Map.Biome == BiomeDef.Named("IceSheet") || fishingSpot.Map.Biome == BiomeDef.Named("SeaIce"))
						{
							num4 -= 0.15f;
						}
						if (Rand.Value > num4)
						{
							string text = __instance.pawn.Name.ToStringShort.CapitalizeFirst() + "RBB.CaughtNothing".Translate();
							if (Rand.Value < 0.75f)
							{
								MoteMaker.ThrowMetaIcon(__instance.pawn.Position, fishingSpot.Map, ThingDefOf.Mote_IncapIcon);
								__instance.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
								if (Controller.Settings.failureLevel >= 1f)
								{
									if (Controller.Settings.failureLevel < 2f)
									{
										Messages.Message(text, new TargetInfo(__instance.pawn.Position, fishingSpot.Map, false), MessageTypeDefOf.SilentInput, true);
										return;
									}
									if (Controller.Settings.failureLevel < 3f)
									{
										Messages.Message(text, new TargetInfo(__instance.pawn.Position, fishingSpot.Map, false), MessageTypeDefOf.NegativeEvent, true);
										return;
									}
									Find.LetterStack.ReceiveLetter("RBB.CaughtNothingTitle".Translate(), text, LetterDefOf.NegativeEvent, __instance.pawn, null, null, null, null);
								}
								return;
							}
							float value = Rand.Value;
							float value2 = Rand.Value;
							if (value < 0.75f)
							{
								if (value2 < 0.5f)
								{
									thing = GenSpawn.Spawn(ThingDefOf.WoodLog, __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
								}
								else
								{
									thing = GenSpawn.Spawn(ThingDefOf.Cloth, __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
								}
							}
							else if (value2 < 0.4f)
							{
								thing = GenSpawn.Spawn(ThingDefOf.WoodLog, __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
							}
							else if (value2 < 0.7f)
							{
								thing = GenSpawn.Spawn(ThingDefOf.Cloth, __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
							}
							else if (value2 < 0.8f)
							{
								thing = GenSpawn.Spawn(ThingDefOf.Steel, __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
							}
							else if (value2 < 0.85f)
							{
								thing = GenSpawn.Spawn(ThingDef.Named("WoolMuffalo"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
							}
							else if (value2 < 0.9f)
							{
								thing = GenSpawn.Spawn(ThingDef.Named("WoolMegasloth"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
							}
							else if (value2 < 0.975f)
							{
								thing = GenSpawn.Spawn(ThingDef.Named("WoolCamel"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
							}
							else if (value2 < 0.97f)
							{
								thing = GenSpawn.Spawn(ThingDef.Named("WoolAlpaca"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
							}
							else
							{
								thing = GenSpawn.Spawn(ThingDef.Named("Synthread"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
							}
							if (Rand.Value < 0.75f)
							{
								fishingSpot.fishStock--;
							}
							text = __instance.pawn.Name.ToStringShort.CapitalizeFirst() + "RBB.SnaggedJunk".Translate() + thing.def.label + ".";
							if (Controller.Settings.junkLevel >= 1f)
							{
								if (Controller.Settings.junkLevel < 2f)
								{
									Messages.Message(text, new TargetInfo(__instance.pawn.Position, fishingSpot.Map, false), MessageTypeDefOf.SilentInput, true);
								}
								else if (Controller.Settings.junkLevel < 3f)
								{
									Messages.Message(text, new TargetInfo(__instance.pawn.Position, fishingSpot.Map, false), MessageTypeDefOf.NegativeEvent, true);
								}
								else
								{
									Find.LetterStack.ReceiveLetter("RBB.SnaggedJunkTitle".Translate(), text, LetterDefOf.NegativeEvent, __instance.pawn, null, null, null, null);
								}
							}
						}
						else
						{
							float num5 = fishingSkillLevel * 0.0002f;
							if (terrainDef.defName.Contains("Deep"))
							{
								num5 += 0.001f;
							}
							if (Rand.Value <= num5)
							{
								float value3 = Rand.Value;
								float value4 = Rand.Value;
								if (value3 < 0.75f)
								{
									thing = GenSpawn.Spawn(ThingDefOf.Silver, __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
									thing.stackCount = Rand.RangeInclusive(5, 50);
								}
								else if (value4 < 0.6f)
								{
									thing = GenSpawn.Spawn(ThingDefOf.Silver, __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
									thing.stackCount = Rand.RangeInclusive(10, 100);
								}
								else if (value4 < 0.9f)
								{
									thing = GenSpawn.Spawn(ThingDefOf.Gold, __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
									thing.stackCount = Rand.RangeInclusive(10, 100);
								}
								else if (value4 < 0.96f)
								{
									thing = GenSpawn.Spawn(ThingDef.Named("Gun_ChargeRifle"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
								}
								else if (value4 < 0.98f)
								{
									thing = GenSpawn.Spawn(ThingDef.Named("SimpleProstheticArm"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
								}
								else
								{
									thing = GenSpawn.Spawn(ThingDef.Named("SimpleProstheticLeg"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
								}
								string text2 = string.Concat(new object[]
								{
									__instance.pawn.Name.ToStringShort.CapitalizeFirst() + "RBB.SunkenTreasureText".Translate(),
									thing.stackCount,
									" ",
									thing.def.label,
									"."
								});
								if (Controller.Settings.treasureLevel >= 1f)
								{
									if (Controller.Settings.treasureLevel < 2f)
									{
										Messages.Message(text2, new TargetInfo(__instance.pawn.Position, fishingSpot.Map, false), MessageTypeDefOf.SilentInput, true);
									}
									else if (Controller.Settings.treasureLevel < 3f)
									{
										Messages.Message(text2, new TargetInfo(__instance.pawn.Position, fishingSpot.Map, false), MessageTypeDefOf.PositiveEvent, true);
									}
									else
									{
										Find.LetterStack.ReceiveLetter("RBB.SunkenTreasureTitle".Translate(), text2, LetterDefOf.PositiveEvent, __instance.pawn, null, null, null, null);
									}
								}
							}
							else
							{
								float num6 = Rand.Value + num4;
								string text3 = __instance.pawn.Name.ToStringShort.CapitalizeFirst();
								if (num6 >= 0.4f && num6 < 0.9f)
								{
									thing = GenSpawn.Spawn(ThingDef.Named("RawFishTiny"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
									text3 += "RBB.TinyFish".Translate();
								}
								else if (terrainDef.defName == "Marsh" && num6 >= 0.9f && num6 < 1.15f)
								{
									thing = GenSpawn.Spawn(ThingDef.Named("Eel"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
									text3 += "RBB.Eel".Translate();
								}
								else if (num6 >= 1.15f && num6 < 1.4f)
								{
									thing = GenSpawn.Spawn(ThingDef.Named("Eel"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
									text3 += "RBB.Eel".Translate();
								}
								else
								{
									float value5 = Rand.Value;
									if (terrainDef.defName.Contains("Ocean"))
									{
										if (terrainDef.defName.Contains("Deep") && (double)value5 > 0.7)
										{
											if ((double)value5 > 0.85)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("Squid"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Squid".Translate();
											}
											else
											{
												thing = GenSpawn.Spawn(ThingDef.Named("SeaCucumber"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.SeaCucumber".Translate();
											}
										}
										else if (fishingSpot.Map.Biome.defName == "IceSheet" || fishingSpot.Map.Biome.defName == "SeaIce")
										{
											if ((double)value5 < 0.025)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("Salmon"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Salmon".Translate();
											}
											else if ((double)value5 < 0.125)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("Jellyfish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Jellyfish".Translate();
											}
											else if ((double)value5 < 0.475)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("RawFishTiny"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.TinyFish".Translate();
											}
											else if ((double)value5 > 0.975)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("Squid"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Squid".Translate();
											}
											else if ((double)value5 > 0.95)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("SeaCucumber"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.SeaCucumber".Translate();
											}
											else
											{
												thing = GenSpawn.Spawn(ThingDef.Named("RawFish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Fish".Translate();
											}
										}
										else if (fishingSpot.Map.Biome.defName.Contains("Tundra") || fishingSpot.Map.Biome.defName.Contains("Permafrost") || fishingSpot.Map.Biome.defName.Contains("Boreal") || fishingSpot.Map.Biome.defName.Contains("ColdBog"))
										{
											if ((double)value5 < 0.15)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("Salmon"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Salmon".Translate();
											}
											else if ((double)value5 < 0.25)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("Jellyfish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Jellyfish".Translate();
											}
											else if ((double)value5 < 0.35)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("RawFishTiny"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.TinyFish".Translate();
											}
											else if ((double)value5 < 0.4)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("Bass"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Bass".Translate();
											}
											else if ((double)value5 > 0.95)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("Squid"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Squid".Translate();
											}
											else if ((double)value5 > 0.9)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("SeaCucumber"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.SeaCucumber".Translate();
											}
											else
											{
												thing = GenSpawn.Spawn(ThingDef.Named("RawFish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Fish".Translate();
											}
										}
										else if ((fishingSpot.Map.Biome.defName.Contains("Temperate") && fishingSpot.Map.Biome.defName != "RRP_TemperateDesert") || fishingSpot.Map.Biome.defName.Contains("Steppes") || fishingSpot.Map.Biome.defName.Contains("Grassland") || fishingSpot.Map.Biome.defName.Contains("Savanna"))
										{
											if ((double)value5 < 0.05)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("Salmon"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Salmon".Translate();
											}
											else if ((double)value5 < 0.15)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("Jellyfish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Jellyfish".Translate();
											}
											else if ((double)value5 < 0.25)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("Pufferfish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Pufferfish".Translate();
											}
											else if ((double)value5 < 0.45)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("Bass"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Bass".Translate();
											}
											else if ((double)value5 > 0.95)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("Squid"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Squid".Translate();
											}
											else if ((double)value5 > 0.9)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("SeaCucumber"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.SeaCucumber".Translate();
											}
											else
											{
												thing = GenSpawn.Spawn(ThingDef.Named("RawFish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Fish".Translate();
											}
										}
										else if (fishingSpot.Map.Biome.defName.Contains("Tropical") || fishingSpot.Map.Biome.defName == "AridShrubland" || fishingSpot.Map.Biome.defName.Contains("Desert") || fishingSpot.Map.Biome.defName.Contains("Oasis"))
										{
											if ((double)value5 < 0.1)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("Jellyfish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Jellyfish".Translate();
											}
											else if ((double)value5 < 0.25)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("Pufferfish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Pufferfish".Translate();
											}
											else if ((double)value5 < 0.45)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("Bass"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Bass".Translate();
											}
											else if ((double)value5 > 0.95)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("Squid"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Squid".Translate();
											}
											else if ((double)value5 > 0.9)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("SeaCucumber"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.SeaCucumber".Translate();
											}
											else
											{
												thing = GenSpawn.Spawn(ThingDef.Named("RawFish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Fish".Translate();
											}
										}
										else if ((double)value5 < 0.1)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Jellyfish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Jellyfish".Translate();
										}
										else if ((double)value5 < 0.2)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Pufferfish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Pufferfish".Translate();
										}
										else if ((double)value5 < 0.3)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("RawFishTiny"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.TinyFish".Translate();
										}
										else if ((double)value5 < 0.4)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Bass"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Bass".Translate();
										}
										else if ((double)value5 > 0.95)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Squid"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Squid".Translate();
										}
										else if ((double)value5 > 0.9)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("SeaCucumber"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.SeaCucumber".Translate();
										}
										else
										{
											thing = GenSpawn.Spawn(ThingDef.Named("RawFish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Fish".Translate();
										}
									}
									else if (fishingSpot.Map.Biome.defName.Contains("Tundra") || fishingSpot.Map.Biome.defName.Contains("Permafrost"))
									{
										if ((double)value5 < 0.5)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Salmon"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Salmon".Translate();
										}
										else if ((double)value5 < 0.7)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Bass"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Bass".Translate();
										}
										else if ((double)value5 < 0.75)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Catfish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Catfish".Translate();
										}
										else
										{
											thing = GenSpawn.Spawn(ThingDef.Named("RawFish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Fish".Translate();
										}
									}
									else if (fishingSpot.Map.Biome.defName.Contains("Boreal") || fishingSpot.Map.Biome.defName.Contains("ColdBog"))
									{
										if ((double)value5 < 0.02)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("SturgeonCaviar"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Sturgeon".Translate();
										}
										else if ((double)value5 < 0.1)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Sturgeon"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Sturgeon".Translate();
										}
										else if ((double)value5 < 0.5)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Salmon"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Salmon".Translate();
										}
										else if ((double)value5 < 0.7)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Bass"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Bass".Translate();
										}
										else if ((double)value5 < 0.75)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Catfish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Catfish".Translate();
										}
										else
										{
											thing = GenSpawn.Spawn(ThingDef.Named("RawFish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Fish".Translate();
										}
									}
									else if ((fishingSpot.Map.Biome.defName.Contains("Temperate") && fishingSpot.Map.Biome.defName != "RRP_TemperateDesert") || fishingSpot.Map.Biome.defName.Contains("Steppes") || fishingSpot.Map.Biome.defName.Contains("Grassland") || fishingSpot.Map.Biome.defName.Contains("Savanna"))
									{
										if (fishingSpot.Map.Biome.defName.Contains("Swamp"))
										{
											if ((double)value5 < 0.02)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("SturgeonCaviar"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Sturgeon".Translate();
											}
											else if ((double)value5 < 0.1)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("Sturgeon"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Sturgeon".Translate();
											}
											else if ((double)value5 < 0.2)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("Salmon"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Salmon".Translate();
											}
											else if ((double)value5 < 0.45)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("Bass"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Bass".Translate();
											}
											else if ((double)value5 > 0.75)
											{
												thing = GenSpawn.Spawn(ThingDef.Named("Catfish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Catfish".Translate();
											}
											else
											{
												thing = GenSpawn.Spawn(ThingDef.Named("RawFish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
												text3 += "RBB.Fish".Translate();
											}
										}
										else if ((double)value5 < 0.04)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("SturgeonCaviar"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Sturgeon".Translate();
										}
										else if ((double)value5 < 0.2)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Sturgeon"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Sturgeon".Translate();
										}
										else if ((double)value5 < 0.3)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Salmon"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Salmon".Translate();
										}
										else if ((double)value5 < 0.6)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Bass"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Bass".Translate();
										}
										else if ((double)value5 > 0.75)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Catfish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Catfish".Translate();
										}
										else
										{
											thing = GenSpawn.Spawn(ThingDef.Named("RawFish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Fish".Translate();
										}
									}
									else if (fishingSpot.Map.Biome.defName.Contains("Tropical"))
									{
										if ((double)value5 < 0.01)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("SturgeonCaviar"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Sturgeon".Translate();
										}
										else if ((double)value5 < 0.05)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Sturgeon"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Sturgeon".Translate();
										}
										else if ((double)value5 < 0.15)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Arapaima"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Arapaima".Translate();
										}
										else if ((double)value5 < 0.45)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Piranha"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Piranha".Translate();
										}
										else if ((double)value5 < 0.65)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Bass"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Bass".Translate();
										}
										else if ((double)value5 > 0.75)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Catfish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Catfish".Translate();
										}
										else
										{
											thing = GenSpawn.Spawn(ThingDef.Named("RawFish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Fish".Translate();
										}
									}
									else if (fishingSpot.Map.Biome.defName == "AridShrubland" || fishingSpot.Map.Biome.defName.Contains("DesertArchi"))
									{
										if ((double)value5 < 0.02)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("SturgeonCaviar"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Sturgeon".Translate();
										}
										else if ((double)value5 < 0.1)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Sturgeon"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Sturgeon".Translate();
										}
										else if ((double)value5 < 0.2)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("RawFishTiny"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.TinyFish".Translate();
										}
										else if ((double)value5 < 0.7)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Bass"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Bass".Translate();
										}
										else if ((double)value5 > 0.75)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Catfish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Catfish".Translate();
										}
										else
										{
											thing = GenSpawn.Spawn(ThingDef.Named("RawFish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Fish".Translate();
										}
									}
									else if (fishingSpot.Map.Biome.defName == "Desert" || fishingSpot.Map.Biome.defName.Contains("Oasis"))
									{
										if ((double)value5 < 0.01)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("SturgeonCaviar"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Sturgeon".Translate();
										}
										else if ((double)value5 < 0.05)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Sturgeon"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Sturgeon".Translate();
										}
										else if ((double)value5 < 0.3)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("RawFishTiny"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.TinyFish".Translate();
										}
										else if ((double)value5 < 0.75)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Bass"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Bass".Translate();
										}
										else
										{
											thing = GenSpawn.Spawn(ThingDef.Named("RawFish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Fish".Translate();
										}
									}
									else if (fishingSpot.Map.Biome.defName == "ExtremeDesert" || fishingSpot.Map.Biome.defName == "RRP_TemperateDesert")
									{
										if ((double)value5 < 0.5)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("RawFishTiny"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.TinyFish".Translate();
										}
										else if ((double)value5 < 0.75)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Bass"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Bass".Translate();
										}
										else
										{
											thing = GenSpawn.Spawn(ThingDef.Named("RawFish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Fish".Translate();
										}
									}
									else if (fishingSpot.Map.Biome.defName == "IceSheet" || fishingSpot.Map.Biome.defName == "SeaIce")
									{
										if ((double)value5 < 0.15)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Salmon"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Salmon".Translate();
										}
										else if ((double)value5 < 0.65)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("RawFishTiny"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.TinyFish".Translate();
										}
										else if ((double)value5 < 0.75)
										{
											thing = GenSpawn.Spawn(ThingDef.Named("Bass"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Bass".Translate();
										}
										else
										{
											thing = GenSpawn.Spawn(ThingDef.Named("RawFish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
											text3 += "RBB.Fish".Translate();
										}
									}
									else if ((double)value5 < 0.01)
									{
										thing = GenSpawn.Spawn(ThingDef.Named("SturgeonCaviar"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
										text3 += "RBB.Sturgeon".Translate();
									}
									else if ((double)value5 < 0.05)
									{
										thing = GenSpawn.Spawn(ThingDef.Named("Sturgeon"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
										text3 += "RBB.Sturgeon".Translate();
									}
									else if ((double)value5 < 0.2)
									{
										thing = GenSpawn.Spawn(ThingDef.Named("RawFishTiny"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
										text3 += "RBB.TinyFish".Translate();
									}
									else if ((double)value5 < 0.7)
									{
										thing = GenSpawn.Spawn(ThingDef.Named("Bass"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
										text3 += "RBB.Bass".Translate();
									}
									else if ((double)value5 > 0.75)
									{
										thing = GenSpawn.Spawn(ThingDef.Named("Catfish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
										text3 += "RBB.Catfish".Translate();
									}
									else
									{
										thing = GenSpawn.Spawn(ThingDef.Named("RawFish"), __instance.pawn.Position, fishingSpot.Map, WipeMode.Vanish);
										text3 += "RBB.Fish".Translate();
									}
								}
								fishingSpot.fishStock--;
								if (Controller.Settings.successLevel >= 1f)
								{
									if (Controller.Settings.successLevel < 2f)
									{
										Messages.Message(text3, new TargetInfo(__instance.pawn.Position, fishingSpot.Map, false), MessageTypeDefOf.SilentInput, true);
									}
									else if (Controller.Settings.successLevel < 3f)
									{
										Messages.Message(text3, new TargetInfo(__instance.pawn.Position, fishingSpot.Map, false), MessageTypeDefOf.PositiveEvent, true);
									}
									else
									{
										Find.LetterStack.ReceiveLetter("RBB.FishingSuccessTitle".Translate(), text3, LetterDefOf.PositiveEvent, __instance.pawn, null, null, null, null);
									}
								}
							}
						}
						__instance.pawn.carryTracker.TryStartCarry(thing, thing.stackCount, true);
						__instance.pawn.carryTracker.TryDropCarriedThing(__instance.pawn.Position, ThingPlaceMode.Near, out thing, null);
						if (!Controller.Settings.fishersHaul.Equals(true))
						{
							__instance.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
							return;
						}
						IntVec3 c;
						if (StoreUtility.TryFindBestBetterStoreCellFor(thing, __instance.pawn, fishingSpot.Map, StoragePriority.Unstored, __instance.pawn.Faction, out c, true))
						{
							__instance.pawn.Reserve(thing, __instance.job, 1, -1, null, true);
							__instance.pawn.Reserve(c, __instance.job, 1, -1, null, true);
							__instance.pawn.CurJob.SetTarget(TargetIndex.B, c);
							__instance.pawn.CurJob.SetTarget(TargetIndex.A, thing);
							__instance.pawn.CurJob.count = 1;
							__instance.pawn.CurJob.haulMode = HaulMode.ToCellStorage;
							return;
						}
						__instance.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
						return;
					}
				}
			};
			yield return toil2;
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true, false);
			yield return Toils_Reserve.Release(__instance.fishingSpotIndex);
			yield break;
		}
	}
}
