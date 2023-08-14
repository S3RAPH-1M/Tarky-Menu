using Aki.Reflection.Patching;
using Comfort.Common;
using EFT;
using EFT.UI;
using System.Reflection;
using UnityEngine;

namespace Tarky_Menu.Classes.Patches
{
    public class GameStartedPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(GameWorld).GetMethod(nameof(GameWorld.OnGameStarted));
        }

        [PatchPostfix]

        private static void Postfix(GameWorld __instance)
        {
            new LocalGamePatch_1().Disable();
            new DeathFadePatch().Disable();
        }
    }
}
