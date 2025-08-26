using AdeptusMechanicus.ExtensionMethods;
using Verse;

namespace AdeptusMechanicus
{
    public static class GrowthUtility_Orkoid
    {
        public static bool IsGrowthBirthday(int age, Pawn pawn)
        {
            int[] GrowthMomentAges = pawn.isOrk() ? GrowthMomentAgesOrk : GrowthMomentAgesGrot;
            for (int i = 0; i < GrowthMomentAges.Length; i++)
            {
                if (age == GrowthMomentAges[i])
                {
                    return true;
                }
            }
            return false;
        }

        public static readonly int[] GrowthMomentAgesOrk = new int[]
        {
            1,
            2,
            10
        };
        public static readonly int[] GrowthMomentAgesGrot = new int[]
        {
            1,
            2,
            3
        };

    }
}

