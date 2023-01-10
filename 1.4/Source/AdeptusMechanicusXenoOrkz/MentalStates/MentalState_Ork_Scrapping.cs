using System;
using AdeptusMechanicus.ExtensionMethods;
using AlienRace;
using RimWorld;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
    public class MentalState_Ork_Scrapping : MentalState_SocialFighting
	{
		Need_Orkoid_Fightyness pawnFightyness = null;
        Need_Orkoid_Fightyness PawnFightyness
		{
			get
			{
				if (pawnFightyness == null)
				{
					foreach (Need item in pawn?.needs?.needs)
					{
						if (item is Need_Orkoid_Fightyness fightyness)
						{
							pawnFightyness = fightyness;

                        }
					}
				}
				return pawnFightyness;

            }
		}
		
		Need_Orkoid_Fightyness otherFightyness = null;
        Need_Orkoid_Fightyness OtherFightyness
		{
			get
			{
				if (otherFightyness == null)
                {
                    foreach (Need item in otherPawn?.needs?.needs)
                    {
						if (item is Need_Orkoid_Fightyness fightyness)
						{
                            otherFightyness = fightyness;

                        }
					}
				}
				return otherFightyness;

            }
		}

        public new bool ShouldStop
		{
			get
			{
				bool otherSpawned = this.otherPawn.Spawned;
				bool otherDead = this.otherPawn.Dead;
				bool otherDowned = this.otherPawn.Downed;
				bool otherOrk = this.otherPawn.isOrk();
				bool IsOtherPawnSocialFightingWithMe = this.IsOtherPawnSocialFightingWithMe;
				bool result = !otherSpawned || otherDead || otherDowned || (!IsOtherPawnSocialFightingWithMe && !otherOrk);

                Log.Message($"ShouldStop::Result {result} :: otherSpawned:: {otherSpawned}, otherDead:: {otherDead}, otherDowned:: {otherDowned}, otherOrk:: {otherOrk}, IsOtherPawnSocialFightingWithMe:: {IsOtherPawnSocialFightingWithMe}");
                return result;
			}
		}

		public new bool IsOtherPawnSocialFightingWithMe
		{
			get
			{
				if (!this.otherPawn.InMentalState)
				{
					return false;
				}
				MentalState_SocialFighting mentalState_SocialFighting = this.otherPawn.MentalState as MentalState_SocialFighting;
				return mentalState_SocialFighting != null && mentalState_SocialFighting.otherPawn == this.pawn;
			}
		}

		public override void MentalStateTick()
		{
			if (this.ShouldStop)
			{
				base.RecoverFromState();
				return;
            }
            if (this.pawn.IsHashIntervalTick(30))
            {
                this.age += 30;
                if (this.age >= this.def.maxTicksBeforeRecovery || (this.age >= this.def.minTicksBeforeRecovery && this.CanEndBeforeMaxDurationNow && Rand.MTBEventOccurs(this.def.recoveryMtbDays, 60000f, 30f)) || (this.forceRecoverAfterTicks != -1 && this.age >= this.forceRecoverAfterTicks))
                {
                    this.RecoverFromState();
                    return;
                }
                if (this.def.recoverFromSleep && !this.pawn.Awake())
                {
                    this.RecoverFromState();
                    return;
                }
            }
        }

		public override void PostEnd()
		{
			if (!this.def.recoveryMessage.NullOrEmpty() && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				TaggedString taggedString = this.def.recoveryMessage.Formatted(this.pawn.LabelShort, this.pawn.Named("PAWN"));
				if (!taggedString.NullOrEmpty())
				{
					Messages.Message(taggedString.AdjustedFor(this.pawn, "PAWN", true).CapitalizeFirst(), this.pawn, MessageTypeDefOf.SituationResolved, true);
				}
			}
			this.pawn.jobs.StopAll(false, true);
			this.pawn.mindState.meleeThreat = null;
            if (this.otherPawn.Dead || this.otherPawn.Downed || this.otherPawn.MentalState is MentalState_Ork_Scrapping scrapping && this.damageDone > scrapping.damageDone)
            {
                won = true;
            }
            if (this.IsOtherPawnSocialFightingWithMe)
			{
				this.otherPawn.MentalState.RecoverFromState();
			}
			if ((PawnUtility.ShouldSendNotificationAbout(this.pawn) || PawnUtility.ShouldSendNotificationAbout(this.otherPawn)) && this.pawn.thingIDNumber < this.otherPawn.thingIDNumber)
			{
				Messages.Message("AdeptusMechanicus.Ork.MessageNoLongerSocialFighting".Translate(this.pawn.LabelShort, this.otherPawn.LabelShort, this.pawn.Named("PAWN1"), this.otherPawn.Named("PAWN2")), this.pawn, MessageTypeDefOf.SituationResolved, true);
			}
			if (!this.pawn.Dead && this.pawn.needs.mood != null && otherPawn.RaceProps.Humanlike /* && !this.otherPawn.Dead */)
			{
				ThoughtDef def;
				if (pawn.isOrk() && this.won)
				{
					def = otherPawn.Downed ? OrkoidThoughtDefOf.OG_Ork_WonScrap_Downed : otherPawn.Dead ? OrkoidThoughtDefOf.OG_Ork_WonScrap_Killed : OrkoidThoughtDefOf.OG_Ork_WonScrap; 
					pawn.ageTracker.growth += damageDone * (0.05f * (int)otherPawn.Orkiness());
                }
				else
				{
					def = OrkoidThoughtDefOf.OG_Ork_LostScrap;
				}
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(def, this.otherPawn);
			}
		}
		public void Damage(float dmg)
		{
			damageDone+=dmg;
		}
		bool won = false;
		public bool instigator = false;
		float damageDone = 0f;
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref this.damageDone, "damageDone", 0f);
			Scribe_Values.Look(ref this.instigator, "instigator", false);
		}
	}
}
