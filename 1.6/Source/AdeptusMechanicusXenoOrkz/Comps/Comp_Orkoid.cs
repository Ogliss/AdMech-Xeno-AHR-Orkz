using AdeptusMechanicus.settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace AdeptusMechanicus
{
    public class CompProperties_Orkoid : CompProperties_Regeneration
    {

    }
    public class Comp_Orkoid : Comp_Regeneration
    {
        public new CompProperties_Orkoid Props => this.props as CompProperties_Orkoid;

        public override float HealFactor => Props.healFactor;
        public override float HealMinimum => AMAMod.settings.OrkoidMinHealing;
        public override float HealFactorScar => Props.healFactorOldWound;
        public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            base.PostPostApplyDamage(dinfo, totalDamageDealt);
        }
    }
}
