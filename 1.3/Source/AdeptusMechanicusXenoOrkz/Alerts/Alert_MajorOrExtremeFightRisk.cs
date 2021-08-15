using AdeptusMechanicus.settings;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace AdeptusMechanicus
{
    public class Alert_MajorOrExtremeFightRisk : Alert_Critical
	{
		private List<Pawn> Culprits
		{
			get
			{
				this.culpritsResult.Clear();
				this.culpritsResult.AddRange(FightRiskAlertUtility.PawnsAtRiskExtreme);
				this.culpritsResult.AddRange(FightRiskAlertUtility.PawnsAtRiskMajor);
				return this.culpritsResult;
			}
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
            if (!AMAMod.settings.OrkoidFightyness)
            {
				return false;
            }
			return AlertReport.CulpritsAre(this.Culprits);
		}

		private List<Pawn> culpritsResult = new List<Pawn>();
	}
}
