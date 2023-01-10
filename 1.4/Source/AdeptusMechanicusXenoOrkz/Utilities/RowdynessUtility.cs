using AdeptusMechanicus.ExtensionMethods;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
    public static class RowdynessUtility
	{
		public static void StartScrap(Pawn pawn, Pawn otherPawn)
		{
			if (PawnUtility.ShouldSendNotificationAbout(pawn) || PawnUtility.ShouldSendNotificationAbout(otherPawn))
			{
				Messages.Message("AdeptusMechanicus.Ork.MessageSocialFight".Translate(pawn.LabelShort, otherPawn.LabelShort, pawn.Named("PAWN1"), otherPawn.Named("PAWN2")), pawn, MessageTypeDefOf.ThreatSmall, true);
			}
			pawn.mindState.mentalStateHandler.TryStartMentalState(AdeptusMentalStateDefOf.OGO_Ork_Scrapping, null, false, false, otherPawn, false);
			if (pawn.MentalState is MentalState_Ork_Scrapping state) state.instigator = true;
            if (otherPawn.isOrk())
			{
				otherPawn.mindState.mentalStateHandler.TryStartMentalState(AdeptusMentalStateDefOf.OGO_Ork_Scrapping, null, false, false, pawn, false);
			}
			else
			{
				otherPawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.SocialFighting, null, false, false, pawn, false);
			}
			TaleRecorder.RecordTale(TaleDefOf.SocialFight, new object[]
			{
				pawn,
				otherPawn
			});
		}

		public static Pawn FindKrumpablePawnFor(Pawn pawn, RowdynessCategory CurCategory, Orkoid max = Orkoid.Warboss)
		{
			List<Pawn> collection = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			workingList.Clear();
			//	float maxSize = pawn.BodySize + (pawn.BodySize * (1 - 1 * (int)CurCategory));
			if (!collection.NullOrEmpty())
			{
				workingList.AddRange(collection);
				workingList.Shuffle<Pawn>();
				workingList.RemoveAll(x => x == pawn || x.Orkiness() > max || x.Downed || !x.Awake());
				//	workingList.RemoveAll(x => x.BodySize > maxSize);
				workingList.RemoveAll(x => !pawn.CanReach(x, PathEndMode.Touch, Danger.Deadly));
			}
			if (workingList.NullOrEmpty())
			{
				return null;
			}
			return workingList.RandomElementByWeight(x => AdeptusMath.Inverse(x.Position.DistanceTo(pawn.Position)));
		}
		public static Pawn FindMunchablePawnFor(Pawn pawn, RowdynessCategory CurCategory, Orkoid max = Orkoid.Grot)
		{
			List<Pawn> collection = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			workingList.Clear();
			//	float maxSize = pawn.BodySize + (pawn.BodySize * (1 - 1 * (int)CurCategory));
			if (!collection.NullOrEmpty())
			{
				workingList.AddRange(collection);
				workingList.Shuffle<Pawn>();
				workingList.RemoveAll(x => x == pawn || x.Orkiness() > max);
				workingList.RemoveAll(x => !pawn.CanReach(x, PathEndMode.Touch, Danger.Deadly));
			}
			if (workingList.NullOrEmpty())
			{
				return null;
			}
			return workingList.RandomElementByWeight(x => AdeptusMath.Inverse(x.Position.DistanceTo(pawn.Position)));
		}
		private static List<Pawn> workingList = new List<Pawn>();

		public static bool GetsRowdy(Pawn pawn, float CurLevelPercentage)
		{
			Rand.PopState();
			bool result = Rand.Chance((CurLevelPercentage / 100f) * pawn.health.summaryHealth.SummaryHealthPercent);
			Rand.PushState();
			return result;
		}

	}
}
