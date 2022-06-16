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
        public ConfigEntry<Boolean> StrengthControlToggle { get; private set; }
        public ConfigEntry<Single> JumpHeightValue { get; private set; }
        public ConfigEntry<Single> ThrowDistanceValue { get; private set; }
        public ConfigEntry<Single> ThrowingDistanceBuff { get; private set; }
        public ConfigEntry<Boolean> EliteStrengthValue { get; private set; }
        public ConfigEntry<Single> MeleeValue { get; set; }
        public ConfigEntry<Boolean> MaxMelee { get; private set; }
        public ConfigEntry<Boolean> InstaSearch { get; private set; }
        public ConfigEntry<Boolean> NoRestraints { get; private set; }



        public void Awake()
        {
            this.StealthStaminaToggle = Instance.Config.Bind("Player | Skills", "Stealth Infinite Stamina", false, "Replenish Stamina Instantly When Its Low");
            this.MaxStaminaToggle = Instance.Config.Bind("Player | Skills", "Infinite Stamina", false, "Stamina Never Drains");
            this.StrengthControlToggle = Instance.Config.Bind("Player | Skills", "Strength Control", false);
            this.JumpHeightValue = Instance.Config.Bind("Player | Skills", "Jump Height", 1f);
            this.ThrowDistanceValue = Instance.Config.Bind("Player | Skills", "Grenade Throw Distance", 1f);
            this.ThrowingDistanceBuff = Instance.Config.Bind("Player | Skills", "Throw Distance Buff Intensity", 1f);
            this.MeleeValue = Instance.Config.Bind("Player | Skills", "Melee Power", 1f);
            this.EliteStrengthValue = Instance.Config.Bind("Player | Skills", "Elite Strength", false);
            this.InstaSearch = Instance.Config.Bind("Player | Skills", "Instant Search", false);
            this.MaxMelee = Instance.Config.Bind("Player | Skills", "Max Melee", false, "Insta-Kills on melee");
            this.NoRestraints = Instance.Config.Bind("Player | Weights", "No Speed Limiters", false, "Weight, Aiming and other speed limiters are off");

        }

        public void Stamina()
        {
            if (MaxStaminaToggle.Value && Instance.LocalPlayer != null && Instance.LocalPlayer.Physical != null)
            {
                Instance.LocalPlayer.Physical.Stamina.Current = Instance.LocalPlayer.Physical.Stamina.TotalCapacity.Value;
                Instance.LocalPlayer.Physical.HandsStamina.Current = Instance.LocalPlayer.Physical.HandsStamina.TotalCapacity.Value;
                Instance.LocalPlayer.Physical.Oxygen.Current = Instance.LocalPlayer.Physical.Oxygen.TotalCapacity.Value;
            }
            else if (StealthStaminaToggle.Value && Instance.LocalPlayer != null && Instance.LocalPlayer.Physical != null)
            {
                if (Instance.LocalPlayer.Physical.Stamina.Current < 50)
                {
                    Instance.LocalPlayer.Physical.Stamina.Current = Instance.LocalPlayer.Physical.Stamina.TotalCapacity.Value;
                }
                if (Instance.LocalPlayer.Physical.HandsStamina.Current < 50)
                {
                    Instance.LocalPlayer.Physical.HandsStamina.Current = Instance.LocalPlayer.Physical.HandsStamina.TotalCapacity.Value;
                }
                if (Instance.LocalPlayer.Physical.Oxygen.Current < 50)
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
                Instance.LocalPlayer.Skills.AttentionEliteExtraLootExp.Value = true;
                Instance.LocalPlayer.Skills.AttentionEliteLuckySearch.Value = 100f;
                Instance.LocalPlayer.Skills.IntellectEliteContainerScope.Value = true;
            }
            if (StrengthControlToggle.Value && Instance.LocalPlayer != null && Instance.LocalPlayer.Skills != null)
            {
                Instance.LocalPlayer.Skills.StrengthBuffJumpHeightInc.Value = JumpHeightValue.Value;
                Instance.LocalPlayer.Skills.StrengthBuffThrowDistanceInc.Value = ThrowDistanceValue.Value;
                Instance.LocalPlayer.Skills.ThrowingStrengthBuff.Value = ThrowingDistanceBuff.Value;
                Instance.LocalPlayer.Skills.StrengthBuffElite.Value = EliteStrengthValue.Value;
                Instance.LocalPlayer.Skills.StrengthBuffMeleePowerInc.Value = MeleeValue.Value;

            }
            else if (MaxMelee.Value && Instance.LocalPlayer != null && Instance.LocalPlayer.Skills != null)
            {
                Instance.LocalPlayer.Skills.StrengthBuffMeleePowerInc.Value = 1337f;
            }
        }
    }
}
