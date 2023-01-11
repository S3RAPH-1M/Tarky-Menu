using System;
using BepInEx;
using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine;
using EFT;
using EFT.InventoryLogic;
using Comfort.Common;
using cameraClass = CameraClass;
using System.Collections.Generic;
using System.Linq;
using EFT.Ballistics;
using System.Reflection;
using static Tarky_Menu.Entry;

namespace Tarky_Menu.Classes.Misc
{
    internal class CameraUtils
    {
        public ConfigEntry<Boolean> FOVEnabled { get; private set; }
        public ConfigEntry<Single> FOV { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> ThermalButton { get; private set; }
        public ConfigEntry<Boolean> ThermalToggle { get; private set; }
        public ConfigEntry<Boolean> HideOverlay { get; private set; }
        public ConfigEntry<Boolean> NoEffects { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> FOVButton { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> NVGButton { get; private set; }
        public ConfigEntry<Boolean> NVGButtonToggle { get; private set; }





        public void Awake()
        {
            this.HideOverlay = Instance.Config.Bind("Player | Camera", "Hide Helmet Overlay", false);
            this.NoEffects = Instance.Config.Bind("Player | Camera", "No More ScreenShake & Blood", false);
            this.FOVEnabled = Instance.Config.Bind("Player | Camera", "FOV Enabled", false, "Description");
            this.FOV = Instance.Config.Bind("Player | Camera", "FOV Amount", 65f, "Your FOV Amount");
            this.FOVButton = Instance.Config.Bind("Player | Camera", "FOV Button", new BepInEx.Configuration.KeyboardShortcut());
            this.NVGButton = Instance.Config.Bind("Player | Camera", "Clearer NVGs Button", new BepInEx.Configuration.KeyboardShortcut());
            this.NVGButtonToggle = Instance.Config.Bind("Player | Camera", "Clearer NVGs Toggle", false, "Toggle Thermals");
            this.ThermalButton = Instance.Config.Bind("Player | Camera", "Thermal Button", new BepInEx.Configuration.KeyboardShortcut());
            this.ThermalToggle = Instance.Config.Bind("Player | Camera", "Thermal Toggle", false, "Toggle Thermals");
        }

        public void FoVController()
        {
            if (cameraClass.Instance != null)
            {
                if (FOVButton.Value.IsDown())
                {
                    FOVEnabled.Value = !FOVEnabled.Value;
                }

                if (FOVEnabled.Value)
                {
                    cameraClass.Instance.SetFov(FOV.Value, 0f);
                }
            }
        }

        public void ScreenFXController()
        {
            if (cameraClass.Instance != null)
            {
                if (HideOverlay.Value && cameraClass.Instance.VisorEffect != null && cameraClass.Instance.VisorEffect.enabled )
                {
                    cameraClass.Instance.VisorEffect.enabled = !HideOverlay.Value;
                }

                if (NoEffects.Value && cameraClass.Instance.EffectsController != null)
                {
                    cameraClass.Instance.EffectsController.enabled = !NoEffects.Value;
                    cameraClass.Instance.Camera.GetComponent<CC_Blend>().enabled = !NoEffects.Value;
                    cameraClass.Instance.Camera.GetComponent<CC_Wiggle>().enabled = !NoEffects.Value;
                }
            }
        }

        public void ThermalController()
        {
            if (cameraClass.Instance != null)
            {
                if (ThermalButton.Value.IsDown())
                {
                    ThermalToggle.Value = !ThermalToggle.Value;
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
        }

        public void NVGController()
        {
            if (cameraClass.Instance != null)
            {
                if (cameraClass.Instance.NightVision != null && NVGButton.Value.IsDown())
                {
                    NVGButtonToggle.Value = !NVGButtonToggle.Value;
                }

                if (NVGButtonToggle.Value)
                {
                    if (cameraClass.Instance.NightVision != null && cameraClass.Instance.NightVision.On)
                    {
                        if (CameraClass.Instance.NightVision.NoiseIntensity > 0f)
                        {
                            CameraClass.Instance.NightVision.NoiseIntensity = 0f;
                        }
                        if (CameraClass.Instance.NightVision.MaskSize != 2000f)
                        {
                            CameraClass.Instance.NightVision.MaskSize = 2000f;
                            CameraClass.Instance.NightVision.ApplySettings();
                        }
                    }
                }
            }
        }

    }

}
