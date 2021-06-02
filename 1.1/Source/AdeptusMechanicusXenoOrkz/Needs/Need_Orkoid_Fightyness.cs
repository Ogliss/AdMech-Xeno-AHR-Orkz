using System;
using System.Collections.Generic;
using System.Linq;
using AdeptusMechanicus.ExtensionMethods;
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
		private float Wants => 0.15f;
		private float Desires => 0.3f;
		private float Craves => 0.5f;
		private float Requires => 0.7f;
		private float Obsessed => 0.9f;
		public FightynessCategory CurCategory
		{
			get
			{
				if (base.CurLevelPercentage < Wants) // 0.0500000007450581
				{
					return FightynessCategory.Free;
				}
				if (base.CurLevelPercentage < Desires) // 0.200000002980232
				{
					return FightynessCategory.Wants;
				}
				if (base.CurLevelPercentage < Craves) // 0.400000005960464
				{
					return FightynessCategory.Desires;
				}
				if (base.CurLevelPercentage < Requires) // 0.600000023841858
				{
					return FightynessCategory.Craves;
				}
				if (base.CurLevelPercentage > Obsessed) // 0.800000011920929
				{
					return FightynessCategory.Requires;
				}
				return FightynessCategory.Obsessed;
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
					result = this.pawn.WorkTagIsDisabled(WorkTags.Violent);
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
					float b = 0.2f;
					float num;
					if (this.pawn.Spawned)
					{
						num = FightynessRate;
						b = 0f;
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
					if (this.fought)
					{
						num2 = foughtSocially ? -1f : -0.25f;
                        if (TicksSatisfied > 15000)
						{
							this.fought = false;
						}
					}
					float curLevel = this.CurLevel;
					if ((double)num2 < 0.0)
					{
						this.CurLevel = Mathf.Min(this.CurLevel, Mathf.Max(this.CurLevel + num2, b));
					}
					else
					{
						this.CurLevel = Mathf.Min(this.CurLevel + num2, 1f);
					}
					this.lastEffectiveDelta = this.CurLevel - curLevel;
					if (pawn.isOrk() && CurCategory != FightynessCategory.Free && Math.Sign(this.lastEffectiveDelta) > 0)
					{
                        Rand.PushState();
					//	Log.Message(pawn.NameShortColored + " will start a fight "+ this.CurLevelPercentage +"%");
						if (Rand.Chance(this.CurLevelPercentage/100f))
						{
							Orkoid maxOrkiness;
							int val1 = (int)pawn.Orkiness() + (int)CurCategory;
							switch (CurCategory)
							{
								case FightynessCategory.Obsessed:
									maxOrkiness = (Orkoid)Math.Min(val1, 9);
									break;
								case FightynessCategory.Requires:
									maxOrkiness = (Orkoid)Math.Min(val1, 8);
									break;
								case FightynessCategory.Craves:
                                    maxOrkiness = (Orkoid)Math.Min(val1, 6);
									break;
								case FightynessCategory.Desires:
									maxOrkiness = Orkoid.Ork;
									break;
								default:
									maxOrkiness = Orkoid.Snotling;
									break;
							}
							Log.Message("if it can find something to krump.....");
							Pawn t = FightynessScrapUtility.FindKrumpablePawn(pawn, this.CurCategory, maxOrkiness);
							if (t != null)
							{
								Log.Message("gonna try and krump "+t.NameShortColored);
								FightynessScrapUtility.StartScrap(pawn ,t);
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
				return Mathf.Max(0, Find.TickManager.TicksGame - pawn.mindState.lastAttackTargetTick);
			}
		}

		private float FightynessRate
		{
			get
			{
				return this.pawn.BodySize * (1f - (1f * (this.pawn.needs.joy != null ? this.pawn.needs.joy.CurLevel : 0f)));
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

		private const float DeltaFactor_InBed = 0.2f;
		private float lastEffectiveDelta;
		public bool fought = false;
		public bool foughtSocially = false;
		private int lastFoughtTick = -99999;
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
