using Aki.Reflection.Patching;
using EFT.UI;
using System.Reflection;
using UnityEngine;

namespace Tarky_Menu.Classes.Patches
{
    public class ConsoleBackground : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(PreloaderUI).GetMethod("InitConsole");
        }

        [PatchPostfix]
        private static void Postfix(PreloaderUI __instance)
        {
            Transform transform = __instance.transform.Find(ConsoleBackground._backgroundPath);
            bool flag = transform != null;
            if (flag)
            {
                transform.gameObject.SetActive(false);
                ModulePatch.Logger.LogInfo("Hid console background");
            }
            else
            {
                ModulePatch.Logger.LogError("Failed to find console background at: " + ConsoleBackground._backgroundPath);
            }
        }

        private static string _backgroundPath = "/Preloader UI/Preloader UI/Console/LogsPanel/Background";
    }
}
