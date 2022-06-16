using System;
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
using static Tarky_Menu.Entry;
using EFT.Interactive;

namespace Tarky_Menu.Classes.Misc
{
    internal class WorldUtils
    {
        public ConfigEntry<KeyCode> DoorUnlock { get; private set; }
        public ConfigEntry<KeyCode> DoorKicker { get; private set; }
        public ConfigEntry<Boolean> AllKeys { get; private set; }

        
        private static FieldInfo _additionalKeys;
        private static string[] _additionalKeysArray = { "5c1d0d6d86f7744bb2683e1f", "5c1d0efb86f7744baf2e7b7b", "5c1d0c5f86f7744bb2683cf0", "5c1e495a86f7743109743dfb", "5c1d0dc586f7744baf2e7b79", "5c1d0f4986f7744bb01837fa", "5c94bbff86f7747ee735c08f" };


        public void Awake()
        {
            this.DoorUnlock = Instance.Config.Bind("World | Misc", "Unlock Doors", KeyCode.None);
            this.DoorKicker = Instance.Config.Bind("World | Misc", "Breach Any Door", KeyCode.None, "HERES JOHNNY!!!");
            this.AllKeys = Instance.Config.Bind("World | Misc", "All Keycards Work On Labs", false);

        }

        public void DoorUnlocker()
        {

            if (Input.GetKeyDown(this.DoorUnlock.Value))
            {
                var worldInteractiveObjects = LocationScene.GetAll<WorldInteractiveObject>();

                foreach (var worldInteractiveObject in worldInteractiveObjects)
                {
                    if (worldInteractiveObject is Door door)
                    {
                        if (door.DoorState == EDoorState.Locked)
                        {
                            door.DoorState = EDoorState.Shut;
                        }
                    }

                    if (worldInteractiveObject is LootableContainer container)
                    {
                        if (container.DoorState == EDoorState.Locked)
                        {
                            container.DoorState = EDoorState.Shut;
                        }
                    }

                    if (worldInteractiveObject is Trunk trunk)
                    {
                        if (trunk.DoorState == EDoorState.Locked)
                        {
                            trunk.DoorState = EDoorState.Shut;
                        }
                    }
                }
            }

            if (Input.GetKeyDown(this.DoorKicker.Value))
            {
                var worldInteractiveObjects = LocationScene.GetAll<WorldInteractiveObject>();

                foreach (var worldInteractiveObject in worldInteractiveObjects)
                {
                    if (worldInteractiveObject is Door door)
                    {
                        door.CanBeBreached = true;
                    }
                }
            }
        }
        
        public void MiscWorldUtilities()
        {
            if (AllKeys.Value)
            {
                var KCDoors = LocationScene.GetAll<WorldInteractiveObject>();
                if (_additionalKeys == null)
                {
                    _additionalKeys = typeof(EFT.Interactive.KeycardDoor).GetField("_additionalKeys", BindingFlags.NonPublic | BindingFlags.Instance);
                }

                foreach (var KCdoor in KCDoors)
                {
                    if (KCdoor is KeycardDoor)
                    {
                        _additionalKeys.SetValue(KCdoor, _additionalKeysArray);

                    }
                }
            }
            
            
        }

    }
}


