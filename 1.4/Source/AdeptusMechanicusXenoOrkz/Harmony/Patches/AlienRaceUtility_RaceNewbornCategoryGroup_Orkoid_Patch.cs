using Verse;
using HarmonyLib;
using AdeptusMechanicus.ExtensionMethods;
using RimWorld;
using System.Collections.Generic;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(AlienRaceUtility), "RaceNewbornCategoryGroup")]
    public static class AlienRaceUtility_RaceNewbornCategoryGroup_Orkoid_Patch
    {
        [HarmonyPostfix]
        public static BackstoryCategoryFilter Postfix(BackstoryCategoryFilter __result, Pawn pawn)
        {
            if (pawn != null && pawn.RaceProps.Humanlike && pawn.isOrkoid())
            {
            //    if (AMAMod.Dev) Log.Message($"{pawn.Name} Using Orkoid AgeSkillMaxFactorCurve");
                return NewbornCategoryGroup;
            }
            return __result;
        }

        private static readonly BackstoryCategoryFilter NewbornCategoryGroup = new BackstoryCategoryFilter
        {
            categories = new List<string>
            {
                "Newborn_Orkoid"
            },
            commonality = 1f
        };

    }

    [HarmonyPatch(typeof(AlienRaceUtility), "RaceChildCategoryGroup")]
    public static class AlienRaceUtility_RaceChildCategoryGroup_Orkoid_Patch
    {
        [HarmonyPostfix]
        public static BackstoryCategoryFilter Postfix(BackstoryCategoryFilter __result, Pawn pawn)
        {
            if (pawn != null && pawn.RaceProps.Humanlike && pawn.isOrkoid())
            {
            //    if (AMAMod.Dev) Log.Message($"{pawn.Name} Using Orkoid AgeSkillMaxFactorCurve");
                return ChildCategoryGroup;
            }
            return __result;
        }

        private static readonly BackstoryCategoryFilter ChildCategoryGroup = new BackstoryCategoryFilter
        {
            categories = new List<string>
            {
                "Child_Orkoid",
                "ChildTribal_Orkoid"
            },
            commonality = 1f
        };

    }

    [HarmonyPatch(typeof(AlienRaceUtility), "RaceAdultCategoryGroup")]
    public static class AlienRaceUtility_RaceAdultCategoryGroup_Orkoid_Patch
    {
        [HarmonyPostfix]
        public static BackstoryCategoryFilter Postfix(BackstoryCategoryFilter __result, Pawn pawn)
        {
            if (pawn != null && pawn.RaceProps.Humanlike && pawn.isOrkoid())
            {
            //    if (AMAMod.Dev) Log.Message($"{pawn.Name} Using Orkoid AgeSkillMaxFactorCurve");
                return AdultCategoryGroup;
            }
            return __result;
        }

        private static readonly BackstoryCategoryFilter AdultCategoryGroup = new BackstoryCategoryFilter
        {
            categories = new List<string>
            {
                "AdultColonist_Orkoid",
                "AdultTribal_Orkoid"
            },
            commonality = 1f
        };

    }
}
