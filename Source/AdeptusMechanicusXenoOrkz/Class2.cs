using System;
using RimWorld;

namespace Verse.AI
{
    // Token: 0x02000AAA RID: 2730
    public class MentalState_StarvingOrkoid : MentalState
    {
        // Token: 0x17000948 RID: 2376
        // (get) Token: 0x06003CEC RID: 15596 RVA: 0x001CAB48 File Offset: 0x001C8F48
        public bool SlaughteredRecently
        {
            get
            {
                return this.lastSlaughterTicks >= 0 && Find.TickManager.TicksGame - this.lastSlaughterTicks < 3750;
            }
        }

        // Token: 0x17000949 RID: 2377
        // (get) Token: 0x06003CED RID: 15597 RVA: 0x001CAB71 File Offset: 0x001C8F71
        protected override bool CanEndBeforeMaxDurationNow
        {
            get
            {
                return this.lastSlaughterTicks >= 0;
            }
        }

        // Token: 0x06003CEE RID: 15598 RVA: 0x001CAB7F File Offset: 0x001C8F7F
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.lastSlaughterTicks, "lastSlaughterTicks", 0, false);
            Scribe_Values.Look<int>(ref this.animalsSlaughtered, "animalsSlaughtered", 0, false);
        }

        // Token: 0x06003CEF RID: 15599 RVA: 0x001CABAC File Offset: 0x001C8FAC
        public override void MentalStateTick()
        {
            base.MentalStateTick();
            if (this.pawn.IsHashIntervalTick(600) && (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.Slaughter) && SlaughtererMentalStateUtility.FindAnimal(this.pawn) == null)
            {
                base.RecoverFromState();
            }
        }

        // Token: 0x06003CF0 RID: 15600 RVA: 0x001CAC14 File Offset: 0x001C9014
        public override void Notify_SlaughteredAnimal()
        {
            this.lastSlaughterTicks = Find.TickManager.TicksGame;
            this.animalsSlaughtered++;
            if (this.animalsSlaughtered >= 4)
            {
                base.RecoverFromState();
            }
        }
        

        // Token: 0x040026AA RID: 9898
        private int lastSlaughterTicks = -1;

        // Token: 0x040026AB RID: 9899
        private int animalsSlaughtered;

        // Token: 0x040026AC RID: 9900
        private const int NoAnimalToSlaughterCheckInterval = 600;

        // Token: 0x040026AD RID: 9901
        private const int MinTicksBetweenSlaughter = 3750;

        // Token: 0x040026AE RID: 9902
        private const int MaxAnimalsSlaughtered = 4;
    }
}
