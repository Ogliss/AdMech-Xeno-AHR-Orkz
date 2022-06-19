using RimWorld;
using System;
using Verse;

namespace AdeptusMechanicus
{
	// Token: 0x020006B8 RID: 1720
	public class JobGiver_BingeFood : JobGiver_Binge
	{
		// Token: 0x06002EFD RID: 12029 RVA: 0x00108A3D File Offset: 0x00106C3D
		protected override int IngestInterval(Pawn pawn)
		{
			return 1100;
		}

		// Token: 0x06002EFE RID: 12030 RVA: 0x00108A44 File Offset: 0x00106C44
		protected override Thing BestIngestTarget(Pawn pawn)
		{
			Thing result;
			ThingDef thingDef;
			if (FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, true, out result, out thingDef, false, true, true, true, true, false, false, false, false, FoodPreferability.Undefined))
			{
				return result;
			}
			return null;
		}

		// Token: 0x04001AE0 RID: 6880
		private const int BaseIngestInterval = 1100;
	}
}
