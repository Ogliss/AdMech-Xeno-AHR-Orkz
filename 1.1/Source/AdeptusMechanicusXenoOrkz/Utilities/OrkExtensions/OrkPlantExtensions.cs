using RimWorld;

namespace AdeptusMechanicus.ExtensionMethods
{
    public static class OrkPlantExtensions
    {
        public static bool isOrkoidFungus(this Plant plant)
        {
            return plant.def == OrkThingDefOf.OG_Plant_Orkoid_Fungus || plant.def == OrkThingDefOf.OG_Plant_Orkoid_Cocoon;
        }
    }

}
