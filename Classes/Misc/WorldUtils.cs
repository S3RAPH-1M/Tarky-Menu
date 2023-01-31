using BepInEx.Configuration;
using EFT.Interactive;
using System;
using System.Reflection;
using static Tarky_Menu.Entry;

namespace Tarky_Menu.Classes.Misc
{
    internal class WorldUtils
    {
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> DoorUnlock { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> PowerButton { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> DoorKicker { get; private set; }
        public ConfigEntry<Boolean> AllKeys { get; private set; }
        public ConfigEntry<Boolean> Noclip { get; private set; }


        private static FieldInfo _additionalKeys;
        private static string[] _additionalKeysArray = { "5c1d0d6d86f7744bb2683e1f", "5c1d0efb86f7744baf2e7b7b", "5c1d0c5f86f7744bb2683cf0", "5c1e495a86f7743109743dfb", "5c1d0dc586f7744baf2e7b79", "5c1d0f4986f7744bb01837fa", "5c94bbff86f7747ee735c08f" };


        public void Awake()
        {
            this.DoorUnlock = Instance.Config.Bind("World | Misc", "Unlock Doors", new BepInEx.Configuration.KeyboardShortcut());
            this.PowerButton = Instance.Config.Bind("World | Misc", "Turn On Power", new BepInEx.Configuration.KeyboardShortcut());
            this.DoorKicker = Instance.Config.Bind("World | Misc", "Breach Any Door", new BepInEx.Configuration.KeyboardShortcut(), "HERES JOHNNY!!!");
            this.AllKeys = Instance.Config.Bind("World | Misc", "All Keycards Work On Labs", false);

        }

        public void DoorUnlocker()
        {

            if (DoorUnlock.Value.IsDown())
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

            if (DoorKicker.Value.IsDown())
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

