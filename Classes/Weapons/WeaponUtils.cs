using System;
using BepInEx.Configuration;
using EFT;
using EFT.InventoryLogic;
using static Tarky_Menu.Entry;

namespace Tarky_Menu.Classes.Weapons {
	public class WeaponUtils {
		
		public ConfigEntry<Boolean> AllFireModes { get; private set; }

		private Weapon.EFireMode[] AllModes = new Weapon.EFireMode[] { (Weapon.EFireMode)0, (Weapon.EFireMode)1, (Weapon.EFireMode)2, (Weapon.EFireMode)3, (Weapon.EFireMode)4, (Weapon.EFireMode)5 };

		
		public void Awake()
		{
			this.AllFireModes = Instance.Config.Bind("Weapon | Misc", "All Fire-Modes", false);
			
		}

		public void FireMod() 
		{
			if (AllFireModes.Value && Instance.LocalPlayer != null || AllFireModes.Value && Instance.HideoutPlayer)
			{
				if (Instance.LocalPlayer.HandsController is Player.FirearmController controller && controller.Item.Template.weapFireType != AllModes)
				{
					controller.Item.Template.weapFireType = AllModes;
				}
			}
		}
	}
}