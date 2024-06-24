using RimWorld;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.MentalState_StarvingOrkoid
    public class MentalState_StarvingOrkoid : MentalState
    {
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look<Pawn>(ref this.target, "target", false);
        }

        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Off;
        }

        public override void PreStart()
        {
            base.PreStart();
            this.TryFindNewTarget();
        }

        public override void MentalStateTick()
        {
            base.MentalStateTick();
            if (this.target != null && this.target.Dead)
            {
                base.RecoverFromState();
            }
            if (this.pawn.IsHashIntervalTick(120) && !this.IsTargetStillValidAndReachable())
            {
                if (!this.TryFindNewTarget())
                {
                    base.RecoverFromState();
                    return;
                }
                Messages.Message("MessageMurderousRageChangedTarget".Translate(this.pawn.LabelShort, this.target.Label, this.pawn.Named("PAWN"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NegativeEvent, true);
                base.MentalStateTick();
            }
        }

        public override TaggedString GetBeginLetterText()
        {
            if (this.target == null)
            {
                Log.Error("No target. This should have been checked in this mental state's worker.");
                return "";
            }
            return this.def.beginLetter.Formatted(this.pawn.NameShortColored.Resolve(), this.target.NameShortColored.Resolve(), this.pawn.Named("PAWN"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true).CapitalizeFirst();
        }

        private bool TryFindNewTarget()
        {
            this.target = StarvingOrkoidMentalStateUtility.FindPawnToEat(this.pawn);
            return this.target != null;
        }

        public bool IsTargetStillValidAndReachable()
        {
            return this.target != null && this.target.SpawnedParentOrMe != null && (!(this.target.SpawnedParentOrMe is Pawn) || this.target.SpawnedParentOrMe == this.target) && this.pawn.CanReach(this.target.SpawnedParentOrMe, PathEndMode.Touch, Danger.Deadly, true, true, TraverseMode.ByPawn);
        }

        public Pawn target;

        private const int NoLongerValidTargetCheckInterval = 120;
    }
}

