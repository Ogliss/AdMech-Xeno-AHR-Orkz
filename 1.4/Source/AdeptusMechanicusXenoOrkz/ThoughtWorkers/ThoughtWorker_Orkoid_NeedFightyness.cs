using System;
using RimWorld;
using Verse;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.ThoughtWorker_Orkoid_NeedFightyness
    public class ThoughtWorker_Orkoid_NeedFightyness : ThoughtWorker
	{
		public override ThoughtState CurrentStateInternal(Pawn p)
		{
			Need_Orkoid_Fightyness need_Violence = (Need_Orkoid_Fightyness)p.needs.AllNeeds.Find((Need x) => x.def == AdeptusNeedDefOf.OG_Ork_Fightyness);
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
					case RowdynessCategory.Obsessed:
						result = ThoughtState.ActiveAtStage(0);
						break;
					case RowdynessCategory.Requires:
						result = ThoughtState.ActiveAtStage(1);
						break;
					case RowdynessCategory.Craves:
						result = ThoughtState.ActiveAtStage(2);
						break;
					case RowdynessCategory.Desires:
						result = ThoughtState.ActiveAtStage(3);
						break;
					case RowdynessCategory.Wants:
						result = ThoughtState.ActiveAtStage(4);
						break;
					case RowdynessCategory.Free:
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
