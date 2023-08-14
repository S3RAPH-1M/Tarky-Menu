using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Tarky_Menu.Entry;

namespace Tarky_Menu.Classes.PlayerStats
{
    internal class Health
    {
        private static String[] TargetBones = { "head", "spine3", "spine2", "spine1" };
        public ConfigEntry<Boolean> Godmode { get; private set; }
        public ConfigEntry<Boolean> Demigod { get; private set; }
        public ConfigEntry<float> DamageMultiplier { get; private set; }
        public Boolean Heal { get; private set; }
        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> HealButton { get; private set; }
        public ConfigEntry<Boolean> NoFall { get; private set; }
        public ConfigEntry<Boolean> HungerEnergyDrain { get; private set; }
        public ConfigEntry<Boolean> InfiniteHealthBoost { get; private set; }
        public Boolean HasDoneGodMode { get; private set; }
        public Boolean HasDoneNoFall { get; private set; }
        public float originalFallValue { get; private set; }



        public void Awake()
        {
            this.Godmode = Instance.Config.Bind("Player | Health", "Godmode", false, "Invincible");
            this.Demigod = Instance.Config.Bind("Player | Health", "Demi-God", false, "Only ur head and thorax are invincible");
            this.DamageMultiplier = Instance.Config.Bind("Player | Health", "Damage Multiplier", 1f);
            this.HealButton = Instance.Config.Bind("Player | Health", "Heal", new BepInEx.Configuration.KeyboardShortcut());
            this.NoFall = Instance.Config.Bind("Player | Health", "No Fall Damage", false);
            this.HungerEnergyDrain = Instance.Config.Bind("Player | Health", "No Energy/Hunger Drain", false);
        }

        public void godMod()
        {
            if (Instance.LocalPlayer != null && Instance.LocalPlayer.ActiveHealthController != null)
            {
                if (Godmode.Value)
                {
                    if (Instance.LocalPlayer.ActiveHealthController.DamageCoeff != -1f)
                    {
                        Instance.LocalPlayer.ActiveHealthController.SetDamageCoeff(-1f);
                        Instance.LocalPlayer.ActiveHealthController.RemoveNegativeEffects(EBodyPart.Common);
                        Instance.LocalPlayer.ActiveHealthController.RestoreFullHealth();
                    }
                    if (Instance.LocalPlayer.ActiveHealthController.FallSafeHeight != 9999999f)
                    {
                        Instance.LocalPlayer.ActiveHealthController.FallSafeHeight = 9999999f;
                    }
                }
                if (!Godmode.Value)
                {
                    if (Instance.LocalPlayer.ActiveHealthController.DamageCoeff != 1f)
                    {
                        Instance.LocalPlayer.ActiveHealthController.SetDamageCoeff(1f);
                        Instance.LocalPlayer.ActiveHealthController.FallSafeHeight = 1f;
                    }
                }

                if (Demigod.Value)
                {
                    var HeadHP = Instance.LocalPlayer.ActiveHealthController.GetBodyPartHealth(EBodyPart.Head, true);
                    var ChestHP = Instance.LocalPlayer.ActiveHealthController.GetBodyPartHealth(EBodyPart.Chest, true);

                    // if ChestHP.Current is less than 45

                    if (ChestHP.Current < ChestHP.Maximum / 2)
                    {
                        Instance.LocalPlayer.ActiveHealthController.RemoveNegativeEffects(EBodyPart.Chest);
                        Instance.LocalPlayer.ActiveHealthController.ChangeHealth(EBodyPart.Chest, ChestHP.Maximum, default);
                    }
                    if (HeadHP.Current < HeadHP.Maximum / 2)
                    {
                        Instance.LocalPlayer.ActiveHealthController.RemoveNegativeEffects(EBodyPart.Head);
                        Instance.LocalPlayer.ActiveHealthController.ChangeHealth(EBodyPart.Head, HeadHP.Maximum, default);
                    }

                    if (Instance.HasDemiGodRan == false)
                    {
                        foreach (Transform transform in Health.EnumerateHierarchyCore(Entry.Instance.LocalPlayer.gameObject.transform).Where(t => TargetBones.Any(u => t.name.ToLower().Contains(u))))
                        {
                            if (transform.gameObject.layer != LayerMask.NameToLayer("PlayerSpiritAura"))
                            {
                                transform.gameObject.layer = LayerMask.NameToLayer("PlayerSpiritAura");
                                Instance.HasDemiGodRan = true;
                            }
                        }
                    }
                }

                if (NoFall.Value && HasDoneNoFall == false)
                {
                    originalFallValue = Instance.LocalPlayer.ActiveHealthController.FallSafeHeight;
                    Instance.LocalPlayer.ActiveHealthController.FallSafeHeight = 9999999f;
                    HasDoneNoFall = true;
                }

                if (!NoFall.Value && HasDoneNoFall == true)
                {
                    Instance.LocalPlayer.ActiveHealthController.FallSafeHeight = originalFallValue;
                    HasDoneNoFall = false;
                }

                if (HealButton.Value.IsDown())
                {
                    Heal = true;
                    if (Heal == true)
                    {
                        Instance.LocalPlayer.ActiveHealthController.RemoveNegativeEffects(EBodyPart.Common);
                        Instance.LocalPlayer.ActiveHealthController.RestoreFullHealth();
                        Heal = false;
                    }
                }

                if (HungerEnergyDrain.Value)
                {
                    Instance.LocalPlayer.ActiveHealthController.ChangeEnergy(100f);
                    Instance.LocalPlayer.ActiveHealthController.ChangeHydration(100f);
                }

            }
        }

        private static IEnumerable<Transform> EnumerateHierarchyCore(Transform root)
        {
            Queue<Transform> transformQueue = new Queue<Transform>();
            transformQueue.Enqueue(root);

            while (transformQueue.Count > 0)
            {
                Transform parentTransform = transformQueue.Dequeue();

                if (!parentTransform)
                {
                    continue;
                }

                for (Int32 i = 0; i < parentTransform.childCount; i++)
                {
                    transformQueue.Enqueue(parentTransform.GetChild(i));
                }

                yield return parentTransform;
            }
        }
    }
}
