using BepInEx;
using BepInEx.Configuration;
using Comfort.Common;
using CommonAssets.Scripts.Game;
using EFT;
using EFT.MovingPlatforms;
using EFT.UI;
using System;
using System.Diagnostics;
using Tarky_Menu.Classes;
using Tarky_Menu.Classes.Misc;
using Tarky_Menu.Classes.PlayerStats;
using Tarky_Menu.Classes.Weapons;
using Tarky_Menu.Classes.World;
using Tarky_Menu.Classes.Patches;
#if DEBUG
using Tarky_Menu.Classes.Development; //Debugging Utils
#endif
using UnityEngine;
using System.Linq;

namespace Tarky_Menu
{
    [BepInPlugin(MOD_GUID, MOD_NAME, MOD_VERSION)]
    [BepInProcess("EscapeFromTarkov.exe")]
    public class Entry : BaseUnityPlugin
    {
        private const String MOD_GUID = "me.ssh.tarkymenu";
        private const String MOD_NAME = "SERVPH's Tarky Menu";
        private const String MOD_VERSION = "1.0";

        private Health _health;
        private CameraUtils _cameraUtils;
        private RecoilControlSystem _recoilControlSystem;
        private SkillzClass _skillzClass;
        private WeaponUtils _weaponUtils;
        private WorldUtils _worldUtils;
        private NPC_Controller _npcController;

#if DEBUG
        private Development _development; //Debugging Utils
#endif

        private Player.ESpeedLimit[] AllLimits = {
            0, (Player.ESpeedLimit)1, (Player.ESpeedLimit)2, (Player.ESpeedLimit)3, (Player.ESpeedLimit)4, (Player.ESpeedLimit)5, (Player.ESpeedLimit)6, (Player.ESpeedLimit)7, (Player.ESpeedLimit)8
        };


        public Player LocalPlayer { get; private set; }
        public Player HideoutPlayer { get; private set; }
        public static Entry Instance { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> FOVButton { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> SummonReserveTrain { get; private set; }
        public ConfigEntry<bool> InfAmmo { get; private set; }
        public ConfigEntry<bool> QuickThrowNade { get; private set; }
        public bool HasDemiGodRan { get; set; }
        public bool HasDied { get; set; }





        private void Awake()
        {
            Instance = this;
            this.SummonReserveTrain = this.Config.Bind("World | Misc", "Summon Extract Train", new BepInEx.Configuration.KeyboardShortcut());
            this.InfAmmo = this.Config.Bind("Weapons | Misc", "Infinite Ammo", false);
            this.QuickThrowNade = this.Config.Bind("Weapons | Misc", "Quick Throw Grenades", false);
            new infiniteammo().Enable();
            new QuickGrenadeThrow().Enable();
            new ConsoleBackground().Enable();
            this._recoilControlSystem = new RecoilControlSystem();
            this._skillzClass = new SkillzClass();
            this._health = new Health();
            this._cameraUtils = new CameraUtils();
            this._weaponUtils = new WeaponUtils();
            this._worldUtils = new WorldUtils();
            this._npcController = new NPC_Controller();
#if DEBUG
            this._development = new Development();
            new GameStartedPatch().Enable();
#endif
            this._skillzClass.Awake();
            this._health.Awake();
            this._weaponUtils.Awake();
            this._recoilControlSystem.Awake();
            this._cameraUtils.Awake();
            this._worldUtils.Awake();
            this._npcController.Awake();
#if DEBUG
            this._development.Awake(); //Debugging Utils
#endif
            ConsoleScreen.Processor.RegisterCommand("q", () => { Process.GetCurrentProcess().Kill(); });
            ConsoleScreen.Processor.RegisterCommand("quit", () => { Process.GetCurrentProcess().Kill(); });
            ConsoleScreen.Processor.RegisterCommand("clear", delegate ()
            {
                MonoBehaviourSingleton<PreloaderUI>.Instance.Console.Clear();
            });
            ConsoleScreen.Processor.RegisterCommand("extract", delegate ()
            {
                bool flag = !Singleton<AbstractGame>.Instantiated;
                if (flag)
                {
                    ConsoleScreen.LogError("This command may only be used inraid");
                }
                else
                {
                    EndByExitTrigerScenario.GInterface70 ginterface = Singleton<AbstractGame>.Instance as EndByExitTrigerScenario.GInterface70;
                    bool flag2 = ginterface != null;
                    if (flag2)
                    {
                        ginterface.StopSession(GamePlayerOwner.MyPlayer.ProfileId, ExitStatus.Survived, Singleton<GameWorld>.Instance.ExfiltrationController.ExfiltrationPoints.FirstOrDefault().name);
                    }
                    else
                    {
                        ConsoleScreen.LogError("Game is not stoppable");
                    }
                }
            });
        }

        private void Update()
        {
            if (!Singleton<GameWorld>.Instantiated)
            {
                this.LocalPlayer = null;
                this.HasDemiGodRan = false;
                return;
            }

            GameWorld gameWorld = Singleton<GameWorld>.Instance;

            if (this.LocalPlayer == null && gameWorld.AllAlivePlayersList.Count > 0)
            {
                this.LocalPlayer = gameWorld.AllAlivePlayersList[0];
                return;
            }

            if (this.HideoutPlayer == null && gameWorld.AllAlivePlayersList.Count > 0)
            {
                this.HideoutPlayer = gameWorld.AllAlivePlayersList[0];
                return;
            }

            this._skillzClass.Stamina();
            this._skillzClass.PlayerStats();
            this._health.godMod();
            this._recoilControlSystem.NoRecoil();
            this._weaponUtils.FireMod();
            this._weaponUtils.NoJamOrHeat();
            this._weaponUtils.FireRateMod();
            this._cameraUtils.FoVController();
            this._cameraUtils.ScreenFXController();
            this._cameraUtils.ThermalController();
            this._cameraUtils.NVGController();
            this._worldUtils.DoorUnlocker();
            this._npcController.Update();
#if DEBUG
            this._development.Update();
#endif
            this._worldUtils.MiscWorldUtilities();

            if (Instance.LocalPlayer != null && Instance.LocalPlayer.ActiveHealthController != null)
            {
                if (SummonReserveTrain.Value.IsDown())
                {
                    Locomotive Train = GameObject.FindObjectOfType<Locomotive>();
                    if (Train != null)
                    {
                        Train.Init(DateTime.UtcNow);
                    }
                }
            }
        }
    }
}
