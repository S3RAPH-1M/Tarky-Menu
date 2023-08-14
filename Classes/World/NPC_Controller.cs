using BepInEx.Configuration;
using Comfort.Common;
using EFT;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Tarky_Menu.Entry;

namespace Tarky_Menu.Classes.World
{
    internal class NPC_Controller
    {
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> TPAll { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> TPBoss { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> TPUsec { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> TPBear { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> TPScav { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> TPCrosshair { get; private set; }
        public ConfigEntry<bool> TPCrosshairValue { get; private set; }


        public void Awake()
        {
            this.TPAll = Instance.Config.Bind("World | AI", "TP All AI To U", new BepInEx.Configuration.KeyboardShortcut());
            this.TPBoss = Instance.Config.Bind("World | AI", "TP All Bosses To U", new BepInEx.Configuration.KeyboardShortcut());
            this.TPUsec = Instance.Config.Bind("World | AI", "TP All Usec To You", new BepInEx.Configuration.KeyboardShortcut());
            this.TPBear = Instance.Config.Bind("World | AI", "TP All Bear To You", new BepInEx.Configuration.KeyboardShortcut());
            this.TPScav = Instance.Config.Bind("World | AI", "TP All Scav To You", new BepInEx.Configuration.KeyboardShortcut());
            this.TPCrosshair = Instance.Config.Bind("World | AI", "TP AI To Crosshair", new BepInEx.Configuration.KeyboardShortcut());
            this.TPCrosshairValue = Instance.Config.Bind("World | AI", "TP AI To Crosshair Bool", false);
        }


        public void Update()
        {
            GameWorld gameWorld = Singleton<GameWorld>.Instance;

            if (Instance.LocalPlayer != null && Instance.LocalPlayer.ActiveHealthController != null)
            {
                if (TPCrosshair.Value.IsDown())
                {
                    TPCrosshairValue.Value = !TPCrosshairValue.Value;
                }

                if (TPAll.Value.IsDown())
                {
                    Vector3 position = Instance.LocalPlayer.Transform.position;
                    if (TPCrosshairValue.Value && Instance.LocalPlayer.HandsController is Player.FirearmController firearmcontroller && Physics.Raycast(new Ray(firearmcontroller.Fireport.position, firearmcontroller.WeaponDirection), out RaycastHit Hit, Single.MaxValue, GClass2764.HitMask))
                    {
                        position = Hit.point;
                    }

                    IEnumerable<Player> players = gameWorld.AllAlivePlayersList.Where(x => !x.IsYourPlayer);
                    foreach (Player player in players)
                    {
                        player.Teleport(position);
                    }
                }

                if (TPScav.Value.IsDown())
                {
                    Vector3 position = Instance.LocalPlayer.Transform.position;
                    if (TPCrosshairValue.Value && Instance.LocalPlayer.HandsController is Player.FirearmController firearmcontroller && Physics.Raycast(new Ray(firearmcontroller.Fireport.position, firearmcontroller.WeaponDirection), out RaycastHit Hit, Single.MaxValue, GClass2764.HitMask))
                    {
                        position = Hit.point;
                    }

                    IEnumerable<Player> players = gameWorld.AllAlivePlayersList.Where(x => !x.IsYourPlayer);
                    foreach (Player player in players)
                    {
                        if (player.Profile.Side == EPlayerSide.Savage)
                        {
                            player.Teleport(position);
                        }
                    }
                }

                if (TPUsec.Value.IsDown())
                {
                    Vector3 position = Instance.LocalPlayer.Transform.position;
                    if (TPCrosshairValue.Value && Instance.LocalPlayer.HandsController is Player.FirearmController firearmcontroller && Physics.Raycast(new Ray(firearmcontroller.Fireport.position, firearmcontroller.WeaponDirection), out RaycastHit Hit, Single.MaxValue, GClass2764.HitMask))
                    {
                        position = Hit.point;
                    }

                    IEnumerable<Player> players = gameWorld.AllAlivePlayersList.Where(x => !x.IsYourPlayer);
                    foreach (Player player in players)
                    {
                        if (player.Profile.Side == EPlayerSide.Usec)
                        {
                            player.Teleport(position);
                        }
                    }
                }

                if (TPBear.Value.IsDown())
                {
                    Vector3 position = Instance.LocalPlayer.Transform.position;
                    if (TPCrosshairValue.Value && Instance.LocalPlayer.HandsController is Player.FirearmController firearmcontroller && Physics.Raycast(new Ray(firearmcontroller.Fireport.position, firearmcontroller.WeaponDirection), out RaycastHit Hit, Single.MaxValue, GClass2764.HitMask))
                    {
                        position = Hit.point;
                    }

                    IEnumerable<Player> players = gameWorld.AllAlivePlayersList.Where(x => !x.IsYourPlayer);
                    foreach (Player player in players)
                    {
                        if (player.Profile.Side == EPlayerSide.Bear)
                        {
                            player.Teleport(position);
                        }
                    }
                }

                if (TPBoss.Value.IsDown())
                {
                    Vector3 position = Instance.LocalPlayer.Transform.position;
                    if (TPCrosshairValue.Value && Instance.LocalPlayer.HandsController is Player.FirearmController firearmcontroller && Physics.Raycast(new Ray(firearmcontroller.Fireport.position, firearmcontroller.WeaponDirection), out RaycastHit Hit, Single.MaxValue, GClass2764.HitMask))
                    {
                        position = Hit.point;
                    }

                    IEnumerable<Player> players = gameWorld.AllAlivePlayersList.Where(x => !x.IsYourPlayer);
                    foreach (Player player in players)
                    {
                        if (player.AIData.IAmBoss == true)
                        {
                            player.Teleport(position);
                        }
                    }
                }
            }
        }
    }
}
