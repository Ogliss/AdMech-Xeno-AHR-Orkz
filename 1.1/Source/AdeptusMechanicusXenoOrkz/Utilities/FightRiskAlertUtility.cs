using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace AdeptusMechanicus
{
    public static class FightRiskAlertUtility
	{
		public static List<Pawn> PawnsAtRiskExtreme
		{
			get
			{
				FightRiskAlertUtility.pawnsAtRiskExtremeResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep)
				{
					if (!pawn.Downed && pawn.needs.TryGetNeed(AdeptusNeedDefOf.OG_Ork_Fightyness) is Need_Orkoid_Fightyness need_Violence && need_Violence.CurCategory <= FightynessCategory.Requires)
					{
						FightRiskAlertUtility.pawnsAtRiskExtremeResult.Add(pawn);
					}
				}
				return FightRiskAlertUtility.pawnsAtRiskExtremeResult;
			}
		}

		public static List<Pawn> PawnsAtRiskMajor
		{
			get
			{
				FightRiskAlertUtility.pawnsAtRiskMajorResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep)
				{
					if (!pawn.Downed && pawn.needs.TryGetNeed(AdeptusNeedDefOf.OG_Ork_Fightyness) is Need_Orkoid_Fightyness need_Violence && need_Violence.CurCategory <= FightynessCategory.Desires)
					{
						FightRiskAlertUtility.pawnsAtRiskMajorResult.Add(pawn);
					}
				}
				return FightRiskAlertUtility.pawnsAtRiskMajorResult;
			}
		}

		public static List<Pawn> PawnsAtRiskMinor
		{
			get
			{
				FightRiskAlertUtility.pawnsAtRiskMinorResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep)
				{
					if (!pawn.Downed && pawn.needs.TryGetNeed(AdeptusNeedDefOf.OG_Ork_Fightyness) is Need_Orkoid_Fightyness need_Violence && need_Violence.CurCategory < FightynessCategory.Free)
					{
						FightRiskAlertUtility.pawnsAtRiskMinorResult.Add(pawn);
					}
				}
				return FightRiskAlertUtility.pawnsAtRiskMinorResult;
			}
		}

		public static string AlertLabel
		{
			get
			{
				int num = FightRiskAlertUtility.PawnsAtRiskExtreme.Count<Pawn>();
				string text;
				if (num > 0)
				{
					text = "AdeptusMechanicus.Ork.FightRiskExtreme".Translate();
				}
				else
				{
					num = FightRiskAlertUtility.PawnsAtRiskMajor.Count<Pawn>();
					if (num > 0)
					{
						text = "AdeptusMechanicus.Ork.FightRiskMajor".Translate();
					}
					else
					{
						num = FightRiskAlertUtility.PawnsAtRiskMinor.Count<Pawn>();
						text = "AdeptusMechanicus.Ork.FightRiskMinor".Translate();
					}
				}
				if (num > 1)
				{
					text = text + " x" + num.ToStringCached();
				}
				return text;
			}
		}

		public static string AlertExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (FightRiskAlertUtility.PawnsAtRiskExtreme.Any<Pawn>())
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					foreach (Pawn pawn in FightRiskAlertUtility.PawnsAtRiskExtreme)
					{
						stringBuilder2.AppendLine("  - " + pawn.NameShortColored.Resolve());
					}
					stringBuilder.Append("AdeptusMechanicus.Ork.FightRiskExtremeDesc".Translate(stringBuilder2).Resolve());
				}
				if (FightRiskAlertUtility.PawnsAtRiskMajor.Any<Pawn>())
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					StringBuilder stringBuilder3 = new StringBuilder();
					foreach (Pawn pawn2 in FightRiskAlertUtility.PawnsAtRiskMajor)
					{
						stringBuilder3.AppendLine("  - " + pawn2.NameShortColored.Resolve());
					}
					stringBuilder.Append("AdeptusMechanicus.Ork.FightRiskMajorDesc".Translate(stringBuilder3).Resolve());
				}
				if (FightRiskAlertUtility.PawnsAtRiskMinor.Any<Pawn>())
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					StringBuilder stringBuilder4 = new StringBuilder();
					foreach (Pawn pawn3 in FightRiskAlertUtility.PawnsAtRiskMinor)
					{
						stringBuilder4.AppendLine("  - " + pawn3.NameShortColored.Resolve());
					}
					stringBuilder.Append("AdeptusMechanicus.Ork.FightRiskMinorDesc".Translate(stringBuilder4).Resolve());
				}
				stringBuilder.AppendLine();
				stringBuilder.Append("AdeptusMechanicus.Ork.FightRiskDescEnding".Translate());
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06005B1F RID: 23327 RVA: 0x001E4880 File Offset: 0x001E2A80
		public static void Clear()
		{
			FightRiskAlertUtility.pawnsAtRiskExtremeResult.Clear();
			FightRiskAlertUtility.pawnsAtRiskMajorResult.Clear();
			FightRiskAlertUtility.pawnsAtRiskMinorResult.Clear();
		}

		// Token: 0x040031E8 RID: 12776
		private static List<Pawn> pawnsAtRiskExtremeResult = new List<Pawn>();

		// Token: 0x040031E9 RID: 12777
		private static List<Pawn> pawnsAtRiskMajorResult = new List<Pawn>();

		// Token: 0x040031EA RID: 12778
		private static List<Pawn> pawnsAtRiskMinorResult = new List<Pawn>();
	}
}
