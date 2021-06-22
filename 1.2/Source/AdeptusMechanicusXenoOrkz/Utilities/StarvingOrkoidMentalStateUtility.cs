using System;
using System.Collections.Generic;
using AdeptusMechanicus.ExtensionMethods;
using RimWorld;

namespace Verse.AI
{
    public static class StarvingOrkoidMentalStateUtility
    {
        public static Pawn FindPawnToEat(Pawn pawn)
        {
            if (!pawn.Spawned)
            {
                return null;
            }
            if (!pawn.isOrkoid())
            {
                return null;
            }
            int Orkiness = (int)pawn.Orkiness();
            int MaxOrkiness = Math.Max(0, Orkiness - 2);
            StarvingOrkoidMentalStateUtility.tmpTargets.Clear();
            List<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned.FindAll(x=> (int)x.Orkiness() <= MaxOrkiness);
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

        private static List<Pawn> tmpTargets = new List<Pawn>();

        public static Pawn FindAnimal(Pawn pawn)
        {
            if (!pawn.Spawned)
            {
                return null;
            }
            StarvingOrkoidMentalStateUtility.tmpAnimals.Clear();
            List<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
            for (int i = 0; i < allPawnsSpawned.Count; i++)
            {
                Pawn pawn2 = allPawnsSpawned[i];
                if (pawn2.RaceProps.Animal && pawn2.Faction == pawn.Faction && pawn2 != pawn && !pawn2.IsBurning() && !pawn2.InAggroMentalState && pawn.CanReserveAndReach(pawn2, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
                {
                    StarvingOrkoidMentalStateUtility.tmpAnimals.Add(pawn2);
                }
            }
            if (!StarvingOrkoidMentalStateUtility.tmpAnimals.Any<Pawn>())
            {
                return null;
            }
            Pawn result = StarvingOrkoidMentalStateUtility.tmpAnimals.RandomElement<Pawn>();
            StarvingOrkoidMentalStateUtility.tmpAnimals.Clear();
            return result;
        }

        // Token: 0x040017D3 RID: 6099
        private static List<Pawn> tmpAnimals = new List<Pawn>();

    }
}

