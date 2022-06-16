using System;
using BepInEx;
using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine;
using CommonAssets.Scripts.Game;
using EFT;
using EFT.InventoryLogic;
using Comfort.Common;
using System.Collections.Generic;
using System.Linq;
using EFT.Ballistics;
using System.Reflection;
using Tarky_Menu.Classes;
using Tarky_Menu.Classes.PlayerStats;
using Tarky_Menu.Classes.Misc;
using System.Diagnostics;
using System.IO;
using EFT.Interactive;
using EFT.UI;
using Tarky_Menu.Classes.Weapons;

namespace Tarky_Menu
{
    [BepInPlugin(MOD_GUID, MOD_NAME, MOD_VERSION)]
    [BepInProcess("EscapeFromTarkov.exe")]
    public class Entry : BaseUnityPlugin
    {
        private const string MOD_GUID = "me.ssh.tarkymenu";
        private const string MOD_NAME = "SERVPH's Tarky Menu";
        private const string MOD_VERSION = "1.0";


        private RecoilControlSystem _recoilControlSystem;
        private SkillzClass _skillzClass;
        private Health _health;
        private CameraUtils _cameraUtils;
        private WorldUtils _worldUtils;
        private WeaponUtils _weaponUtils;
        private Player.ESpeedLimit[] AllLimits = new Player.ESpeedLimit[] { (Player.ESpeedLimit)0, (Player.ESpeedLimit)1, (Player.ESpeedLimit)2, (Player.ESpeedLimit)3, (Player.ESpeedLimit)4, (Player.ESpeedLimit)5, (Player.ESpeedLimit)6, (Player.ESpeedLimit)7, (Player.ESpeedLimit)8 };

        public Player LocalPlayer { get; private set; }

        public static Entry Instance { get; private set; }
        public ConfigEntry<KeyCode> KillSelf { get; private set; }
        public ConfigEntry<Boolean> KillAll { get; private set; }
        public ConfigEntry<KeyCode> FOVButton { get; private set; }
        public ConfigEntry<KeyCode> TPAll { get; private set; }
        public ConfigEntry<KeyCode> TPUsec { get; private set; }
        public ConfigEntry<KeyCode> TPBear { get; private set; }
        public ConfigEntry<KeyCode> TPScav { get; private set; }
        public ConfigEntry<KeyCode> TPCrate { get; private set; }
        public ConfigEntry<Boolean> NoRagdollStop { get; private set; }

        void Awake()
        {
            Instance = this;
            this.KillSelf = this.Config.Bind("Player | Health", "Kill Yourself", KeyCode.None, "This is so sad :(");
            this.KillAll = this.Config.Bind("World | AI", "Kill all", false, "Genocide :D");
            this.TPAll = this.Config.Bind("World | AI", "TP All 2 U", KeyCode.None);
            this.TPCrate = this.Config.Bind("World | AI", "TP Crates To You", KeyCode.None);
            this.TPUsec = this.Config.Bind("World | AI", "TP All Usec To You", KeyCode.None);
            this.TPBear = this.Config.Bind("World | AI", "TP All Bear To You", KeyCode.None);
            this.TPScav = this.Config.Bind("World | AI", "TP All Scav To You", KeyCode.None);
            this.NoRagdollStop = this.Config.Bind("World | AI", "Ragdolls Never Freeze", false, "Has virtually no impact on performance, And looks really good :)");
            _recoilControlSystem = new Classes.RecoilControlSystem();
            _skillzClass = new Classes.PlayerStats.SkillzClass();
            _health = new Classes.PlayerStats.Health();
            _cameraUtils = new Classes.Misc.CameraUtils();
            _worldUtils = new Classes.Misc.WorldUtils();
            _weaponUtils = new WeaponUtils();
            _recoilControlSystem.Awake();
            _skillzClass.Awake();
            _health.Awake();
            _cameraUtils.Awake();
            _worldUtils.Awake();
            this._weaponUtils.Awake();

            ConsoleScreen.Commands.Add(new GClass2285("quit", _ => { Process.GetCurrentProcess().Kill(); }));
            ConsoleScreen.Commands.Add(new GClass2285("q", _ => { Process.GetCurrentProcess().Kill(); }));

        }


        void Update()
        {

            if (!Singleton<GameWorld>.Instantiated)
            {
                LocalPlayer = null;
                return;
            }

            GameWorld gameWorld = Singleton<GameWorld>.Instance;



            if (LocalPlayer == null && gameWorld.RegisteredPlayers.Count > 0)
            {
                LocalPlayer = gameWorld.RegisteredPlayers[0];
                return;
            }

            _skillzClass.Stamina();
            _skillzClass.PlayerStats();
            _recoilControlSystem.NoRecoil();
            _health.godMod();
            _cameraUtils.CameraStuff();
            _cameraUtils.NightVisionHax();
            _worldUtils.DoorUnlocker();
            _worldUtils.MiscWorldUtilities();
            _weaponUtils.FireMod();

            if (Instance.LocalPlayer != null && Instance.LocalPlayer.ActiveHealthController != null)
            {
                if (Input.GetKeyDown(KillSelf.Value))
                {
                    LocalPlayer.ActiveHealthController.Kill(EDamageType.RadExposure);
                }

                if (KillAll.Value)
                {
                    var players = gameWorld.RegisteredPlayers.Where(x => x != LocalPlayer);
                    foreach (var player in players)
                    {
                        player.ActiveHealthController.Kill(EDamageType.RadExposure);
                    }
                }

                if (Input.GetKeyDown(TPAll.Value))
                {
                    var players = gameWorld.RegisteredPlayers.Where(x => !x.IsYourPlayer);
                    foreach (var player in players)
                    {
                        player.Teleport(LocalPlayer.Transform.position + new Vector3(0, 10, 0));
                    }
                }

                if (Input.GetKeyDown(TPScav.Value))
                {
                    var players = gameWorld.RegisteredPlayers.Where(x => !x.IsYourPlayer);
                    foreach (var player in players)
                    {
                        if (player.Profile.Side == EPlayerSide.Savage)
                        {
                            player.Teleport(LocalPlayer.Transform.position);
                        }
                    }
                }

                if (Input.GetKeyDown(TPUsec.Value))
                {
                    var players = gameWorld.RegisteredPlayers.Where(x => !x.IsYourPlayer);
                    foreach (var player in players)
                    {
                        if (player.Profile.Side == EPlayerSide.Usec)
                        {
                            player.Teleport(LocalPlayer.Transform.position);
                        }
                    }
                }

                if (Input.GetKeyDown(TPBear.Value))
                {
                    var players = gameWorld.RegisteredPlayers.Where(x => !x.IsYourPlayer);
                    foreach (var player in players)
                    {
                        if (player.Profile.Side == EPlayerSide.Bear)
                        {
                            player.Teleport(LocalPlayer.Transform.position);
                        }
                    }
                }

                if (Input.GetKeyDown(TPCrate.Value))
                {
                    foreach (var crate in LocationScene.GetAllObjects<SynchronizableObject>())
                    {
                        crate.transform.position = LocalPlayer.Transform.position;
                    }
                }

                if (NoRagdollStop.Value)
                {
                    EFTHardSettings.Instance.DEBUG_CORPSE_PHYSICS = NoRagdollStop.Value;
                }
            }
        }
    }
}