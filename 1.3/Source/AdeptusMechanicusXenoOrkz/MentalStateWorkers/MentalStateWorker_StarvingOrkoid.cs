using AdeptusMechanicus.ExtensionMethods;
using System;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.MentalStateWorker_StarvingOrkoid
    public class MentalStateWorker_StarvingOrkoid : MentalStateWorker
    {
        public override bool StateCanOccur(Pawn pawn)
        {

            return base.StateCanOccur(pawn) && pawn.isOrkoid() && StarvingOrkoidMentalStateUtility.FindPawnToEat(pawn) != null && pawn.needs.food.Starving;
        }
    }
}
