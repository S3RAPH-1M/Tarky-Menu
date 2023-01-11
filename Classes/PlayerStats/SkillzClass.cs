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
using static Tarky_Menu.Entry;

namespace Tarky_Menu.Classes.PlayerStats
{

    public class SkillzClass
    {
        public ConfigEntry<Boolean> StealthStaminaToggle { get; set; }
        public ConfigEntry<Boolean> MaxStaminaToggle { get; set; }
        public ConfigEntry<Boolean> MaxMelee { get; private set; }
        public ConfigEntry<Boolean> InstaSearch { get; private set; }
        public ConfigEntry<Boolean> NoRestraints { get; private set; }



        public void Awake()
        {
            this.StealthStaminaToggle = Instance.Config.Bind("Player | Skills", "Stealth Infinite Stamina", false, "Replenish Stamina Instantly When Its Low");
            this.InstaSearch = Instance.Config.Bind("Player | Skills", "Instant Search", false);
            this.MaxMelee = Instance.Config.Bind("Player | Skills", "Max Melee", false, "Insta-Kills on melee");
            this.NoRestraints = Instance.Config.Bind("Player | Weights", "No Speed Limiters", false, "Weight, Aiming and other speed limiters are off");

        }

        public void Stamina()
        {
            if (StealthStaminaToggle.Value && Instance.LocalPlayer != null && Instance.LocalPlayer.Physical != null)
            {
                if (Instance.LocalPlayer.Physical.Stamina.Current < 35)
                {
                    Instance.LocalPlayer.Physical.Stamina.Current = Instance.LocalPlayer.Physical.Stamina.TotalCapacity.Value;
                }
                if (Instance.LocalPlayer.Physical.HandsStamina.Current < 35)
                {
                    Instance.LocalPlayer.Physical.HandsStamina.Current = Instance.LocalPlayer.Physical.HandsStamina.TotalCapacity.Value;
                }
                if (Instance.LocalPlayer.Physical.Oxygen.Current < 75)
                {
                    Instance.LocalPlayer.Physical.Oxygen.Current = Instance.LocalPlayer.Physical.Oxygen.TotalCapacity.Value;
                }
            }
        }

        public void PlayerStats()
        {
            if (NoRestraints.Value && Instance.LocalPlayer != null)
            {
                Instance.LocalPlayer.RemoveStateSpeedLimit(Player.ESpeedLimit.Weight);
                Instance.LocalPlayer.RemoveStateSpeedLimit(Player.ESpeedLimit.Swamp);
                Instance.LocalPlayer.RemoveStateSpeedLimit(Player.ESpeedLimit.SurfaceNormal);
                Instance.LocalPlayer.RemoveStateSpeedLimit(Player.ESpeedLimit.Shot);
                Instance.LocalPlayer.RemoveStateSpeedLimit(Player.ESpeedLimit.HealthCondition);
                Instance.LocalPlayer.RemoveStateSpeedLimit(Player.ESpeedLimit.Fall);
                Instance.LocalPlayer.RemoveStateSpeedLimit(Player.ESpeedLimit.BarbedWire);
                Instance.LocalPlayer.RemoveStateSpeedLimit(Player.ESpeedLimit.Armor);
                Instance.LocalPlayer.RemoveStateSpeedLimit(Player.ESpeedLimit.Aiming);
            }
            
            if (InstaSearch.Value && Instance.LocalPlayer != null && Instance.LocalPlayer.Skills != null)
            {
                if (Instance.LocalPlayer.Skills.AttentionEliteExtraLootExp.Value != true)
                {
                    Instance.LocalPlayer.Skills.AttentionEliteExtraLootExp.Value = true;

                }
                if (Instance.LocalPlayer.Skills.AttentionEliteLuckySearch.Value != 100f)
                {
                    Instance.LocalPlayer.Skills.AttentionEliteLuckySearch.Value = 100f;

                }
                if (Instance.LocalPlayer.Skills.IntellectEliteContainerScope.Value != true)
                {
                    Instance.LocalPlayer.Skills.IntellectEliteContainerScope.Value = true;

                }
            }
            if (MaxMelee.Value && Instance.LocalPlayer != null && Instance.LocalPlayer.Skills != null && Instance.LocalPlayer.Skills.StrengthBuffMeleePowerInc.Value != 1337f)
            {
                Instance.LocalPlayer.Skills.StrengthBuffMeleePowerInc.Value = 1337f;
            }
        }
    }
}
