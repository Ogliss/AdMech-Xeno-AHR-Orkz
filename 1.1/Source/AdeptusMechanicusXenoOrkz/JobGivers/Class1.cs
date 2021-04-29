using RimWorld;
using System;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
	// Token: 0x0200071F RID: 1823
	public class JobGiver_Berserk : ThinkNode_JobGiver
	{
		// Token: 0x06003090 RID: 12432 RVA: 0x00111B38 File Offset: 0x0010FD38
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Rand.Value < 0.5f)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Wait_Combat);
				job.expiryInterval = 90;
				job.canUseRangedWeapon = false;
				return job;
			}
			if (pawn.TryGetAttackVerb(null, false) == null)
			{
				return null;
			}
			Pawn pawn2 = this.FindPawnTarget(pawn);
			if (pawn2 != null)
			{
				Job job2 = JobMaker.MakeJob(JobDefOf.AttackMelee, pawn2);
				job2.maxNumMeleeAttacks = 1;
				job2.expiryInterval = Rand.Range(420, 900);
				job2.canBash = true;
				return job2;
			}
			return null;
		}

		// Token: 0x06003091 RID: 12433 RVA: 0x00111BB8 File Offset: 0x0010FDB8
		private Pawn FindPawnTarget(Pawn pawn)
		{
			return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedReachable, delegate (Thing x)
			{
				Pawn pawn2;
				return (pawn2 = (x as Pawn)) != null && pawn2.Spawned && !pawn2.Downed && !pawn2.IsInvisible();
			}, 0f, 40f, default(IntVec3), float.MaxValue, true, true);
		}

		// Token: 0x04001B55 RID: 6997
		private const float MaxAttackDistance = 40f;

		// Token: 0x04001B56 RID: 6998
		private const float WaitChance = 0.5f;

		// Token: 0x04001B57 RID: 6999
		private const int WaitTicks = 90;

		// Token: 0x04001B58 RID: 7000
		private const int MinMeleeChaseTicks = 420;

		// Token: 0x04001B59 RID: 7001
		private const int MaxMeleeChaseTicks = 900;
	}
}
