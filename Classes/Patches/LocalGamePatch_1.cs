using Aki.Reflection.Patching;
using EFT;
using EFT.UI;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace Tarky_Menu.Classes.Patches
{
    public class LocalGamePatch_1 : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(Player).Assembly.GetType("EFT.LocalGame").GetMethod("Stop", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        [PatchPrefix]
        static bool Prefix()
        {
            Entry.Instance.HasDied = true;
            StaticManager.Instance.StartCoroutine(DoGhost());
            return false;
        }

        static IEnumerator DoGhost()
        {
            yield return new WaitForSeconds(1f);
            var DeathScreen = GameObject.Find("Game Scene/Death Screen");
            DeathScreen.SetActive(false);
        }
    }
}
