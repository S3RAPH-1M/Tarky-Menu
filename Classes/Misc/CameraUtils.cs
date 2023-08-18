using BepInEx.Configuration;
using System;
using UnityEngine;
using static Tarky_Menu.Entry;


namespace Tarky_Menu.Classes.Misc
{
    internal class CameraUtils
    {
        public ConfigEntry<Boolean> FOVEnabled { get; private set; }
        public ConfigEntry<Boolean> FOVAimingEnabled { get; private set; }
        public ConfigEntry<Single> FOV { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> ThermalButton { get; private set; }
        public ConfigEntry<Boolean> ThermalToggle { get; private set; }
        public ConfigEntry<Boolean> HideOverlay { get; private set; }
        public ConfigEntry<Boolean> NoSmudge { get; private set; }
        public ConfigEntry<Boolean> NoEffects { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> FOVButton { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> NVGButton { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> FOVAimButton { get; private set; }
        public ConfigEntry<Boolean> NVGButtonToggle { get; private set; }
        public ConfigEntry<Color> NVGColor { get; private set; }
        public ConfigEntry<Boolean> NVGColorToggle { get; private set; }

        public float timer = 0f;
        public float interval = 0.015f; // 



        public void Awake()
        {
            this.HideOverlay = Instance.Config.Bind("Player | Camera", "Hide Helmet Overlay", false);
            this.NoSmudge = Instance.Config.Bind("Player | Camera", "No Visor Smudge", false);
            this.NoEffects = Instance.Config.Bind("Player | Camera", "No More ScreenShake & Blood", false);
            this.FOVEnabled = Instance.Config.Bind("Player | Camera", "FOV Enabled", false, "Description");
            this.FOVAimingEnabled = Instance.Config.Bind("Player | Camera", "FOV Applied When Aiming", true);
            this.FOV = Instance.Config.Bind("Player | Camera", "FOV Amount", 65f, "Your FOV Amount");
            this.FOVButton = Instance.Config.Bind("Player | Camera", "FOV Button", new BepInEx.Configuration.KeyboardShortcut());
            this.NVGButton = Instance.Config.Bind("Player | Camera", "Clearer NVGs Button", new BepInEx.Configuration.KeyboardShortcut());
            this.NVGButtonToggle = Instance.Config.Bind("Player | Camera", "Clearer NVGs Toggle", false, "Toggle Thermals");
            this.ThermalButton = Instance.Config.Bind("Player | Camera", "Thermal Button", new BepInEx.Configuration.KeyboardShortcut());
            this.FOVAimButton = Instance.Config.Bind("Player | Camera", "FOV Aiming Button", new BepInEx.Configuration.KeyboardShortcut());
            this.NVGColorToggle = Instance.Config.Bind("Player | Camera", "NVG Color Enabled", false);
            this.NVGColor = Instance.Config.Bind("Player | Camera", "NVG Color", Color.green);
        }

        public void FoVController()
        {
            if (CameraClass.Instance != null)
            {
                if (FOVButton.Value.IsDown())
                {
                    FOVEnabled.Value = !FOVEnabled.Value;
                }
                if (FOVAimButton.Value.IsDown())
                {
                    FOVAimingEnabled.Value = !FOVAimingEnabled.Value;
                }
                if (FOVEnabled.Value)
                {
                    timer += Time.deltaTime;
                    if (Instance.LocalPlayer != null && CameraClass.Instance != null && timer >= interval)
                    {
                        if (!FOVAimingEnabled.Value)
                        {
                            if (Instance.LocalPlayer.HandsController.IsAiming)
                            {
                                return;
                            }
                        }
                        CameraClass.Instance.SetFov(FOV.Value, 0f, true);
                        CameraClass.Instance.Camera.nearClipPlane = 0.005f;
                        timer = 0f;
                    }
                }
            }
        }

        public void ScreenFXController()
        {
            if (CameraClass.Instance != null)
            {
                if (HideOverlay.Value && CameraClass.Instance.VisorEffect != null && CameraClass.Instance.VisorEffect.enabled)
                {
                    CameraClass.Instance.VisorEffect.enabled = !HideOverlay.Value;
                }

                if (NoEffects.Value && CameraClass.Instance.EffectsController != null)
                {
                    CameraClass.Instance.EffectsController.enabled = !NoEffects.Value;
                    CameraClass.Instance.Camera.GetComponent<CC_Blend>().enabled = !NoEffects.Value;
                    CameraClass.Instance.Camera.GetComponent<CC_Wiggle>().enabled = !NoEffects.Value;
                }

                if (NoSmudge.Value && CameraClass.Instance.VisorEffect != null && CameraClass.Instance.VisorEffect.enabled)
                {
                    CameraClass.Instance.VisorEffect.blurIterations = 0;
                    CameraClass.Instance.VisorEffect.blurSize = 0;
                    CameraClass.Instance.VisorEffect.DistortIntensity = 0;
                    CameraClass.Instance.VisorEffect.ScratcesIntensity = 0;
                }
            }
        }

        public void ThermalController()
        {
            if (CameraClass.Instance != null)
            {
                if (ThermalButton.Value.IsDown())
                {
                    CameraClass.Instance.ThermalVision.On = !CameraClass.Instance.ThermalVision.On;
                    CameraClass.Instance.ThermalVision.IsNoisy = false;
                    CameraClass.Instance.ThermalVision.IsGlitch = false;
                    CameraClass.Instance.ThermalVision.IsMotionBlurred = false;
                    CameraClass.Instance.ThermalVision.IsPixelated = false;
                    CameraClass.Instance.ThermalVision.ThermalVisionUtilities.NoiseParameters.NoiseIntensity = 0;
                }
            }
        }

        public void NVGController()
        {
            if (CameraClass.Instance != null)
            {
                if (CameraClass.Instance.NightVision != null && NVGButton.Value.IsDown())
                {
                    NVGButtonToggle.Value = !NVGButtonToggle.Value;
                }

                if (NVGButtonToggle.Value)
                {
                    if (CameraClass.Instance.NightVision != null && CameraClass.Instance.NightVision.On)
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

                if (NVGColorToggle.Value)
                {
                    if (CameraClass.Instance.NightVision != null && CameraClass.Instance.NightVision.On && CameraClass.Instance.NightVision.Color != null)
                    {
                        CameraClass.Instance.NightVision.Color = NVGColor.Value;
                    }
                }

            }
        }

    }

}
