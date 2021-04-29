using RimWorld;
using System;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
	// Token: 0x020006B6 RID: 1718
	public abstract class JobGiver_Binge : ThinkNode_JobGiver
	{
		// Token: 0x06002EF1 RID: 12017 RVA: 0x001087AB File Offset: 0x001069AB
		protected bool IgnoreForbid(Pawn pawn)
		{
			return pawn.InMentalState;
		}

		// Token: 0x06002EF2 RID: 12018
		protected abstract int IngestInterval(Pawn pawn);

		// Token: 0x06002EF3 RID: 12019 RVA: 0x001087B4 File Offset: 0x001069B4
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Find.TickManager.TicksGame - pawn.mindState.lastIngestTick > this.IngestInterval(pawn))
			{
				Job job = this.IngestJob(pawn);
				if (job != null)
				{
					return job;
				}
			}
			return null;
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x001087F0 File Offset: 0x001069F0
		private Job IngestJob(Pawn pawn)
		{
			Thing thing = this.BestIngestTarget(pawn);
			if (thing == null)
			{
				return null;
			}
			ThingDef finalIngestibleDef = FoodUtility.GetFinalIngestibleDef(thing, false);
			Job job = JobMaker.MakeJob(JobDefOf.Ingest, thing);
			job.count = finalIngestibleDef.ingestible.maxNumToIngestAtOnce;
			job.ignoreForbidden = this.IgnoreForbid(pawn);
			job.overeat = true;
			return job;
		}

		// Token: 0x06002EF5 RID: 12021
		protected abstract Thing BestIngestTarget(Pawn pawn);
	}
}
