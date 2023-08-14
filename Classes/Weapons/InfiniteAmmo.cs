using Aki.Reflection.Patching;
using EFT.Ballistics;
using EFT.InventoryLogic;
using System.Linq;
using System.Reflection;

namespace Tarky_Menu.Classes.Weapons
{
    public class infiniteammo : ModulePatch
    {


        protected override MethodBase GetTargetMethod()
        {
            return typeof(BallisticsCalculator).GetMethod(nameof(BallisticsCalculator.Shoot));
        }

        [PatchPostfix]
        private static void Postfix(GClass2765 shot)
        {
            if (!Entry.Instance.InfAmmo.Value || !shot.Player.iPlayer.IsYourPlayer ||
                !(shot.Weapon is Weapon weapon) || shot.Ammo.Template.Name.ToLower().StartsWith("shrapnel"))
            {
                return;
            }

            MagazineClass magazine = weapon.GetCurrentMagazine();
            switch (magazine)
            {
                case null:
                    weapon.FirstFreeChamberSlot?.AddWithoutRestrictions(Utils.CreateItem<BulletClass>(shot.Ammo.TemplateId, default) ??
                                                     shot.Ammo);
                    return;
                case CylinderMagazineClass gClass2120:
                    {
                        gClass2120.Camoras.FirstOrDefault(c => c.ContainedItem == null)
                            ?.AddWithoutRestrictions(Utils.CreateItem<BulletClass>(shot.Ammo.TemplateId, default) ?? shot.Ammo);
                        return;
                    }
                default:
                    magazine.Cartridges?.Add(Utils.CreateItem<BulletClass>(shot.Ammo.TemplateId, default) ?? shot.Ammo,
                        false);
                    return;
            }
        }
    }
}