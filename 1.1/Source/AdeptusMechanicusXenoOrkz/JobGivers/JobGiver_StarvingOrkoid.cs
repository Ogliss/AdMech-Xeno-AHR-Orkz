using RimWorld;
using System;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.JobGiver_StarvingOrkoid
    public class JobGiver_StarvingOrkoid : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_StarvingOrkoid mentalState_StarvingOrkoid = pawn.MentalState as MentalState_StarvingOrkoid;
			if (mentalState_StarvingOrkoid == null || !mentalState_StarvingOrkoid.IsTargetStillValidAndReachable())
			{
				return null;
			}
			Thing spawnedParentOrMe = mentalState_StarvingOrkoid.target.SpawnedParentOrMe;
			Job job = JobMaker.MakeJob(OrkJobDefOf.OGO_Orkoid_Hunt, spawnedParentOrMe);
			job.canBash = true;
			job.killIncappedTarget = true;
			if (spawnedParentOrMe != mentalState_StarvingOrkoid.target)
			{
				job.maxNumMeleeAttacks = 2;
			}
			return job;
		}
	}
}
