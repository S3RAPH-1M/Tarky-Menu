using System;
using System.Linq;
using System.Reflection;
using Aki.Reflection.Patching;
using BepInEx.Configuration;
using Comfort.Common;
using EFT;
using static Tarky_Menu.Entry;

namespace Tarky_Menu.Classes.Weapons
{
    public class QuickGrenadeThrow : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(Player).GetMethods().First(m =>
                m.Name == "SetInHands" && m.GetParameters()[0].Name == "throwWeap");
        }

        [PatchPrefix]
        private static Boolean Prefix(Player __instance, GrenadeClass throwWeap, Callback<GInterface110> callback)
        {
            if (Entry.Instance.LocalPlayer != null && Entry.Instance.QuickThrowNade.Value)
            {
                __instance.SetInHandsForQuickUse(throwWeap, null);
            }
            return !Entry.Instance.QuickThrowNade.Value;
        }
    }
}