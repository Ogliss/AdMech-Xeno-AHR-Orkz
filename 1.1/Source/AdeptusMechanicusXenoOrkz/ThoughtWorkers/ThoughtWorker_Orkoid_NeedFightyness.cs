using System;
using RimWorld;
using Verse;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.ThoughtWorker_Orkoid_NeedFightyness
    public class ThoughtWorker_Orkoid_NeedFightyness : ThoughtWorker
	{
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Need_Orkoid_Fightyness need_Violence = (Need_Orkoid_Fightyness)p.needs.AllNeeds.Find((Need x) => x.def == OrkNeedDefOf.OG_Ork_Fightyness);
			bool flag = need_Violence == null || p.HostFaction != null;
			ThoughtState result;
			if (flag)
			{
				result = ThoughtState.Inactive;
			}
			else
			{
				switch (need_Violence.CurCategory)
				{
					case FightynessCategory.Obsessed:
						result = ThoughtState.ActiveAtStage(0);
						break;
					case FightynessCategory.Requires:
						result = ThoughtState.ActiveAtStage(1);
						break;
					case FightynessCategory.Craves:
						result = ThoughtState.ActiveAtStage(2);
						break;
					case FightynessCategory.Desires:
						result = ThoughtState.ActiveAtStage(3);
						break;
					case FightynessCategory.Wants:
						result = ThoughtState.ActiveAtStage(4);
						break;
					case FightynessCategory.Free:
						result = ThoughtState.Inactive;
						break;
					default:
						throw new InvalidOperationException("Unknown ViolenceCategory");
				}
			}
			return result;
		}
	}
}
