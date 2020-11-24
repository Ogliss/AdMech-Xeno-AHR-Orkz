using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace AdeptusMechanicus
{
    public static class OrkoidFungualUtility
    {
        // Token: 0x04000BDF RID: 3039
        public static readonly SimpleCurve SquigSpawnCurve = new SimpleCurve
		{
			{
				new CurvePoint(-1f, 1f),
				true
			},
			{
				new CurvePoint(10f, 0.75f),
				true
			},
			{
				new CurvePoint(20f, 0.52f),
				true
			},
			{
				new CurvePoint(25f, 0.25f),
				true
			},
			{
				new CurvePoint(30f, 0.0f),
				true
			}
		};
        // Token: 0x04000BDF RID: 3039
        public static readonly SimpleCurve SnotSpawnCurve = new SimpleCurve
		{
			{
				new CurvePoint(-1f, 1f),
				true
			},
			{
				new CurvePoint(5f, 0.85f),
				true
			},
			{
				new CurvePoint(10f, 0.62f),
				true
			},
			{
				new CurvePoint(15f, 0.55f),
				true
			},
			{
				new CurvePoint(20f, 0.1f),
				true
			}
		};
        // Token: 0x04000BDF RID: 3039
        public static readonly SimpleCurve GrotSpawnCurve = new SimpleCurve
		{
			{
				new CurvePoint(-1f, 1f),
				true
			},
			{
				new CurvePoint(8f, 0.85f),
				true
			},
			{
				new CurvePoint(12f, 0.62f),
				true
			},
			{
				new CurvePoint(16f, 0.55f),
				true
			},
			{
				new CurvePoint(20f, 0.3f),
				true
			}
		};
        // Token: 0x04000BDF RID: 3039
        public static readonly SimpleCurve OrkSpawnCurve = new SimpleCurve
		{
			{
				new CurvePoint(-1f, 1f),
				true
			},
			{
				new CurvePoint(4f, 0.85f),
				true
			},
			{
				new CurvePoint(6f, 0.8f),
				true
			},
			{
				new CurvePoint(8f, 0.75f),
				true
			},
			{
				new CurvePoint(10f, 0.5f),
				true
			}
		};
	}
}
