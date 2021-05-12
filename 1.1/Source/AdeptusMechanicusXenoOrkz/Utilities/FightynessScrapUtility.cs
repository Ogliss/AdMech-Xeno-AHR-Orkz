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
    public static class FightynessScrapUtility
	{
		public static void StartScrap(Pawn pawn, Pawn otherPawn)
		{
			if (PawnUtility.ShouldSendNotificationAbout(pawn) || PawnUtility.ShouldSendNotificationAbout(otherPawn))
			{
				Messages.Message("AMO_MessageSocialFight".Translate(pawn.LabelShort, otherPawn.LabelShort, pawn.Named("PAWN1"), otherPawn.Named("PAWN2")), pawn, MessageTypeDefOf.ThreatSmall, true);
			}
			pawn.mindState.mentalStateHandler.TryStartMentalState(AdeptusMentalStateDefOf.OGO_Ork_Scrapping, null, false, false, otherPawn, false);
			if (otherPawn.isOrk())
			{
				otherPawn.mindState.mentalStateHandler.TryStartMentalState(AdeptusMentalStateDefOf.OGO_Ork_Scrapping, null, false, false, pawn, false);
			}
			else
			{

			}
			TaleRecorder.RecordTale(TaleDefOf.SocialFight, new object[]
			{
				pawn,
				otherPawn
			});
		}

		public static Pawn FindKrumpablePawn(Pawn pawn, FightynessCategory CurCategory, Orkoid max = Orkoid.GargantuanSquiggoth)
		{
			List<Pawn> collection = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			float maxSize = pawn.BodySize + (pawn.BodySize * (1 * (int)CurCategory));
			workingList.Clear();
			workingList.AddRange(collection);
			workingList.Shuffle<Pawn>();
			workingList.RemoveAll(x => x == pawn || x.Orkiness() > max);
			workingList.RemoveAll(x => x.BodySize > maxSize);
			workingList.RemoveAll(x => !pawn.CanReach(x, PathEndMode.Touch, Danger.Deadly));
			if (workingList.NullOrEmpty())
			{
				return null;
			}
			return workingList.RandomElementByWeight(x => AdeptusMath.Inverse(x.Position.DistanceTo(pawn.Position)));
		}
		private static List<Pawn> workingList = new List<Pawn>();

	}
}
