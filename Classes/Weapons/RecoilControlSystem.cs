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

namespace Tarky_Menu.Classes
{
    public class RecoilControlSystem
    {
        public ConfigEntry<Boolean> NoRecoilExtreme { get; set; }
        public ConfigEntry<Single> RecoilValue { get; set; }
        public ConfigEntry<Boolean> RCSToggle { get; set; }
        public ConfigEntry<Single> RecoilIntensityValue { get; set; }
        public ConfigEntry<Single> RecoilIntensityToggle { get; set; }
        public ConfigEntry<Single> WalkIntensityValue { get; set; }
        public ConfigEntry<Single> BreathIntensityValue { get; set; }
        public ConfigEntry<Single> ForceReactIntensityValue { get; set; }
        public ConfigEntry<Single> MotionReactIntesityValue { get; set; }
        public ConfigEntry<Single> ShootingStiffnessValue { get; set; }
       

        public void Awake()
        {
            this.NoRecoilExtreme = Instance.Config.Bind("Weapon | Recoil", "No Recoil Xtreme", false, "I cant think of anything to put here");
            this.RCSToggle = Instance.Config.Bind("Weapon | Recoil", "Recoil Control System", false, "Recoil Control Toggle");
            this.RecoilIntensityValue = Instance.Config.Bind("Weapon | Recoil", "Recoil Intensity Value", 1f);
            this.ShootingStiffnessValue = Instance.Config.Bind("Weapon | Recoil", "Shooting Stiffness Value", 1f);
            this.ForceReactIntensityValue = Instance.Config.Bind("Weapon | Recoil", "Force Reaction Value", 1f);
            this.MotionReactIntesityValue = Instance.Config.Bind("Weapon | Recoil", "Motion Reaction Value", 1f);
            this.BreathIntensityValue = Instance.Config.Bind("Weapon | Recoil", "Breath Intensity Value", 1f);
        }

        public void NoRecoil()
        {
            if (NoRecoilExtreme.Value && Instance.LocalPlayer != null && Singleton<GameWorld>.Instantiated)
            {
                Instance.LocalPlayer.ProceduralWeaponAnimation.Shootingg.Intensity = 0f;
                Instance.LocalPlayer.ProceduralWeaponAnimation.Shootingg.Stiffness = 0f;
                Instance.LocalPlayer.ProceduralWeaponAnimation.Walk.Intensity = 0f;
                Instance.LocalPlayer.ProceduralWeaponAnimation._shouldMoveWeaponCloser = false;
                Instance.LocalPlayer.ProceduralWeaponAnimation.WalkEffectorEnabled = false;
                Instance.LocalPlayer.ProceduralWeaponAnimation.Breath.Intensity = 0f;
                Instance.LocalPlayer.ProceduralWeaponAnimation.MotionReact.Intensity = 0f;
                Instance.LocalPlayer.ProceduralWeaponAnimation.ForceReact.Intensity = 0f;
            }
            else if (RCSToggle.Value && Instance.LocalPlayer != null && Singleton<GameWorld>.Instantiated)
            {
                Instance.LocalPlayer.ProceduralWeaponAnimation.Shootingg.Intensity = RecoilIntensityValue.Value;
                Instance.LocalPlayer.ProceduralWeaponAnimation.Shootingg.Stiffness = ShootingStiffnessValue.Value;
                Instance.LocalPlayer.ProceduralWeaponAnimation.Breath.Intensity = BreathIntensityValue.Value;
                Instance.LocalPlayer.ProceduralWeaponAnimation.MotionReact.Intensity = MotionReactIntesityValue.Value;
                Instance.LocalPlayer.ProceduralWeaponAnimation.ForceReact.Intensity = ForceReactIntensityValue.Value;
            }

        }
    }
}
