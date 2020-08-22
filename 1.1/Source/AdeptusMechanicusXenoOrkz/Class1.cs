using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace AdeptusMechanicus
{

    class OrkoidFungus : Plant
	{
		public new float GrowthRateFactor_Temperature
		{
			get
			{
				float num;
				if (!GenTemperature.TryGetTemperatureForCell(base.Position, base.Map, out num))
				{
					return 1f;
				}
				if (num < 10f)
				{
					return Mathf.InverseLerp(-20f, 10f, num);
				}
				if (num > 42f)
				{
					return Mathf.InverseLerp(116f, 42f, num);
				}
				return 1f;
			}
		}

		public new bool HasEnoughLightToGrow => true;
	} 
}
