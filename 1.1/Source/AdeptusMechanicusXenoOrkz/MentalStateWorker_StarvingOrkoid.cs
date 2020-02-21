using System;

namespace Verse.AI
{
    // Token: 0x0200055C RID: 1372
    public class MentalStateWorker_StarvingOrkoid : MentalStateWorker
    {
        // Token: 0x0600267F RID: 9855 RVA: 0x000DF9B5 File Offset: 0x000DDBB5
        public override bool StateCanOccur(Pawn pawn)
        {
            return base.StateCanOccur(pawn) && StarvingOrkoidMentalStateUtility.FindPawnToKill(pawn) != null && pawn.needs.food.Starving;
        }
    }
}
