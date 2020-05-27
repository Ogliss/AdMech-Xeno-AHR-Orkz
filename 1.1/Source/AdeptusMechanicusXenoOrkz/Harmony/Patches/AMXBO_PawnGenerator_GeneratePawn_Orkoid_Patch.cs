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

namespace AdeptusMechanicus.Orkz
{
    [HarmonyPatch(typeof(PawnGenerator), "GeneratePawn", new Type[] { typeof(PawnGenerationRequest) })]
    public static class AMXBO_PawnGenerator_GeneratePawn_Orkoid_Patch
    {
        [HarmonyPrefix]
        public static void Pre_GeneratePawn(ref PawnGenerationRequest request)
        {
            if (request.KindDef.isOrkoid())
            {
                request.FixedGender = Gender.None;
                //    Log.Message(string.Format("GeneratePawn request is {0}, {1}, {2}", request.KindDef.LabelCap, request.FixedGender, request.MustBeCapableOfViolence));
                PawnKindDef pawnKind = request.KindDef;
                float relation = 0f;
                bool mustbeviolent = request.KindDef.race != OGOrkThingDefOf.OG_Alien_Grot;
                request = new PawnGenerationRequest(pawnKind, request.Faction, request.Context, request.Tile, request.ForceGenerateNewPawn, request.Newborn, request.AllowDead, request.AllowDowned, request.CanGeneratePawnRelations, mustbeviolent, relation, request.ForceAddFreeWarmLayerIfNeeded, request.AllowGay, request.AllowFood, request.AllowAddictions, request.Inhabitant, request.CertainlyBeenInCryptosleep, request.ForceRedressWorldPawnIfFormerColonist, request.WorldPawnFactionDoesntMatter, request.BiocodeWeaponChance, request.ExtraPawnForExtraRelationChance, request.RelationWithExtraPawnChanceFactor, request.ValidatorPreGear, request.ValidatorPostGear, request.ForcedTraits, request.ProhibitedTraits, request.MinChanceToRedressWorldPawn, request.FixedBiologicalAge, request.FixedChronologicalAge, request.FixedGender, request.FixedMelanin, request.FixedLastName);
                //    Log.Message(string.Format("GeneratePawn End request is {0}, {1}, {2}", request.KindDef.LabelCap, request.FixedGender, request.MustBeCapableOfViolence));
            }
        }
        /*
        [HarmonyPostfix]
        public static void Post_GeneratePawn(ref Pawn __result)
        {
            if (__result!=null)
            {
                if (__result.kindDef!=null)
                {
                    if (__result.kindDef.isOrkoid())
                    {
                        PawnKindDef kindDef = __result.kindDef;
                        if (__result.RaceProps!=null)
                        {
                            if (__result.RaceProps.Humanlike)
                            {
                                Pawn_StoryTracker storyTracker = __result.story;
                                bool weirdKind = kindDef.defName.Contains("Weird");
                                bool weirdStory = storyTracker.adulthood.identifier.Contains("Weird") || storyTracker.childhood.identifier.Contains("Weird");
                                bool weirdOrk = __result.def == OGOrkThingDefOf.OG_Alien_Ork && (weirdStory || weirdKind);
                                bool weirdGrot = __result.def == OGOrkThingDefOf.OG_Alien_Grot && (weirdStory || weirdKind);
                                bool weird = (weirdGrot || weirdOrk);
                                if (weird)
                                {
                                    //   Log.Message(string.Format("{0} iz a wierd {1}....",__result.NameShortColored, __result.def.LabelCap));
                                }
                            }
                        }
                    }
                }
            }
        }
        */
    }
}
