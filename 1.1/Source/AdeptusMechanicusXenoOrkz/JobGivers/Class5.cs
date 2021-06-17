using RimWorld;
using System;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
	// Token: 0x02000728 RID: 1832
	public class JobGiver_SlaughterRandomAnimal : ThinkNode_JobGiver
	{
		// Token: 0x060030A9 RID: 12457 RVA: 0x00112290 File Offset: 0x00110490
		public override Job TryGiveJob(Pawn pawn)
		{
			MentalState_Slaughterer mentalState_Slaughterer = pawn.MentalState as MentalState_Slaughterer;
			if (mentalState_Slaughterer != null && mentalState_Slaughterer.SlaughteredRecently)
			{
				return null;
			}
			Pawn pawn2 = SlaughtererMentalStateUtility.FindAnimal(pawn);
			if (pawn2 == null || !pawn.CanReserveAndReach(pawn2, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Slaughter, pawn2);
			job.ignoreDesignations = true;
			return job;
		}
	}
}
