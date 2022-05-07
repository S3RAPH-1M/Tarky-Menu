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

namespace Tarky_Menu.Classes.Misc
{
    internal class WorldUtils
    {
        public ConfigEntry<KeyCode> DoorUnlock { get; private set; }

        public void Awake()
        {
            this.DoorUnlock = Instance.Config.Bind("Cheats", "Unlock Doors", KeyCode.None);
        }

        public void DoorUnlocker()
        {



            if (Input.GetKeyDown(this.DoorUnlock.Value))
            {
                EFT.Interactive.Door[] doors = UnityEngine.Object.FindObjectsOfType<EFT.Interactive.Door>();

                foreach (EFT.Interactive.Door door in doors)
                {
                   if (door.DoorState == EFT.Interactive.EDoorState.Locked)
                    {
                        door.DoorState = EFT.Interactive.EDoorState.Shut;
                    }
                }

            }
        }

    }
}
