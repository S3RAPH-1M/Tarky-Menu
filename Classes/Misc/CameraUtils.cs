using BepInEx.Configuration;
using System;
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
            if (CameraClass.Instance != null)
            {
                if (FOVButton.Value.IsDown())
                {
                    FOVEnabled.Value = !FOVEnabled.Value;
                }
                if (FOVEnabled.Value)
                {
                    if (Instance.LocalPlayer != null && CameraClass.Instance != null && CameraClass.Instance.Camera.fieldOfView != FOV.Value)
                    {
                        CameraClass.Instance.SetFov(FOV.Value, 0f, true);
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
            }
        }

        public void ThermalController()
        {
            if (CameraClass.Instance != null)
            {
                if (ThermalButton.Value.IsDown())
                {
                    ThermalToggle.Value = !ThermalToggle.Value;
                }


                if (CameraClass.Instance.ThermalVision != null)
                {
                    CameraClass.Instance.ThermalVision.On = ThermalToggle.Value;
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
            }
        }

    }

}
