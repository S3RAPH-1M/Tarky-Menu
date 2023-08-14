using Aki.Reflection.Patching;
using EFT.UI;
using System.Reflection;
using UnityEngine;

namespace Tarky_Menu.Classes.Patches
{
    public class DeathFadePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(DeathFade).GetMethod("EnableEffect");
        }

        [PatchPrefix]
        static bool Prefix(DeathFade __instance)
        {
            return false;
        }
    }
}
