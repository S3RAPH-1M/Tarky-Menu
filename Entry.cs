using System;
using BepInEx;
using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine;
using EFT;
using EFT.InventoryLogic;
using Comfort.Common;
using cameraClass = GClass1683;
using System.Collections.Generic;
using System.Linq;

namespace Tarky_Menu
{
    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInProcess("EscapeFromTarkov.exe")]
    public class EntryMethod : BaseUnityPlugin
    {
        private const string modGUID = "servph.TarkyMenu";
        private const string modName = "SERVPH's Tarky Menu";
        private const string modVersion = "1.0";
        private readonly Harmony harmony = new Harmony(modGUID);


        private Player _localPlayer;
        public static EntryMethod Instance { get; private set; }
        public ConfigEntry<Boolean> FOVEnabled { get; private set; }
        public ConfigEntry<Single> FOV { get; private set; }
        public ConfigEntry<Boolean> ThermalToggle { get; private set; }
        public ConfigEntry<Boolean> NightVisionToggle { get; private set; }
        public ConfigEntry<Boolean> MaxMelee { get; private set; }
        public ConfigEntry<Boolean> StealthStamina { get; private set; }
        public ConfigEntry<Boolean> MaxStamina { get; private set; }
        public ConfigEntry<Boolean> RecoilToggle { get; private set; }
        public ConfigEntry<Boolean> NoRecoilExtreme { get; private set; }
        public ConfigEntry<Single> Recoil { get; private set; }
        public ConfigEntry<Boolean> Godmode { get; private set; }
        public ConfigEntry<Boolean> HungerEnergyDrain { get; private set; }
        public ConfigEntry<Boolean> HideOverlay { get; private set; }
        public ConfigEntry<Boolean> NoFall { get; private set; }
        public ConfigEntry<Boolean> StrengthControl { get; private set; }
        public ConfigEntry<Single> StrengthValue { get; private set; }
        public ConfigEntry<Boolean> KillSelf { get; private set; }
        public ConfigEntry<Boolean> KillAll { get; private set; }
        void Awake()
        {
            Instance = this;
            this.FOVEnabled = this.Config.Bind("Misc | FOV Settings", "FOV Enabled", false, "Description");
            this.FOV = this.Config.Bind("Misc | FOV Settings", "FOV Amount", 65f, "Your FOV Amount");
            this.HideOverlay = this.Config.Bind("Misc | Random Test Stuff", "Hide Helmet Overlay", false, "");
            this.NoFall = this.Config.Bind("Cheats", "No Fall Damage", false, "");
            this.ThermalToggle = this.Config.Bind("Cheats", "Thermal Toggle", false, "Toggle Thermals");
            this.MaxMelee = this.Config.Bind("Cheats", "Max Melee", false, "Insta-Kills on melee");
            this.StealthStamina = this.Config.Bind("Cheats", "Stealth Infinite Stamina", false, "Replenish Stamina Instantly When Its Low");
            this.MaxStamina = this.Config.Bind("Cheats", "Infinite Stamina", false, "Stamina Never Drains");
            this.RecoilToggle = this.Config.Bind("Cheats", "Recoil Control", false, "Recoil Control Toggle");
            this.Recoil = this.Config.Bind("Cheats", "Recoil Value", 1f, "");
            this.NoRecoilExtreme = this.Config.Bind("Cheats", "No Recoil Xtreme", false, "I cant think of anything to put here");
            this.Godmode = this.Config.Bind("Cheats", "Godmode", false, "Invincible");
            this.HungerEnergyDrain = this.Config.Bind("Cheats", "No Energy/Hunger Drain", false, "");
            this.StrengthControl = this.Config.Bind("Cheats", "Strength Control", false, "");
            this.StrengthValue = this.Config.Bind("Cheats", "Strength Value", 1f, "");
            this.KillSelf = this.Config.Bind("Misc | Random Test Stuff", "Kill Yourself", false, "This is so sad :(");
            this.KillAll = this.Config.Bind("Misc | Random Test Stuff", "Genocide Everyone", false, "Genocide :D");
        }
        void Update()
        {

            if (!Singleton<GameWorld>.Instantiated)
            {
                _localPlayer = null;
                return;
            }

            GameWorld gameWorld = Singleton<GameWorld>.Instance;



            if (_localPlayer == null && gameWorld.RegisteredPlayers.Count > 0)
            {
                _localPlayer = gameWorld.RegisteredPlayers[0];
                return;
            }

            if (cameraClass.Instance != null)
            {
                if (FOVEnabled.Value && cameraClass.Instance.Fov != FOV.Value)
                {
                    cameraClass.Instance.Fov = FOV.Value;
                    cameraClass.Instance.SetFov(FOV.Value, 0f);
                }
                if (cameraClass.Instance.ThermalVision != null)
                {
                    cameraClass.Instance.ThermalVision.On = ThermalToggle.Value;

                    cameraClass.Instance.ThermalVision.IsNoisy = false;
                    cameraClass.Instance.ThermalVision.IsGlitch = false;
                    cameraClass.Instance.ThermalVision.IsMotionBlurred = false;
                    cameraClass.Instance.ThermalVision.IsPixelated = false;
                    cameraClass.Instance.ThermalVision.ThermalVisionUtilities.NoiseParameters.NoiseIntensity = 0;
                }
            }

            if (StealthStamina.Value && _localPlayer.Physical.Stamina.Current < 50)
            {
                _localPlayer.Physical.Stamina.Current = _localPlayer.Physical.Stamina.TotalCapacity.Value;
                _localPlayer.Physical.HandsStamina.Current = _localPlayer.Physical.HandsStamina.TotalCapacity.Value;
            }

            if (MaxStamina.Value)
            {
                _localPlayer.Physical.Stamina.Current = _localPlayer.Physical.Stamina.TotalCapacity.Value;
                _localPlayer.Physical.HandsStamina.Current = _localPlayer.Physical.HandsStamina.TotalCapacity.Value;
            }

            if (MaxMelee.Value)
            {
                _localPlayer.Skills.StrengthBuffMeleePowerInc.Value = 1337f;
            }

            if (RecoilToggle.Value)
            {
                _localPlayer.ProceduralWeaponAnimation.Shootingg.Intensity = Recoil.Value;
            }

            if (NoRecoilExtreme.Value)
            {
                _localPlayer.ProceduralWeaponAnimation.Shootingg.Intensity = 0f;
                _localPlayer.ProceduralWeaponAnimation.Shootingg.Stiffness = 0f;
                _localPlayer.ProceduralWeaponAnimation.Walk.Intensity = 0f;
                _localPlayer.ProceduralWeaponAnimation._shouldMoveWeaponCloser = false;
                _localPlayer.ProceduralWeaponAnimation.WalkEffectorEnabled = false;
                _localPlayer.ProceduralWeaponAnimation.Breath.Intensity = 0f;
                _localPlayer.ProceduralWeaponAnimation.MotionReact.Intensity = 0f;
                _localPlayer.ProceduralWeaponAnimation.ForceReact.Intensity = 0f;
            }

            if (Godmode.Value)
            {
                _localPlayer.ActiveHealthController.SetDamageCoeff(-50);
                _localPlayer.ActiveHealthController.RemoveNegativeEffects(EBodyPart.Common);
                _localPlayer.ActiveHealthController.RestoreFullHealth();
                _localPlayer.ActiveHealthController.FallSafeHeight = Math.BigMul(5000, 5000);
            }

            if (NoFall.Value)
            {
                _localPlayer.ActiveHealthController.FallSafeHeight = Math.BigMul(5000, 5000);
            }

            if (StrengthControl.Value)
            {
                _localPlayer.Skills.StrengthBuffJumpHeightInc.Value = StrengthValue.Value;
                _localPlayer.Skills.StrengthBuffSprintSpeedInc.Value = StrengthValue.Value;
                _localPlayer.Skills.StrengthBuffThrowDistanceInc.Value = StrengthValue.Value;
                _localPlayer.Skills.ThrowingStrengthBuff.Value = StrengthValue.Value;
                _localPlayer.Skills.StrengthBuffElite.Value = true;
            }

            if (HideOverlay.Value)
            {
                cameraClass.Instance.VisorEffect.enabled = !HideOverlay.Value;
            }

            if (KillSelf.Value)
            {
                _localPlayer.ActiveHealthController.ApplyDamage(EBodyPart.Head, 99999, default);
            }

            if (KillAll.Value)
            {
                var players = gameWorld.RegisteredPlayers.Where(x => x != _localPlayer).ToList();
                foreach (var player in players)
                {
                    player.ActiveHealthController.ApplyDamage(EBodyPart.Head, 99999, default);
                }
            }
        }
    }
}