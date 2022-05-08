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
using EFT.Ballistics;
using System.Reflection;
using static Tarky_Menu.Entry;

namespace Tarky_Menu.Classes.Misc
{
    internal class CameraUtils
    {
        public ConfigEntry<Boolean> FOVEnabled { get; private set; }
        public ConfigEntry<Single> FOV { get; private set; }
        public ConfigEntry<KeyCode> ThermalButton { get; private set; }
        public ConfigEntry<Boolean> ThermalToggle { get; private set; }
        public ConfigEntry<Boolean> HideOverlay { get; private set; }
        public ConfigEntry<KeyCode> FOVButton { get; private set; }
        public ConfigEntry<KeyCode> NVGButton { get; private set; }
        public ConfigEntry<Boolean> NVGButtonToggle { get; private set; }
        public ConfigEntry<Boolean> NVGCheckbox { get; private set; }





        public void Awake()
        {
            this.HideOverlay = Instance.Config.Bind("Misc | Random Test Stuff", "Hide Helmet Overlay", false);
            this.FOVEnabled = Instance.Config.Bind("Misc | FOV Settings", "FOV Enabled", false, "Description");
            this.FOV = Instance.Config.Bind("Misc | FOV Settings", "FOV Amount", 65f, "Your FOV Amount");
            this.FOVButton = Instance.Config.Bind("Misc | FOV Settings", "FOV Button", KeyCode.None);
            this.NVGButton = Instance.Config.Bind("Misc | FOV Settings", "NVG Button", KeyCode.None);
            this.ThermalButton = Instance.Config.Bind("Cheats", "Thermal Button", KeyCode.None);
            this.ThermalToggle = Instance.Config.Bind("Cheats", "Thermal Toggle", false, "Toggle Thermals");
            this.NVGCheckbox = Instance.Config.Bind("Misc | FOV Settings", "NVG Toggle", false, "Toggle NVG");
        }

        public void CameraStuff()
        {
            if (cameraClass.Instance != null)
            {


                if (Input.GetKeyDown(FOVButton.Value))
                {
                    FOVEnabled.Value = !FOVEnabled.Value;
                }

                if (FOVEnabled.Value)
                {
                    cameraClass.Instance.SetFov(FOV.Value, 0f);
                }

                if (HideOverlay.Value && cameraClass.Instance.VisorEffect != null)
                {
                    cameraClass.Instance.VisorEffect.enabled = !HideOverlay.Value;
                }
                
                if (Input.GetKeyDown(ThermalButton.Value))
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

        public void NightVisionHax()
        {

            if (Input.GetKeyDown(NVGButton.Value))
            {
                var nvgcomponent = cameraClass.Instance.Camera.GetComponent<BSG.CameraEffects.NightVision>();

                nvgcomponent.StartSwitch(NVGCheckbox.Value = !NVGCheckbox.Value);
                nvgcomponent.TextureMask.Color = new Color(0f, 0f, 0f, 0f);
            }

        }
    }
}
