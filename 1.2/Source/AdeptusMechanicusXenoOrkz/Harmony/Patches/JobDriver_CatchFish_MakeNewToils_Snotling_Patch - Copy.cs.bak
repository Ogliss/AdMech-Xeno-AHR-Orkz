using System;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using AdeptusMechanicus.ExtensionMethods;
using Verse;
using System.Collections.Generic;
using Verse.AI;
using Quarry;
using System.Linq;

namespace AdeptusMechanicus.HarmonyInstance
{
	// Quarry.WorkGiver_MineQuarry.JobOnThing
	public class WorkGiver_MineQuarry_JobOnThing_Snotling_Patch
	{
        public static bool Prefix(Pawn pawn, Thing t, bool forced, ref Job __result)
        {
            if (pawn.isSnotling())
            {
				__result = MakeNewToils(pawn, t, forced);
				return false;
			}

			return true;
        }

        public static Job MakeNewToils(Pawn pawn, Thing t, bool forced)
        {
            Building_Quarry quarry = t as Building_Quarry;
            /*
            if (!pawn.workSettings.WorkIsActive(QuarryDefOf.QuarryMining))
            {
                return null;
            }
            */

            // Make sure a permitted quarry is found, and that it has resources, and does not have too many workers
            if (quarry == null || quarry.IsForbidden(pawn) || quarry.Depleted)
            {
                return null;
            }

            if (!quarry.Unowned && !quarry.AssignedPawns.Contains(pawn))
            {
                return null;
            }

            // Find a cell within the middle of the quarry to mine at
            IntVec3 cell = IntVec3.Invalid;
            CellRect rect = quarry.OccupiedRect().ContractedBy(quarry.WallThickness);
            foreach (IntVec3 c in rect.Cells.InRandomOrder())
            {
                if (pawn.Map.reservationManager.CanReserve(pawn, c, 1))
                {
                    cell = c;
                    break;
                }
            }
            // If a cell wasn't found, fail
            if (!cell.IsValid)
            {
                return null;
            }

            return new Job(QuarryDefOf.QRY_MineQuarry, cell);
        }
    }
}
