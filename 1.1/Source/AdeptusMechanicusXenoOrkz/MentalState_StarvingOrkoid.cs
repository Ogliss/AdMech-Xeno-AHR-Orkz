using System;
using System.Collections.Generic;
using AdeptusMechanicus.ExtensionMethods;
using RimWorld;

namespace Verse.AI
{
    // Token: 0x0200053B RID: 1339
    public class MentalState_StarvingOrkoid : MentalState
    {
        // Token: 0x06002605 RID: 9733 RVA: 0x000DE6F1 File Offset: 0x000DC8F1
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look<Pawn>(ref this.target, "target", false);
        }

        // Token: 0x06002606 RID: 9734 RVA: 0x0001006A File Offset: 0x0000E26A
        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Off;
        }

        // Token: 0x06002607 RID: 9735 RVA: 0x000DE70A File Offset: 0x000DC90A
        public override void PreStart()
        {
            base.PreStart();
            this.TryFindNewTarget();
        }

        // Token: 0x06002608 RID: 9736 RVA: 0x000DE71C File Offset: 0x000DC91C
        public override void MentalStateTick()
        {
            base.MentalStateTick();
            if (this.target != null && this.target.Dead)
            {
                base.RecoverFromState();
            }
            if (this.pawn.IsHashIntervalTick(120) && !this.IsTargetStillValidAndReachable())
            {
                if (!this.TryFindNewTarget())
                {
                    base.RecoverFromState();
                    return;
                }
                Messages.Message("MessageMurderousRageChangedTarget".Translate(this.pawn.LabelShort, this.target.Label, this.pawn.Named("PAWN"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NegativeEvent, true);
                base.MentalStateTick();
            }
        }

        // Token: 0x06002609 RID: 9737 RVA: 0x000DE7F8 File Offset: 0x000DC9F8
        public override string GetBeginLetterText()
        {
            if (this.target == null)
            {
                Log.Error("No target. This should have been checked in this mental state's worker.", false);
                return "";
            }
            return this.def.beginLetter.Formatted(this.pawn.NameShortColored.Resolve(), this.target.NameShortColored.Resolve(), this.pawn.Named("PAWN"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true).CapitalizeFirst();
        }

        // Token: 0x0600260A RID: 9738 RVA: 0x000DE89F File Offset: 0x000DCA9F
        private bool TryFindNewTarget()
        {
            this.target = StarvingOrkoidMentalStateUtility.FindPawnToKill(this.pawn);
            return this.target != null;
        }

        // Token: 0x0600260B RID: 9739 RVA: 0x000DE8BC File Offset: 0x000DCABC
        public bool IsTargetStillValidAndReachable()
        {
            return this.target != null && this.target.SpawnedParentOrMe != null && (!(this.target.SpawnedParentOrMe is Pawn) || this.target.SpawnedParentOrMe == this.target) && this.pawn.CanReach(this.target.SpawnedParentOrMe, PathEndMode.Touch, Danger.Deadly, true, TraverseMode.ByPawn);
        }

        // Token: 0x040016DD RID: 5853
        public Pawn target;

        // Token: 0x040016DE RID: 5854
        private const int NoLongerValidTargetCheckInterval = 120;
    }


    // Token: 0x0200055D RID: 1373
    public static class StarvingOrkoidMentalStateUtility
    {
        // Token: 0x06002681 RID: 9857 RVA: 0x000DF9CC File Offset: 0x000DDBCC
        public static Pawn FindPawnToKill(Pawn pawn)
        {
            if (!pawn.Spawned)
            {
                return null;
            }
            if (!pawn.isOrkoid())
            {
                return null;
            }
            int Orkiness = (int)StarvingOrkoidMentalStateUtility.Orkiness(pawn);
            int MaxOrkiness = Math.Max(0, Orkiness - 2);
            StarvingOrkoidMentalStateUtility.tmpTargets.Clear();
            List<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned.FindAll(x=> (int)StarvingOrkoidMentalStateUtility.Orkiness(x) <= MaxOrkiness);
            for (int i = 0; i < allPawnsSpawned.Count; i++)
            {
                Pawn pawn2 = allPawnsSpawned[i];
                if ((pawn2.Faction == pawn.Faction || (pawn2.IsPrisoner && pawn2.HostFaction == pawn.Faction)) /* && pawn2.RaceProps.Humanlike */ && pawn2 != pawn && pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn) && (pawn2.CurJob == null || !pawn2.CurJob.exitMapOnArrival))
                {
                    StarvingOrkoidMentalStateUtility.tmpTargets.Add(pawn2);
                }
            }
            if (!StarvingOrkoidMentalStateUtility.tmpTargets.Any<Pawn>())
            {
                return null;
            }
            Pawn result = StarvingOrkoidMentalStateUtility.tmpTargets.RandomElement<Pawn>();
            StarvingOrkoidMentalStateUtility.tmpTargets.Clear();
            return result;
        }

        // Token: 0x040016FA RID: 5882
        private static List<Pawn> tmpTargets = new List<Pawn>();

        public static Orkoid Orkiness(Pawn pawn)
        {
            if (pawn.isOrkoid())
            {
                if (pawn.def.defName.Contains("Gargantuan"))
                {
                    return Orkoid.GargantuanSquiggoth;
                }
                else
                if (pawn.def.defName.Contains("Squiggoth"))
                {
                    return Orkoid.Squiggoth;
                }
                else
                if (pawn.isOrk())
                {
                    if (pawn.kindDef.defName.Contains("Warboss"))
                    {
                        return Orkoid.Warboss;
                    }
                    else
                    if (pawn.kindDef.defName.Contains("Nob"))
                    {
                        return Orkoid.Nob;
                    }
                    else
                    if (pawn.kindDef.defName.Contains("Runt"))
                    {
                        return Orkoid.Runt;
                    }
                    else
                    return Orkoid.Ork;
                }
                else
                if (pawn.isGrot())
                {
                    return Orkoid.Grot;
                }
                else
                if (pawn.isSnotling())
                {
                    return Orkoid.Snotling;
                }
                else
                if (pawn.isSquig())
                {
                    return Orkoid.Squig;
                }
            }
            return Orkoid.NonOrkoid;
        }
    }

    public enum Orkoid
    {
        NonOrkoid,
        Squig,
        Snotling,
        Grot,
        Runt,
        Ork,
        Nob,
        Warboss,
        Squiggoth,
        GargantuanSquiggoth
    }
}

