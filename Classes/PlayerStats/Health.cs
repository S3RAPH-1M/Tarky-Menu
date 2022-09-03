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

namespace Tarky_Menu.Classes.PlayerStats
{
    internal class Health
    {
        private static String[] TargetBones = {"head", "spine3", "spine2", "spine1"};
        
        public ConfigEntry<Boolean> Godmode { get; private set; }
        public ConfigEntry<Boolean> Demigod { get; private set; }
        public ConfigEntry<KeyCode> Heal { get; private set; }
        public ConfigEntry<Boolean> NoFall { get; private set; }
        public ConfigEntry<Boolean> HungerEnergyDrain { get; private set; }
        public ConfigEntry<Boolean> InfiniteHealthBoost { get; private set; }



        public void Awake()
        {
            this.Godmode = Instance.Config.Bind("Player | Health", "Godmode", false, "Invincible");
            this.Demigod = Instance.Config.Bind("Player | Health", "Demi-God", false, "Only ur head and thorax are invincible");
            this.Heal = Instance.Config.Bind("Player | Health", "Heal", KeyCode.None);
            this.NoFall = Instance.Config.Bind("Player | Health", "No Fall Damage", false);
            this.HungerEnergyDrain = Instance.Config.Bind("Player | Health", "No Energy/Hunger Drain", false);
            this.InfiniteHealthBoost = Instance.Config.Bind("Player | Health", "Permanent Heal Boost", false);
        }

        public void godMod()
        {
            if (Instance.LocalPlayer != null && Instance.LocalPlayer.ActiveHealthController != null)
            {
                if (Godmode.Value)
                {
                    Instance.LocalPlayer.ActiveHealthController.SetDamageCoeff(-1f);
                    Instance.LocalPlayer.ActiveHealthController.RemoveNegativeEffects(EBodyPart.Common);
                    Instance.LocalPlayer.ActiveHealthController.RestoreFullHealth();
                }
                else
                {
                    Instance.LocalPlayer.ActiveHealthController.SetDamageCoeff(1f);
                }

                if (Demigod.Value)
                {
                    Instance.LocalPlayer.ActiveHealthController.RemoveNegativeEffects(EBodyPart.Head);
                    Instance.LocalPlayer.ActiveHealthController.RemoveNegativeEffects(EBodyPart.Chest);
                    Instance.LocalPlayer.ActiveHealthController.ChangeHealth(EBodyPart.Head, 2147483640, new DamageInfo());
                    Instance.LocalPlayer.ActiveHealthController.ChangeHealth(EBodyPart.Chest, 2147483640, new DamageInfo());
                    foreach (Transform child in EnumerateHierarchyCore(Instance.LocalPlayer.gameObject.transform).Where(t => TargetBones.Any(u => t.name.ToLower().Contains(u))))
                    {
                        child.gameObject.layer = LayerMask.NameToLayer("PlayerSpiritAura");
                    }
                }

                if (NoFall.Value)
                {
                    Instance.LocalPlayer.ActiveHealthController.FallSafeHeight = 9999999f;
                }
                else
                {
                    Instance.LocalPlayer.ActiveHealthController.FallSafeHeight = 3;
                }

                if (Input.GetKeyDown(Heal.Value))
                {
                    Instance.LocalPlayer.ActiveHealthController.RemoveNegativeEffects(EBodyPart.Common);
                    Instance.LocalPlayer.ActiveHealthController.RestoreFullHealth();
                }

                if (HungerEnergyDrain.Value)
                {
                    Instance.LocalPlayer.ActiveHealthController.ChangeEnergy(1000f);
                    Instance.LocalPlayer.ActiveHealthController.ChangeHydration(1000f);
                }

                if (InfiniteHealthBoost.Value)
                {

                    Instance.LocalPlayer.ActiveHealthController.DoPermanentHealthBoost(500000f);

                }
            }
        }

        private static IEnumerable<Transform> EnumerateHierarchyCore(Transform root) {
            Queue<Transform> transformQueue = new Queue<Transform>();
            transformQueue.Enqueue(root);

            while (transformQueue.Count > 0) {
                Transform parentTransform = transformQueue.Dequeue();

                if (!parentTransform) {
                    continue;
                }

                for (Int32 i = 0; i < parentTransform.childCount; i++) {
                    transformQueue.Enqueue(parentTransform.GetChild(i));
                }

                yield return parentTransform;
            }
        }
    }
}
