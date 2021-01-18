using System;
using RimWorld;
using AdeptusMechanicus.settings;
using Verse;

namespace AdeptusMechanicus
{
    // Token: 0x02000007 RID: 7
    public static class AMOSetIncidents
    {
        // Token: 0x06000010 RID: 16 RVA: 0x000026AC File Offset: 0x000016AC
        public static void SetIncidentLevels()
        {
            foreach (IncidentDef incidentDef in DefDatabase<IncidentDef>.AllDefsListForReading)
            {
                if (incidentDef == OGOrkIncidentDefOf.OG_Ork_Rok_Crash)
                {
                    if (AMAMod.settings.AllowOrkRok)
                    {
                        incidentDef.baseChance = 0.1f;
                    }
                    else
                    {
                        incidentDef.baseChance = 0f;
                    }
                }
            }
        }
    }
}
