using System;
using System.Collections.Generic;
using System.Linq;
using AdeptusMechanicus.ExtensionMethods;
using AdeptusMechanicus.settings;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.Need_Orkoid_Fightyness
    public class Need_Orkoid_Fightyness : Need
	{
		public Need_Orkoid_Fightyness(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(Wants);
			this.threshPercents.Add(Desires);
			this.threshPercents.Add(Craves);
			this.threshPercents.Add(Requires);
			this.threshPercents.Add(Obsessed);
		}
		private float Wants => 0.3f;
		private float Desires => 0.5f;
		private float Craves => 0.65f;
		private float Requires => 0.85f;
		private float Obsessed => 0.9f;
		public RowdynessCategory CurCategory
		{
			get
			{
				if (base.CurLevelPercentage < Wants) // 0.0500000007450581
				{
					return RowdynessCategory.Free;
				}
				if (base.CurLevelPercentage < Desires) // 0.200000002980232
				{
					return RowdynessCategory.Wants;
				}
				if (base.CurLevelPercentage < Craves) // 0.400000005960464
				{
					return RowdynessCategory.Desires;
				}
				if (base.CurLevelPercentage < Requires) // 0.600000023841858
				{
					return RowdynessCategory.Craves;
				}
				if (base.CurLevelPercentage > Obsessed) // 0.800000011920929
				{
					return RowdynessCategory.Requires;
				}
				return RowdynessCategory.Obsessed;
			}
		}

		public override bool ShowOnNeedList
		{
			get
			{
				return !this.Disabled;
			}
		}

		public bool Disabled
		{
			get
			{
				bool result;
				if (this.pawn.isOrkoid())
				{
					result = !AMAMod.settings.OrkoidFightyness;// this.pawn.WorkTagIsDisabled(WorkTags.Violent) && !pawn.Downed;
				}
				else
				{
					result = true;
				}
				return result;
			}
		}

		public override void SetInitialLevel()
		{
			this.CurLevel = 0f;
		}

		public override void NeedInterval()
		{
			if (this.Disabled)
			{
				this.CurLevel = 0f;
			}
			else
			{
				if (!this.IsFrozen)
				{
					float num;
					if (this.pawn.Spawned)
					{
						num = FightynessRate;
					}
					else
					{
						num = 0.2f;
					}
					if (this.pawn.InBed() && (double)num < 0.0)
					{
						num *= DeltaFactor_InBed;
					}
					float num2 = num * 0.00333333341f;
					if (this.Fought || pawn.Downed)
					{
					//	if (AMAMod.Dev && this.Fought) Log.Message(pawn + " fought recently");
						num2 = -(num2 * (foughtSocially || pawn.Downed ? 0.5f : 1f));
                        if (this.Fought && TicksSatisfied == 0)
						{
							if (AMAMod.Dev) Log.Message(pawn + " now needs to fight again");
							this.fought = false;
						}
					}
					float curLevel = this.CurLevel;

					this.CurLevel = Mathf.Clamp(this.CurLevel+num2, 0, 1f);

					this.lastEffectiveDelta = this.CurLevel - curLevel;
					if (pawn.isOrk() && !(pawn.MentalState is MentalState_SocialFighting) && CurCategory != RowdynessCategory.Free && Math.Sign(this.lastEffectiveDelta) > 0)
					{
                        Rand.PushState();
					//	if (AMAMod.Dev) Log.Message(pawn.NameShortColored + " will start a fight "+ this.CurLevelPercentage +"%");
						if (Rand.Chance((this.CurLevelPercentage / 100f)*pawn.health.summaryHealth.SummaryHealthPercent))
						{
							Orkoid Orkiness = pawn.Orkiness();
							Orkoid maxOrkiness;

							int val1 = (int)Orkiness + (5 - (int)CurCategory);
							switch (CurCategory)
							{
								case RowdynessCategory.Obsessed:
									maxOrkiness = (Orkoid)Math.Min(val1, (int)Orkoid.Warboss);
									break;
								case RowdynessCategory.Requires:
									maxOrkiness = (Orkoid)Math.Min(val1, (int)Orkoid.Warboss);
									break;
								case RowdynessCategory.Craves:
                                    maxOrkiness = (Orkoid)Math.Min(val1, (int)Orkoid.Warboss);
									break;
								case RowdynessCategory.Desires:
									maxOrkiness = Orkoid.Ork;
									break;
								default:
									maxOrkiness = Orkoid.Snotling;
									break;
							}
                            if (maxOrkiness > Orkoid.Warboss)
                            {
								maxOrkiness = Orkoid.Warboss;
							}
							//	Log.Message("if it can find something to krump.....");
							Pawn t = RowdynessUtility.FindKrumpablePawnFor(pawn, this.CurCategory, maxOrkiness);
							if (t != null)
							{
								//	Log.Message("gonna try and krump "+t.NameShortColored);
								RowdynessUtility.StartScrap(pawn ,t);
							}
                        }
						Rand.PopState();
					}
                    if (pawn.isGrot())
                    {
						Rand.PushState();
						if (Rand.Chance((this.CurLevelPercentage / 100f) * pawn.health.summaryHealth.SummaryHealthPercent))
						{
							switch (CurCategory)
							{
								case RowdynessCategory.Obsessed:

									break;
								case RowdynessCategory.Requires:

									break;
								case RowdynessCategory.Craves:

									break;
								case RowdynessCategory.Desires:

									break;
								default:

									break;
							}
						}
						Rand.PopState();
					}
				}
			}
		}
		public int TicksSatisfied
		{
			get
			{
				int happyUntill = (lastFoughtTick + satisifiedTicks);
				int remaining = Mathf.Max(0, happyUntill - Find.TickManager.TicksGame);
				if (AMAMod.Dev && remaining < satisifiedTicks/ AMAMod.settings.OrkoidFightynessStatisfied) Log.Message($"{this.pawn.NameShortColored}, Cur Tick: {Find.TickManager.TicksGame}, Last Attack Tick: {lastFoughtTick}, Disabled Until: {happyUntill} remaining satisifiedTicks: {remaining}");
				return remaining;
			}
		}

		public bool Fought
		{
			get
			{
				return fought;
			}
			set
			{
				fought = value;
				lastFoughtTick = Find.TickManager.TicksGame;
			}
		}
		private float FightynessRate
		{
			get
			{
				return this.pawn.BodySize * (1f - (1f * (this.pawn.needs.joy != null ? this.pawn.needs.joy.CurLevel : 0f))) / 2;
			}
		}
		

		public void StartMentalBreakWith(Pawn pawn, MentalBreakDef locBrDef = null)
        {
			if (pawn != null)
			{
                if (locBrDef != null)
				{
					locBrDef.Worker.TryStart(pawn, null, false);
				}
                else
				{
					pawn.mindState.mentalBreaker.TryDoRandomMoodCausedMentalBreak();
				}
			}
		}
		
		public override int GUIChangeArrow
		{
			get
			{
				int result;
				if (this.IsFrozen)
				{
					result = 0;
				}
				else
				{
					result = Math.Sign(this.lastEffectiveDelta);
				}
				return result;
			}
		}
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.fought, "fought", false);
			Scribe_Values.Look<bool>(ref this.foughtSocially, "foughtSocially", false);
			Scribe_Values.Look<int>(ref this.lastFoughtTick, "lastFoughtTick", -99999, false);
		}

		private const float DeltaFactor_InBed = 0.02f;
		private float lastEffectiveDelta;
		public bool fought = false;
		public bool foughtSocially = false;
		private int lastFoughtTick = -99999;
		private  int satisifiedTicks => ((AMAMod.settings.OrkoidFightynessStatisfied * 60) *60).SecondsToTicks();
		public bool IsFighting(Pawn pawn)
		{
			return pawn.CurJob != null && (pawn.CurJob.def == JobDefOf.AttackMelee || pawn.CurJob.def == JobDefOf.AttackStatic /*|| pawn.CurJob.def == JobDefOf.Wait_Combat*/ || pawn.CurJob.def == JobDefOf.PredatorHunt);
		}
	}

	/*
		public void StartSocialFightWith(Pawn t, MentalStateDef locBrDef = null)
        {
			if (t != null)
			{
				if (locBrDef != null && locBrDef != MentalStateDefOf.SocialFighting )
				{
					this.pawn.mindState.mentalStateHandler.TryStartMentalState(locBrDef, null, true, false, null, false);
				}
				else
				this.StartScrap(t);
			}
		}
	*/
}
