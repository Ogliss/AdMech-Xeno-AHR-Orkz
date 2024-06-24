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

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(StatPart_Age), "AgeMultiplier")]
    public static class StatPart_Age_AgeMultiplier_Orkoid_Patch
    {
        static StatDef ShootingAccuracyChildFactor = DefDatabase<StatDef>.GetNamedSilentFail("ShootingAccuracyChildFactor");
        [HarmonyPostfix]
        public static float Postfix(float __result, StatPart_Age __instance, Pawn pawn)
        {
            if (__instance.humanlikeOnly && pawn.RaceProps.Humanlike && pawn.isOrkoid() )
            {
                SimpleCurve curve = __instance.curve;
                if (__instance.parentStat == StatDefOf.WorkSpeedGlobal)
                {
                    curve = curveWorkSpeedGlobal;
                }
                else
                if (__instance.parentStat == StatDefOf.MeleeHitChance)
                {
                    curve = curveMeleeHitChance;
                }
                else
                if (__instance.parentStat == StatDefOf.AimingDelayFactor)
                {
                    curve = curveAimingDelayFactor;
                }
                else
                if (ShootingAccuracyChildFactor != null && __instance.parentStat == ShootingAccuracyChildFactor)
                {
                    curve = curveShootingAccuracyChildFactor;
                }
                else
                if (__instance.parentStat == StatDefOf.ArrestSuccessChance)
                {
                    curve = curveArrestSuccessChance;
                }
                else
                if (__instance.parentStat == StatDefOf.MarketValue)
                {
                    curve = curveMarketValue;
                }
                if (!__instance.useBiologicalYears)
                {
                    return curve.Evaluate((float)pawn.ageTracker.AgeBiologicalYearsFloat / pawn.RaceProps.lifeExpectancy);
                }
                return curve.Evaluate((float)pawn.ageTracker.AgeBiologicalYearsFloat);
            }
            return __result;
        }
        private static SimpleCurve curveWorkSpeedGlobal = new SimpleCurve()
        {
            {
                new CurvePoint(0f, 0.2f),
                true
            },
            {
                new CurvePoint(0.1f, 0.4f),
                true
            },
            {
                new CurvePoint(0.4f, 0.6f),
                true
            },
            {
                new CurvePoint(1f, 0.8f),
                true
            },
            {
                new CurvePoint(2f, 1.0f),
                true
            }
        };
        private static SimpleCurve curveMeleeHitChance = new SimpleCurve()
        {
            {
                new CurvePoint(0f, 0.5f),
                true
            },
            {
                new CurvePoint(0.5f, 0.6f),
                true
            },
            {
                new CurvePoint(1f, 0.8f),
                true
            },
            {
                new CurvePoint(2f, 1.0f),
                true
            }
        };
        private static SimpleCurve curveAimingDelayFactor = new SimpleCurve()
        {
            {
                new CurvePoint(0f, 1.5f),
                true
            },
            {
                new CurvePoint(0.5f, 1.25f),
                true
            },
            {
                new CurvePoint(1f, 1.1f),
                true
            },
            {
                new CurvePoint(2f, 1.0f),
                true
            }
        };
        private static SimpleCurve curveShootingAccuracyChildFactor = new SimpleCurve()
        {
            {
                new CurvePoint(0f, 0.75f),
                true
            },
            {
                new CurvePoint(1f, 0.95f),
                true
            },
            {
                new CurvePoint(2f, 1.0f),
                true
            }
        };
        private static SimpleCurve curveArrestSuccessChance = new SimpleCurve()
        {
            {
                new CurvePoint(0f, 0.05f),
                true
            },
            {
                new CurvePoint(1f, 0.8f),
                true
            },
            {
                new CurvePoint(2f, 1.0f),
                true
            }
        };
        private static SimpleCurve curveMarketValue = new SimpleCurve()
        {
            {
                new CurvePoint(0f, 0.5f),
                true
            },
            {
                new CurvePoint(1f, 0.9f),
                true
            },
            {
                new CurvePoint(2f, 1.0f),
                true
            }
        };
    }
}
