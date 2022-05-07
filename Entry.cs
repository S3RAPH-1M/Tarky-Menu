﻿using System;
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
using Tarky_Menu.Classes;
using Tarky_Menu.Classes.PlayerStats;
using Tarky_Menu.Classes.Misc;
using System.Diagnostics;

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

        public Player LocalPlayer { get; private set; }

        public static Entry Instance { get; private set; }
        public ConfigEntry<Boolean> HungerEnergyDrain { get; private set; }
        public ConfigEntry<Boolean> StrengthControl { get; private set; }
        public ConfigEntry<Single> StrengthValue { get; private set; }
        public ConfigEntry<KeyCode> KillSelf { get; private set; }
        public ConfigEntry<Boolean> KillAll { get; private set; }
        public ConfigEntry<Boolean> NightVision { get; private set; }
        public ConfigEntry<Boolean> Speedhack { get; private set; }
        public ConfigEntry<KeyCode> FOVButton { get; private set; }
        void Awake()
        {
            Instance = this;
            this.NightVision = this.Config.Bind("Cheats", "NVG Toggle", false, "Toggle NVG");
            this.Speedhack = this.Config.Bind("Cheats", "Speedhack", false, "");
            this.HungerEnergyDrain = this.Config.Bind("Cheats", "No Energy/Hunger Drain", false);
            this.StrengthControl = this.Config.Bind("Cheats", "Strength Control", false);
            this.StrengthValue = this.Config.Bind("Cheats", "Strength Value", 1f);
            this.KillSelf = this.Config.Bind("Misc | Random Test Stuff", "Kill Yourself", KeyCode.None, "This is so sad :(");
            this.KillAll = this.Config.Bind("Misc | Random Test Stuff", "Kill all", false, "Genocide :D");
            _recoilControlSystem = new Classes.RecoilControlSystem();
            _skillzClass = new Classes.PlayerStats.SkillzClass();
            _health = new Classes.PlayerStats.Health();
            _cameraUtils = new Classes.Misc.CameraUtils();
            _worldUtils = new Classes.Misc.WorldUtils();
            _recoilControlSystem.Awake();
            _skillzClass.Awake();
            _health.Awake();
            _cameraUtils.Awake();
            _worldUtils.Awake();

            EFT.UI.ConsoleScreen.Commands.Add(new GClass2285("quit", _ => { Process.GetCurrentProcess().Kill(); }));
            EFT.UI.ConsoleScreen.Commands.Add(new GClass2285("q", _ => { Process.GetCurrentProcess().Kill(); }));
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
            _worldUtils.DoorUnlocker();

            if (Instance.LocalPlayer != null && Instance.LocalPlayer.ActiveHealthController != null)
            {
                if (Input.GetKeyDown(KillSelf.Value))
                {
                    LocalPlayer.ActiveHealthController.ApplyDamage(EBodyPart.Head, 99999, default);
                }

                if (KillAll.Value)
                {
                    var players = gameWorld.RegisteredPlayers.Where(x => x != LocalPlayer);
                    foreach (var player in players)
                    {
                        player.ActiveHealthController.Kill(EDamageType.Existence);
                    }
                }
            }



        }
    }
}