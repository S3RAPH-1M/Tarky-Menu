using System;
using BepInEx;
using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine;
using EFT;
using EFT.InventoryLogic;
using Comfort.Common;
using System.Collections.Generic;
using System.Linq;
using EFT.Ballistics;
using System.Reflection;
using Tarky_Menu.Classes;
using Tarky_Menu.Classes.PlayerStats;
using static Tarky_Menu.Entry;

namespace Tarky_Menu.Classes.PlayerStats
{
    internal class Health
    {
        public ConfigEntry<Boolean> Godmode { get; private set; }
        public ConfigEntry<KeyCode> Heal { get; private set; }
        public ConfigEntry<Boolean> NoFall { get; private set; }
        public ConfigEntry<Boolean> HungerEnergyDrain { get; private set; }



        public void Awake()
        {
            this.Godmode = Instance.Config.Bind("Cheats", "Godmode", false, "Invincible");
            this.Heal = Instance.Config.Bind("Misc | Random Test Stuff", "Heal", KeyCode.None);
            this.NoFall = Instance.Config.Bind("Cheats", "No Fall Damage", false);
            this.HungerEnergyDrain = Instance.Config.Bind("Cheats", "No Energy/Hunger Drain", false);
        }

        public void godMod()
        {
            if (Instance.LocalPlayer != null && Instance.LocalPlayer.ActiveHealthController != null)
            {
                if (Godmode.Value)
                {
                    Instance.LocalPlayer.ActiveHealthController.SetDamageCoeff(-1f);
                    Instance.LocalPlayer.ActiveHealthController.RemoveNegativeEffects(EBodyPart.Common);
                    Instance.LocalPlayer.ActiveHealthController.RestoreFullHealth();
                }
                else
                {
                    Instance.LocalPlayer.ActiveHealthController.SetDamageCoeff(1f);
                }

                if (NoFall.Value)
                {
                    Instance.LocalPlayer.ActiveHealthController.FallSafeHeight = 9999999f;
                }
                else
                {
                    Instance.LocalPlayer.ActiveHealthController.FallSafeHeight = 3;
                }

                if (Input.GetKeyDown(Heal.Value))
                {
                    Instance.LocalPlayer.ActiveHealthController.RemoveNegativeEffects(EBodyPart.Common);
                    Instance.LocalPlayer.ActiveHealthController.RestoreFullHealth();
                }

                if (HungerEnergyDrain.Value)
                {
                    Instance.LocalPlayer.ActiveHealthController.ChangeEnergy(1000f);
                    Instance.LocalPlayer.ActiveHealthController.ChangeHydration(1000f);
                }
            }
        }
    }
}
