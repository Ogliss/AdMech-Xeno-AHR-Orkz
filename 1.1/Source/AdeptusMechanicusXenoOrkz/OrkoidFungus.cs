using AdeptusMechanicus.settings;
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

    public class OrkoidFungus : Plant
	{
		public new bool HasEnoughLightToGrow => true;
		public float spawnChance => this.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonSpawnChance : AMMod.Instance.settings.FungusSpawnChance;

		public float snotlingChance => this.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonSnotChance : AMMod.Instance.settings.FungusSnotChance;
		public float grotChance => this.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonGrotChance : AMMod.Instance.settings.FungusGrotChance;
		public float orkChance => this.def.defName.Contains("Cocoon") ? AMMod.Instance.settings.CocoonOrkChance : AMMod.Instance.settings.FungusOrkChance;

		public new float GrowthRateFactor_Temperature
		{
			get
			{
				float num;
				if (!GenTemperature.TryGetTemperatureForCell(base.Position, base.Map, out num))
				{
					return 1f;
				}
				if (num < -10f)
				{
					return Mathf.InverseLerp(-50f, -10f, num);
				}
				if (num > 62f)
				{
					return Mathf.InverseLerp(116f, 62f, num);
				}
				return 1f;
			}
		}

		public override void PlantCollected()
		{

			base.PlantCollected();
		}
	} 
}
