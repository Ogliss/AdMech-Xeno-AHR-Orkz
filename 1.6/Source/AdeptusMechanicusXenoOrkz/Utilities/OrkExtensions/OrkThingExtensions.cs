using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace AdeptusMechanicus.ExtensionMethods
{
    public static class OrkThingExtensions
    {
        public static bool isOrkoidFungus(this Thing thing)
        {
            return thing.def == AdeptusThingDefOf.OG_Plant_Orkoid_Fungus || thing.def == AdeptusThingDefOf.OG_Plant_Orkoid_Cocoon;
        }

    }

}
