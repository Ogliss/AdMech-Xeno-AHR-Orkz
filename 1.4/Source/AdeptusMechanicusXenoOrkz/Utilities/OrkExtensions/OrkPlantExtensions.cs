using RimWorld;

namespace AdeptusMechanicus.ExtensionMethods
{
    public static class OrkPlantExtensions
    {
        public static bool isOrkoidFungus(this Plant plant)
        {
            return plant.def == AdeptusThingDefOf.OG_Plant_Orkoid_Fungus || plant.def == AdeptusThingDefOf.OG_Plant_Orkoid_Cocoon;
        }
    }

}
