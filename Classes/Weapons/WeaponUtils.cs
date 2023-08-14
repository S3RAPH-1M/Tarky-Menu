using BepInEx.Configuration;
using EFT;
using EFT.InventoryLogic;
using System;
using static Tarky_Menu.Entry;

namespace Tarky_Menu.Classes.Weapons
{
    public class WeaponUtils
    {

        public ConfigEntry<Boolean> AllFireModes { get; private set; }
        public ConfigEntry<Boolean> NoOverheat { get; private set; }
        public ConfigEntry<Boolean> FireRateEnabled { get; private set; }
        public ConfigEntry<Single> FireRateNumber { get; private set; }

        private Weapon.EFireMode[] AllModes = new Weapon.EFireMode[] { (Weapon.EFireMode)0, (Weapon.EFireMode)1, (Weapon.EFireMode)2, (Weapon.EFireMode)3, (Weapon.EFireMode)4, (Weapon.EFireMode)5 };


        public void Awake()
        {
            this.NoOverheat = Instance.Config.Bind("Weapons | Misc", "No Jam & Overheat", false);
            this.AllFireModes = Instance.Config.Bind("Weapons | Misc", "All Fire-Modes", false);
            this.FireRateEnabled = Instance.Config.Bind("Weapons | Misc", "FireRate Changer", false);
            this.FireRateNumber = Instance.Config.Bind("Weapons | Misc", "FireRate Number", 650f);

        }

        public void FireMod()
        {
            if (AllFireModes.Value && Instance.LocalPlayer != null)
            {
                if (Instance.LocalPlayer.HandsController is Player.FirearmController controller && controller.Item.Template.weapFireType != AllModes)
                {
                    controller.Item.Template.weapFireType = AllModes;
                    //controller.Item.Template.bFirerate = controller.Item.Template.SingleFireRate;
                }
            }
        }

        public void NoJamOrHeat()
        {
            if (NoOverheat.Value == true && Instance.LocalPlayer != null)
            {
                if (Instance.LocalPlayer.HandsController is Player.FirearmController controller)
                {
                    controller.Item.Template.AllowOverheat = false;
                    controller.Item.Template.AllowMisfire = false;
                    controller.Item.Template.AllowJam = false;
                    controller.Item.Template.AllowFeed = false;
                    controller.Item.Template.AllowSlide = false;
                }
            }
        }

        public void FireRateMod()
        {
            if (FireRateEnabled.Value && Instance.LocalPlayer != null)
            {
                if (Instance.LocalPlayer.HandsController is Player.FirearmController controller)
                {
                    controller.Item.Template.bFirerate = (int)FireRateNumber.Value;
                    controller.Item.Template.SingleFireRate = (int)FireRateNumber.Value;
                }
            }
        }
    }
}