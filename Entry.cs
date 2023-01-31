using BepInEx;
using BepInEx.Configuration;
using Comfort.Common;
using EFT;
using EFT.MovingPlatforms;
using EFT.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tarky_Menu.Classes;
using Tarky_Menu.Classes.Misc;
using Tarky_Menu.Classes.PlayerStats;
using Tarky_Menu.Classes.Weapons;
using UnityEngine;

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

        private Player.ESpeedLimit[] AllLimits = {
            0, (Player.ESpeedLimit)1, (Player.ESpeedLimit)2, (Player.ESpeedLimit)3, (Player.ESpeedLimit)4, (Player.ESpeedLimit)5, (Player.ESpeedLimit)6, (Player.ESpeedLimit)7, (Player.ESpeedLimit)8
        };

        public Player LocalPlayer { get; private set; }
        public Player HideoutPlayer { get; private set; }

        public static Entry Instance { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> FOVButton { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> TPAll { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> TPUsec { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> TPBear { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> TPScav { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> SummonReserveTrain { get; private set; }
        public ConfigEntry<bool> InfAmmo { get; private set; }
        public ConfigEntry<bool> QuickThrowNade { get; private set; }
        public bool HasDemiGodRan { get; set; }

        private void Awake()
        {
            Instance = this;
            this.TPAll = this.Config.Bind("World | AI", "TP All 2 U", new BepInEx.Configuration.KeyboardShortcut());
            this.TPUsec = this.Config.Bind("World | AI", "TP All Usec To You", new BepInEx.Configuration.KeyboardShortcut());
            this.TPBear = this.Config.Bind("World | AI", "TP All Bear To You", new BepInEx.Configuration.KeyboardShortcut());
            this.TPScav = this.Config.Bind("World | AI", "TP All Scav To You", new BepInEx.Configuration.KeyboardShortcut());
            this.SummonReserveTrain = this.Config.Bind("World | Misc", "Summon Extract Train", new BepInEx.Configuration.KeyboardShortcut());
            this.InfAmmo = this.Config.Bind("Weapons | Misc", "Infinite Ammo", false);
            this.QuickThrowNade = this.Config.Bind("Weapons | Misc", "Quick Throw Grenades", false);
            new infiniteammo().Enable();
            new QuickGrenadeThrow().Enable();
            this._recoilControlSystem = new RecoilControlSystem();
            this._skillzClass = new SkillzClass();
            this._health = new Health();
            this._cameraUtils = new CameraUtils();
            this._weaponUtils = new WeaponUtils();
            this._worldUtils = new WorldUtils();
            this._skillzClass.Awake();
            this._health.Awake();
            this._weaponUtils.Awake();
            this._recoilControlSystem.Awake();
            this._cameraUtils.Awake();
            this._worldUtils.Awake();

            ConsoleScreen.Processor.RegisterCommand("q", () => { Process.GetCurrentProcess().Kill(); });
            ConsoleScreen.Processor.RegisterCommand("quit", () => { Process.GetCurrentProcess().Kill(); });
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

            if (this.LocalPlayer == null && gameWorld.RegisteredPlayers.Count > 0)
            {
                this.LocalPlayer = gameWorld.RegisteredPlayers[0];
                return;
            }

            if (this.HideoutPlayer == null && gameWorld.RegisteredPlayers.Count > 0)
            {
                this.HideoutPlayer = gameWorld.RegisteredPlayers[0];
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
            this._worldUtils.MiscWorldUtilities();

            if (Instance.LocalPlayer != null && Instance.LocalPlayer.ActiveHealthController != null)
            {
                if (TPAll.Value.IsDown())
                {
                    IEnumerable<Player> players = gameWorld.RegisteredPlayers.Where(x => !x.IsYourPlayer);
                    foreach (Player player in players)
                    {
                        player.Teleport(this.LocalPlayer.Transform.position);
                    }
                }

                if (TPScav.Value.IsDown())
                {
                    IEnumerable<Player> players = gameWorld.RegisteredPlayers.Where(x => !x.IsYourPlayer);
                    foreach (Player player in players)
                    {
                        if (player.Profile.Side == EPlayerSide.Savage)
                        {
                            player.Teleport(this.LocalPlayer.Transform.position);
                        }
                    }
                }

                if (TPUsec.Value.IsDown())
                {
                    IEnumerable<Player> players = gameWorld.RegisteredPlayers.Where(x => !x.IsYourPlayer);
                    foreach (Player player in players)
                    {
                        if (player.Profile.Side == EPlayerSide.Usec)
                        {
                            player.Teleport(this.LocalPlayer.Transform.position);
                        }
                    }
                }

                if (TPBear.Value.IsDown())
                {
                    IEnumerable<Player> players = gameWorld.RegisteredPlayers.Where(x => !x.IsYourPlayer);
                    foreach (Player player in players)
                    {
                        if (player.Profile.Side == EPlayerSide.Bear)
                        {
                            player.Teleport(this.LocalPlayer.Transform.position);
                        }
                    }
                }

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