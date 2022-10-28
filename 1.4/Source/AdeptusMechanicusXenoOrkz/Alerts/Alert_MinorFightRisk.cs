using AdeptusMechanicus.settings;
using RimWorld;
using System;
using Verse;

namespace AdeptusMechanicus
{
	public class Alert_MinorFightRisk : Alert
	{
		public Alert_MinorFightRisk()
		{
			this.defaultPriority = AlertPriority.High;
		}

		public override string GetLabel()
		{
			return FightRiskAlertUtility.AlertLabel;
		}

		public override TaggedString GetExplanation()
		{
			return FightRiskAlertUtility.AlertExplanation;
		}

		public override AlertReport GetReport()
		{
			if (!AMAMod.settings.OrkoidFightyness || FightRiskAlertUtility.PawnsAtRiskExtreme.Any<Pawn>() || FightRiskAlertUtility.PawnsAtRiskMajor.Any<Pawn>())
			{
				return false;
			}
			return AlertReport.CulpritsAre(FightRiskAlertUtility.PawnsAtRiskMinor);
		}
	}
}
