using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace AdeptusMechanicus
{
    public static class OrkoidFungalUtility
    {

        public static readonly SimpleCurve SquigSpawnCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(5f, 0.85f),
				true
			},
			{
				new CurvePoint(20f, 0.75f),
				true
			},
			{
				new CurvePoint(25f, 0.65f),
				true
			},
			{
				new CurvePoint(30f, 0.5f),
				true
			},
			{
				new CurvePoint(50f, 0.00f),
				true
			}
		};

        public static readonly SimpleCurve SnotSpawnCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(5f, 0.75f),
				true
			},
			{
				new CurvePoint(10f, 0.5f),
				true
			},
			{
				new CurvePoint(15f, 0.25f),
				true
			},
			{
				new CurvePoint(20f, 0.125f),
				true
			},
			{
				new CurvePoint(50f, 0.00f),
				true
			}
		};

        public static readonly SimpleCurve GrotSpawnCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(8f, 0.75f),
				true
			},
			{
				new CurvePoint(10f, 0.5f),
				true
			},
			{
				new CurvePoint(15f, 0.25f),
				true
			},
			{
				new CurvePoint(20f, 0.125f),
				true
			}
		};

        public static readonly SimpleCurve OrkSpawnCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
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
