using RimWorld;
using System.Collections.Generic;
using Verse;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.OrkoidFungalProps
    public class OrkoidFungalProps : PlantProperties
    {
        public bool canspawn = true;
        public bool spawnwild = true;
        public FloatRange tempsOptimal = new FloatRange(10f, 42f);
        public FloatRange tempsLimits = new FloatRange(0f, 58f);
        public float optionalChance = 0.01f;
        public List<OptionalThings> optionals = new List<OptionalThings>();
        
        public struct OptionalThings
        {
            public ThingDef def;
            public float weight;
        }
    }
}