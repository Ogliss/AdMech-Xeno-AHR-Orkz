using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using HarmonyLib;
using Verse.Sound;
using AdeptusMechanicus;
using AdeptusMechanicus.ExtensionMethods;
using AdeptusMechanicus.settings;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(AlienRaceUtility), "RaceAgeSkillMaxFactorCurve")]
    public static class AlienRaceUtility_RaceAgeSkillMaxFactorCurve_Orkoid_Patch
    {
        [HarmonyPostfix]
        public static SimpleCurve Postfix(SimpleCurve __result, Pawn pawn)
        {
            if (pawn != null && pawn.RaceProps.Humanlike && pawn.isOrkoid())
            {
                if (AMAMod.Dev) Log.Message($"{pawn.Name} Using Orkoid AgeSkillMaxFactorCurve");
                return AgeSkillMaxFactorCurve;
            }
            
            return __result;
        }
        private static readonly SimpleCurve AgeSkillMaxFactorCurve = new SimpleCurve
        {
            {
                new CurvePoint(0f, 0.7f),
                true
            },
            {
                new CurvePoint(2f, 1.3f),
                true
            },
            {
                new CurvePoint(30f, 1.6f),
                true
            },
            {
                new CurvePoint(50f, 1.9f),
                true
            }
        };
    }
}
