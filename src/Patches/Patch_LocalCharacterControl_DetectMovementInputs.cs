using HarmonyLib;
using OutwardBasicChatCommands.Managers;
using OutwardBasicChatCommands.Utility.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardBasicChatCommands.Patches
{
    [HarmonyPatch(typeof(LocalCharacterControl), nameof(LocalCharacterControl.DetectMovementInputs))]
    public static class Patch_LocalCharacterControl_DetectMovementInputs
    {
        static void Postfix(LocalCharacterControl __instance)
        {
            if(FollowerDataManager.Instance.IsCharacterFollowing(__instance.Character, out FollowerData data))
            {
                data.Update();
            }
        }
    }
}
