using Comfort.Common;
using EFT;
using EFT.InventoryLogic;
using System;
using System.Reflection;

namespace Tarky_Menu.Classes
{
    public static class Utils
    {

        public static Inventory GetInventory(this Player player)
        {
            return typeof(Player).GetProperty("Inventory", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(player) as Inventory;
        }

        public static InventoryControllerClass GetInventoryController(this Player player)
        {
            return typeof(Player).GetField("_inventoryController", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(player) as InventoryControllerClass;
        }

        public static bool ContainsIgnoreCase(this string source, string toCheck, StringComparison comp = StringComparison.InvariantCultureIgnoreCase)
        {
            return source != null && source.IndexOf(toCheck, comp) >= 0;
        }


        public static bool IsOfType(ItemTemplate template, string parentId)
        {
            var itemTemplates = Singleton<ItemFactory>.Instance.ItemTemplates;
            while (!string.IsNullOrEmpty(template._parent))
            {
                bool flag = template._parent == parentId;
                if (flag)
                {
                    return true;
                }
                template = itemTemplates[template._parent];
            }
            return false;
        }


        public static string GenerateId()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 24);
        }

        public static T CreateItem<T>(string templateId, string itemId = null) where T : Item
        {
            bool instantiated = Singleton<ItemFactory>.Instantiated;
            if (instantiated)
            {
                ItemFactory instance = Singleton<ItemFactory>.Instance;
                bool flag = instance.ItemTemplates.ContainsKey(templateId);
                if (flag)
                {
                    bool flag2 = itemId == null;
                    if (flag2)
                    {
                        itemId = Utils.GenerateId();
                    }
                    return instance.CreateItem(itemId, templateId, null) as T;
                }
            }
            return default(T);
        }
    }
}
