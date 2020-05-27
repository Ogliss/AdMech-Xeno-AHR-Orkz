using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace AdeptusMechanicus
{
    // Token: 0x0200021B RID: 539
    public class ThoughtWorker_HumanlikeLeatherApparel_Ork : ThoughtWorker
    {
        // Token: 0x06000A2A RID: 2602 RVA: 0x0004FCF4 File Offset: 0x0004E0F4
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            string text = null;
            int num = 0;
            List<Apparel> wornApparel = p.apparel.WornApparel;
            for (int i = 0; i < wornApparel.Count; i++)
            {
                if ( wornApparel[i].Stuff.defName.Contains("Alien_Tau") || wornApparel[i].Stuff.defName.Contains("Alien_Kroot"))
                {
                    if (text == null)
                    {
                        text = wornApparel[i].def.label;
                    }
                    num++;
                }
            }
            if (num == 0)
            {
                return ThoughtState.Inactive;
            }
            if (num >= 5)
            {
                return ThoughtState.ActiveAtStage(4, text);
            }
            return ThoughtState.ActiveAtStage(num - 1, text);
        }
    }
}
