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


        public void Awake()
        {
            this.StealthStaminaToggle = Instance.Config.Bind("Cheats | Player Stats | Stamina", "Stealth Infinite Stamina", false, "Replenish Stamina Instantly When Its Low");
            this.MaxStaminaToggle = Instance.Config.Bind("Cheats | Player Stats | Stamina", "Infinite Stamina", false, "Stamina Never Drains");
            this.StrengthControlToggle = Instance.Config.Bind("Cheats | Player Stats | Strength", "Strength Control", false);
            this.JumpHeightValue = Instance.Config.Bind("Cheats | Player Stats | Strength", "Jump Height", 1f);
            this.ThrowDistanceValue = Instance.Config.Bind("Cheats | Player Stats | Strength", "Grenade Throw Distance", 1f);
            this.ThrowingDistanceBuff = Instance.Config.Bind("Cheats | Player Stats | Strength", "Throw Distance Buff Intensity", 1f);
            this.MeleeValue = Instance.Config.Bind("Cheats | Player Stats | Strength", "Melee Power", 1f);
            this.EliteStrengthValue = Instance.Config.Bind("Cheats | Player Stats | Strength", "Elite Strength", false);
            this.MaxMelee = Instance.Config.Bind("Cheats | Player Stats | Strength", "Max Melee", false, "Insta-Kills on melee");

        }

        public void Stamina()
        {
            if (MaxStaminaToggle.Value && Instance.LocalPlayer != null && Instance.LocalPlayer.Physical != null)
            {
                Instance.LocalPlayer.Physical.Stamina.Current = Instance.LocalPlayer.Physical.Stamina.TotalCapacity.Value;
                Instance.LocalPlayer.Physical.HandsStamina.Current = Instance.LocalPlayer.Physical.HandsStamina.TotalCapacity.Value;
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
            }
        }

        public void PlayerStats()
        {
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
