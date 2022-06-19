using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using Comfort.Common;
using EFT;
using EFT.Interactive;
using EFT.UI;
using Tarky_Menu.Classes;
using Tarky_Menu.Classes.Misc;
using Tarky_Menu.Classes.PlayerStats;
using Tarky_Menu.Classes.Weapons;
using UnityEngine;

namespace Tarky_Menu {
	[BepInPlugin(MOD_GUID, MOD_NAME, MOD_VERSION)]
	[BepInProcess("EscapeFromTarkov.exe")]
	public class Entry : BaseUnityPlugin {
		private const String MOD_GUID = "me.ssh.tarkymenu";
		private const String MOD_NAME = "SERVPH's Tarky Menu";
		private const String MOD_VERSION = "1.0";
		private CameraUtils _cameraUtils;
		private Health _health;

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
		public ConfigEntry<KeyCode> KillSelf { get; private set; }
		public ConfigEntry<Boolean> KillAll { get; private set; }
		public ConfigEntry<KeyCode> FOVButton { get; private set; }
		public ConfigEntry<KeyCode> TPAll { get; private set; }
		public ConfigEntry<KeyCode> TPUsec { get; private set; }
		public ConfigEntry<KeyCode> TPBear { get; private set; }
		public ConfigEntry<KeyCode> TPScav { get; private set; }
		public ConfigEntry<KeyCode> TPCrate { get; private set; }
		public ConfigEntry<Boolean> NoRagdollStop { get; private set; }
		public ConfigEntry<KeyCode> MineRemover { get; private set; }
		public ConfigEntry<KeyCode> SniperRemover { get; private set; }

		private void Awake() {
			Instance = this;
			this.KillSelf = this.Config.Bind("Player | Health", "Kill Yourself", KeyCode.None, "This is so sad :(");
			this.KillAll = this.Config.Bind("World | AI", "Kill all", false, "Genocide :D");
			this.TPAll = this.Config.Bind("World | AI", "TP All 2 U", KeyCode.None);
			this.TPCrate = this.Config.Bind("World | AI", "TP Crates To You", KeyCode.None);
			this.TPUsec = this.Config.Bind("World | AI", "TP All Usec To You", KeyCode.None);
			this.TPBear = this.Config.Bind("World | AI", "TP All Bear To You", KeyCode.None);
			this.TPScav = this.Config.Bind("World | AI", "TP All Scav To You", KeyCode.None);
			this.NoRagdollStop = this.Config.Bind("World | AI", "Ragdolls Never Freeze", false, "Has virtually no impact on performance, And looks really good :)");
			this.MineRemover = this.Config.Bind("World | Misc", "Mine Remover", KeyCode.None);
			this.SniperRemover = this.Config.Bind("World | Misc", "Sniper Remover", KeyCode.None);
			this._recoilControlSystem = new RecoilControlSystem();
			this._skillzClass = new SkillzClass();
			this._health = new Health();
			this._cameraUtils = new CameraUtils();
			this._worldUtils = new WorldUtils();
			this._weaponUtils = new WeaponUtils();
			this._recoilControlSystem.Awake();
			this._skillzClass.Awake();
			this._health.Awake();
			this._cameraUtils.Awake();
			this._worldUtils.Awake();
			this._weaponUtils.Awake();

			ConsoleScreen.Commands.Add(new GClass2285("quit", _ => { Process.GetCurrentProcess().Kill(); }));
			ConsoleScreen.Commands.Add(new GClass2285("q", _ => { Process.GetCurrentProcess().Kill(); }));
		}

		private void Update() {
			if (!Singleton<GameWorld>.Instantiated) {
				this.LocalPlayer = null;
				return;
			}

			GameWorld gameWorld = Singleton<GameWorld>.Instance;

			if (this.LocalPlayer == null && gameWorld.RegisteredPlayers.Count > 0) {
				this.LocalPlayer = gameWorld.RegisteredPlayers[0];
				return;
			}
			
			if (this.HideoutPlayer == null && gameWorld.RegisteredPlayers.Count > 0) {
				this.HideoutPlayer = gameWorld.RegisteredPlayers[0];
				return;
			}

			this._skillzClass.Stamina();
			this._skillzClass.PlayerStats();
			this._recoilControlSystem.NoRecoil();
			this._health.godMod();
			this._cameraUtils.CameraStuff();
			this._cameraUtils.NightVisionHax();
			this._worldUtils.DoorUnlocker();
			this._worldUtils.MiscWorldUtilities();
			this._weaponUtils.FireMod();

			if (Instance.LocalPlayer != null && Instance.LocalPlayer.ActiveHealthController != null) {
				if (Input.GetKeyDown(this.KillSelf.Value)) {
					this.LocalPlayer.ActiveHealthController.Kill(EDamageType.RadExposure);
				}

				if (this.KillAll.Value) {
					IEnumerable<Player> players = gameWorld.RegisteredPlayers.Where(x => x != this.LocalPlayer);
					foreach (Player player in players) {
						player.ActiveHealthController.Kill(EDamageType.RadExposure);
					}
				}

				if (Input.GetKeyDown(this.TPAll.Value)) {
					IEnumerable<Player> players = gameWorld.RegisteredPlayers.Where(x => !x.IsYourPlayer);
					foreach (Player player in players) {
						player.Teleport(this.LocalPlayer.Transform.position);
					}
				}

				if (Input.GetKeyDown(this.TPScav.Value)) {
					IEnumerable<Player> players = gameWorld.RegisteredPlayers.Where(x => !x.IsYourPlayer);
					foreach (Player player in players) {
						if (player.Profile.Side == EPlayerSide.Savage) {
							player.Teleport(this.LocalPlayer.Transform.position);
						}
					}
				}

				if (Input.GetKeyDown(this.TPUsec.Value)) {
					IEnumerable<Player> players = gameWorld.RegisteredPlayers.Where(x => !x.IsYourPlayer);
					foreach (Player player in players) {
						if (player.Profile.Side == EPlayerSide.Usec) {
							player.Teleport(this.LocalPlayer.Transform.position);
						}
					}
				}

				if (Input.GetKeyDown(this.TPBear.Value)) {
					IEnumerable<Player> players = gameWorld.RegisteredPlayers.Where(x => !x.IsYourPlayer);
					foreach (Player player in players) {
						if (player.Profile.Side == EPlayerSide.Bear) {
							player.Teleport(this.LocalPlayer.Transform.position);
						}
					}
				}

				if (Input.GetKeyDown(this.TPCrate.Value)) {
					foreach (SynchronizableObject crate in LocationScene.GetAllObjects<SynchronizableObject>()) {
						crate.transform.position = this.LocalPlayer.Transform.position;
					}
				}

				if (this.NoRagdollStop.Value) {
					EFTHardSettings.Instance.DEBUG_CORPSE_PHYSICS = this.NoRagdollStop.Value;
				}

				if (Input.GetKeyDown(this.MineRemover.Value)) {
					Minefield minefield = FindObjectOfType<Minefield>();
					minefield.enabled = false;
				}

				if (Input.GetKeyDown(this.SniperRemover.Value)) {
					SniperFiringZone sniper = FindObjectOfType<SniperFiringZone>();
					sniper.enabled = false;
				}
			}
		}
	}
}