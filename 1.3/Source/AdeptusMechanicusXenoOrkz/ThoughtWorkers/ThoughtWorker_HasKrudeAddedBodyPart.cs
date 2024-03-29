﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace AdeptusMechanicus
{
    public class ThoughtWorker_HasKrudeAddedBodyPart : ThoughtWorker
    {
        public override ThoughtState CurrentStateInternal(Pawn p)
        {
            //Make Painstoppers being able to nullify the thought. 
            if (p.health?.hediffSet.hediffs.Any(hediff => hediff.CurStage?.painFactor == 0f) ?? false)
            {
                return false;
            }

            int num = CountCrudeAddedParts(p.health.hediffSet);
            ThoughtState result;
            if (num != -1)
            {
                result = ThoughtState.ActiveAtStage(num);
            }
            else
            {
                result = false;
            }

            return result;
        }

        public static int CountCrudeAddedParts(HediffSet hs)
        {
            int num = -1;
            List<Hediff> hediffs = hs.hediffs;
            for (int i = 0; i < hediffs.Count; i++)
            {
                if (hediffs[i] is Hediff_AddedPart addedPart && addedPart.def.defName.Contains("OG_KrudeBionik"))
                {
                    num++;
                }
            }
            return num;
        }
    }
    public class HediffCompProperties_CrudeAddedPart : HediffCompProperties
    {
        public HediffCompProperties_CrudeAddedPart()
        {
            compClass = typeof(HediffComp_Null);
        }
    }
    public class HediffComp_Null : HediffComp
    {

    }
}
