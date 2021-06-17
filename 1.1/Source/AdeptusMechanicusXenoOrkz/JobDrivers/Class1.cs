using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
	// Token: 0x02000636 RID: 1590
	public class JobDriver_Slaughter : JobDriver
	{
		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06002BE8 RID: 11240 RVA: 0x000FF61B File Offset: 0x000FD81B
		protected Pawn Victim
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x000FF82E File Offset: 0x000FDA2E
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x000FF850 File Offset: 0x000FDA50
		public override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOnThingMissingDesignation(TargetIndex.A, DesignationDefOf.Slaughter);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.WaitWith(TargetIndex.A, 180, true, false);
			yield return Toils_General.Do(delegate
			{
				ExecutionUtility.DoExecutionByCut(this.pawn, this.Victim);
				this.pawn.records.Increment(RecordDefOf.AnimalsSlaughtered);
				if (this.pawn.InMentalState)
				{
					this.pawn.MentalState.Notify_SlaughteredAnimal();
				}
			});
			yield break;
		}

		// Token: 0x04001A0E RID: 6670
		public const int SlaughterDuration = 180;
	}
}
