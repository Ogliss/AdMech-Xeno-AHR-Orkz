using RimWorld;
using System;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
	// AdeptusMechanicus.JobGiver_Ork_Scrapping
	public class JobGiver_Ork_Scrapping : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.RaceProps.Humanlike && pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return null;
			}
			Pawn otherPawn = ((MentalState_Ork_Scrapping)pawn.MentalState).otherPawn;
			Verb verbToUse;
			if (!InteractionUtility.TryGetRandomVerbForSocialFight(pawn, out verbToUse))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(OrkJobDefOf.OGO_Orkoid_Scrap, otherPawn);
			job.maxNumMeleeAttacks = 1;
			job.verbToUse = verbToUse;
			return job;
		}
	}
}
