using System.Reflection;
using HarmonyLib;
using MelonLoader;
using ScheduleOne.Economy;
using ScheduleOne.Employees;
using ScheduleOne.Law;
using ScheduleOne.Noise;
using ScheduleOne.NPCs;
using ScheduleOne.NPCs.Actions;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.NPCs.Responses;
using ScheduleOne.PlayerScripts;
using UnityEngine.SceneManagement;

[assembly: MelonInfo(typeof(DealersAintSnitchez.Core), "DealersAintSnitchez", "1.0.0", "Stehlel", null)]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace DealersAintSnitchez
{
    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
        }

        [HarmonyPatch(typeof(NPCResponses_Civilian), "GetThreatResponse")]
        public static class Patch_GetThreatResponse_None
        {
            static bool Prefix(NPCResponses_Civilian __instance, ref NPCResponses_Civilian.EAttackResponse __result)
            {
                var npcField = typeof(NPCResponses).GetProperty("npc", BindingFlags.NonPublic | BindingFlags.Instance);
                var npcObj = (NPC)npcField.GetValue(__instance);
                bool isDealerRecruited = npcObj is Dealer dealer && dealer.IsRecruited;
                if (isDealerRecruited)
                {
                    MelonLogger.Msg("Suppressed ThreatResponse of " + npcObj.FirstName);
                    __result = NPCResponses_Civilian.EAttackResponse.None;
                    return false;
                }
                return true;
            }
        }
    }
}