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
    [HarmonyPatch(typeof(QuestManager), "Notify_PlantHarvested")]
    public static class QuestManager_Notify_PlantHarvested_Orkoid_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn worker, Thing harvested)
        {
            Log.Message(worker + " Harvested " + harvested);
        }
    }
}
