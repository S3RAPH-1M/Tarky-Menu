using System;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT.Ballistics;
using EFT.InventoryLogic;
using BepInEx.Configuration;
using SERVPHAPI;
using EFT;
using SERVPH_API;
using static Tarky_Menu.Entry;

namespace Tarky_Menu.Classes.Weapons {
	public class infiniteammo : ModulePatch {


		protected override MethodBase GetTargetMethod() {
			return typeof(BallisticsCalculator).GetMethod("Shoot");
		}

		[PatchPostfix]
		private static void Postfix(GClass2388 shot) {
			Weapon weapon = null;
			bool flag;
			if (Entry.Instance.InfAmmo.Value && shot.Player.IsYourPlayer) {
				weapon = (shot.Weapon as Weapon);
				flag = (weapon != null);
			}
			else {
				flag = false;
			}
			bool flag2 = flag;
			if (flag2) {
				MagazineClass currentMagazine = weapon.GetCurrentMagazine();
				if (currentMagazine != null) {
					StackSlot cartridges = currentMagazine.Cartridges;
					if (cartridges != null) {
						cartridges.Add(Utils.CreateItem<Item>(shot.Ammo.TemplateId, null) ?? shot.Ammo, false);
					}
				}
			}
		}
	}
}