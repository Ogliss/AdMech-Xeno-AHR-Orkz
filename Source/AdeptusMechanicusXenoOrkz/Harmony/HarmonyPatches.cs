using AlienRace;
using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus.Orkz
{
    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {

        static HarmonyPatches()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("rimworld.ogliss.adeptusmechanicus.orkz");

            harmony.Patch(
                AccessTools.Method(typeof(PawnGenerator), "GeneratePawn", new[] { typeof(PawnGenerationRequest)}), 
                new HarmonyMethod(typeof(HarmonyPatches), nameof(Pre_GeneratePawn_Orkoid)),
                new HarmonyMethod(typeof(HarmonyPatches), nameof(Post_GeneratePawn_Orkoid)));
            /*
            harmony.Patch(
                AccessTools.Method(typeof(PawnGenerator), "GeneratePawnRelations", null, null), 
                new HarmonyMethod(HarmonyPatches.patchType, "Pre_GeneratePawnRelations_Orkoid", null), null, null);
            */
        }
        private static readonly Type patchType = typeof(HarmonyPatches);
        
        
        public static bool Pre_GeneratePawnRelations_Orkoid(Pawn pawn, ref PawnGenerationRequest request)
        {
            /*
            if (pawn.kindDef.RaceProps.BloodDef == OGOrkThingDefOf.OG_FilthBlood_Orkoid)
            {
                return false;
            }
            */
            return true;
        }

        public static void Pre_GeneratePawn_Orkoid(ref PawnGenerationRequest request, ref Pawn __result)
        {
            if (request.KindDef.race.race.BloodDef == OGOrkThingDefOf.OG_FilthBlood_Orkoid)
            {
                //    Log.Message(string.Format("Pre_GeneratePawn_Astartes request is {0}, {1}, {2}", request.KindDef.LabelCap, request.FixedGender, request.MustBeCapableOfViolence));
                PawnKindDef pawnKind = request.KindDef;
                float relation = 100f;
                bool mustbeviolent = request.KindDef.race != OGOrkThingDefOf.OG_Alien_Grot;
                request = new PawnGenerationRequest(pawnKind, request.Faction, request.Context, -1, true, false, false, false, false, mustbeviolent, relation, fixedGender: Gender.None, allowGay: false);
                //    Log.Message(string.Format("Pre_GeneratePawn_Astartes End request is {0}, {1}, {2}", request.KindDef.LabelCap, request.FixedGender, request.MustBeCapableOfViolence));
            }
        }

        public static void Post_GeneratePawn_Orkoid(PawnGenerationRequest request, ref Pawn __result)
        {
            PawnKindDef pawnKindDef = request.KindDef;
            /*
            if (__result.kindDef.race == AstartesOGDefOf.Human_Imperial_Astartes || __result.kindDef == AstartesOGDefOf.OG_Astartes_Neophyte)
            {
                //    Log.Message(string.Format("Post_GeneratePawn_Astartes request is {0}, {1}, {2}", request.KindDef.LabelCap, request.FixedGender, request.MustBeCapableOfViolence));
                __result.story.hairDef = Rand.Chance(0.5f) ? AstartesOGDefOf.Shaved : AstartesOGDefOf.Topdog;
                if (__result.kindDef == AstartesOGDefOf.OG_Astartes_Neophyte || __result.def == Constants.Astarte)
                {
                    Log.Message(string.Format("{0}: {1}", __result.LabelShortCap, __result.kindDef.label));
                    var Astartes_HediffGiverSet = AstartesOGDefOf.OG_Astartes_HediffGiverSet;
                    var Astartes_hediffGivers = Astartes_HediffGiverSet.hediffGivers;
                    if (Astartes_hediffGivers.Any(y => y is HediffGiver_AstartesHediff))
                    {
                        foreach (var hdg in Astartes_hediffGivers.Where(x => x is HediffGiver_AstartesHediff))
                        {
                            HediffGiver_AstartesHediff hediffGiver_StartWith = (HediffGiver_AstartesHediff)hdg;
                            hediffGiver_StartWith.GiveHediff(__result);
                        }
                    }
                }
                if (__result.kindDef == AstartesOGDefOf.OG_Astartes_Neophyte)
                {
                    bool flag1 = __result.health.hediffSet.HasHediff(AstartesOGDefOf.Hediff_AstartesOrgansOG_Ossmodula);
                    bool flag1a = __result.health.hediffSet.GetFirstHediffOfDef(AstartesOGDefOf.Hediff_AstartesOrgansOG_Ossmodula) != null && __result.health.hediffSet.GetFirstHediffOfDef(AstartesOGDefOf.Hediff_AstartesOrgansOG_Ossmodula).Severity == 1f;
                    bool flag2 = __result.health.hediffSet.HasHediff(AstartesOGDefOf.Hediff_AstartesOrgansOG_Biscopea);
                    bool flag2a = __result.health.hediffSet.GetFirstHediffOfDef(AstartesOGDefOf.Hediff_AstartesOrgansOG_Biscopea) != null && __result.health.hediffSet.GetFirstHediffOfDef(AstartesOGDefOf.Hediff_AstartesOrgansOG_Biscopea).Severity == 1f;
                    //    Log.Message(string.Format("Ossmodula: {0}\nBiscopea: {1}", flag1, flag2));
                    if (flag1 && flag1a)
                    {
                        if (flag2 && flag2a)
                        {
                            __result.story.bodyType = BodyTypeDefOf.Male;
                        }
                        else __result.story.bodyType = BodyTypeDefOf.Male;
                    }
                    else __result.story.bodyType = BodyTypeDefOf.Thin;
                }
                else __result.story.bodyType = BodyTypeDefOf.Hulk;
            }
            */
            return;
        }

    }
}