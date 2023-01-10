using Verse;
using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using AdeptusMechanicus.ExtensionMethods;
using static UnityEngine.ParticleSystem;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(MeditationFocusDef), "EnablingThingsExplanation")]
    public static class MeditationFocusDef_EnablingThingsExplanation_Orkoid_Patch
    {
        // change to transpiler that null checks the chosen backstory of the slot of the pawn, missing adulthood backstory error
        [HarmonyPrefix]
        public static bool Prefix(Pawn pawn, MeditationFocusDef __instance, ref string __result)
        {
            if (pawn.isOrkoid())
            {
                List<string> reasons = new List<string>();
                if (__instance.requiresRoyalTitle && pawn.royalty != null && pawn.royalty.AllTitlesInEffectForReading.Count > 0)
                {
                    RoyalTitle royalTitle = pawn.royalty.AllTitlesInEffectForReading.MaxBy((RoyalTitle t) => t.def.seniority);
                    reasons.Add("MeditationFocusEnabledByTitle".Translate(royalTitle.def.GetLabelCapFor(pawn).Named("TITLE"), royalTitle.faction.Named("FACTION")).Resolve());
                }
                if (pawn.story != null)
                {
                    RimWorld.BackstoryDef adulthood = pawn.story.Adulthood;
                    RimWorld.BackstoryDef childhood = pawn.story.Childhood;
                    if (!__instance.requiresRoyalTitle && __instance.requiredBackstoriesAny.Count == 0)
                    {
                    //    Log.Message("requiresRoyalTitle requiredBackstoriesAny == 0");
                        for (int i = 0; i < __instance.incompatibleBackstoriesAny.Count; i++)
                        {
                            BackstoryCategoryAndSlot backstoryCategoryAndSlot = __instance.incompatibleBackstoriesAny[i];
                            RimWorld.BackstoryDef backstoryDef = ((backstoryCategoryAndSlot.slot == BackstorySlot.Adulthood) ? adulthood : childhood);
                            Log.Message($"{backstoryDef} {backstoryCategoryAndSlot.slot} {backstoryCategoryAndSlot.categoryName}");
                            if (backstoryDef != null && !backstoryDef.spawnCategories.Contains(backstoryCategoryAndSlot.categoryName))
                            {
                                AddBackstoryReason(backstoryCategoryAndSlot.slot, backstoryDef, ref reasons);
                            }
                        //    Log.Message("OK");
                        }
                        for (int j = 0; j < DefDatabase<TraitDef>.AllDefsListForReading.Count; j++)
                        {
                            TraitDef traitDef = DefDatabase<TraitDef>.AllDefsListForReading[j];
                            List<MeditationFocusDef> disallowedMeditationFocusTypes = traitDef.degreeDatas[0].disallowedMeditationFocusTypes;
                            if (disallowedMeditationFocusTypes != null && disallowedMeditationFocusTypes.Contains(__instance))
                            {
                                reasons.Add("MeditationFocusDisabledByTrait".Translate() + ": " + traitDef.degreeDatas[0].GetLabelCapFor(pawn) + ".");
                            }
                        }
                    }
                //    Log.Message("requiredBackstoriesAny");
                    for (int k = 0; k < __instance.requiredBackstoriesAny.Count; k++)
                    {
                        BackstoryCategoryAndSlot backstoryCategoryAndSlot2 = __instance.requiredBackstoriesAny[k];
                        RimWorld.BackstoryDef backstoryDef2 = ((backstoryCategoryAndSlot2.slot == BackstorySlot.Adulthood) ? adulthood : childhood);
                        if (backstoryDef2 != null && backstoryDef2.spawnCategories.Contains(backstoryCategoryAndSlot2.categoryName))
                        {
                            AddBackstoryReason(backstoryCategoryAndSlot2.slot, backstoryDef2, ref reasons);
                        }
                    }
                //    Log.Message("allTraits");
                    for (int l = 0; l < pawn.story.traits.allTraits.Count; l++)
                    {
                        Trait trait = pawn.story.traits.allTraits[l];
                        if (!trait.Suppressed)
                        {
                            List<MeditationFocusDef> allowedMeditationFocusTypes = trait.CurrentData.allowedMeditationFocusTypes;
                            if (allowedMeditationFocusTypes != null && allowedMeditationFocusTypes.Contains(__instance))
                            {
                                reasons.Add("MeditationFocusEnabledByTrait".Translate() + ": " + trait.LabelCap + ".");
                            }
                        }
                    }
                //    Log.Message("Done");
                }
                __result = reasons.ToLineList("  - ", capitalizeItems: true);
                return false;
            }
            return true;
        }

        static void AddBackstoryReason(BackstorySlot slot, RimWorld.BackstoryDef backstory, ref List<string> reasons)
        {
            string s = string.Empty;
            if (slot == BackstorySlot.Adulthood)
            {
                s = ("MeditationFocusEnabledByAdulthood".Translate() + ": " + backstory.title.CapitalizeFirst() + ".");
            }
            else
            {
                s = ("MeditationFocusEnabledByChildhood".Translate() + ": " + backstory.title.CapitalizeFirst() + ".");
            }
            if (!reasons.Contains(s)) reasons.Add(s);
        }
    }
}
