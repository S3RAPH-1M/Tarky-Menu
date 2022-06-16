using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Aki.Reflection.Patching;

namespace Tarky_Menu.Classes.Misc
{
    internal class ExhaustionPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(GClass657).GetProperty(nameof(GClass657.Exhausted), BindingFlags.Public | BindingFlags.Instance).GetGetMethod();
        }

        [PatchPostfix]
        private static void Postfix(ref bool __result)
        {
            __result = false;
        }
    }
}
