using System.Collections.Generic;

namespace EcomechReclamation.Player
{
    internal class InventoryManager
    {
        private const int MAX_SLOTS = 10;

        public int SlotCount => Slots.Count;
        public bool IsFull => Slots.Count >= MAX_SLOTS;

        private List<Slot> Slots { get; } = [];
        private Dictionary<string, Slot> ItemSlots = [];

        public void AddItem(string item)
        {
            if (Slots.Count >= MAX_SLOTS) { return; } // Inventory is full.

            if (ItemSlots.TryGetValue(item, out Slot slot))
            {
                // Add to existing slot.
                slot.ItemCount++;
                return;
            }

            // Add item to new slot.
            Slots.Add(new(item, 1));
        }

        public IEnumerable<Slot> GetSlots() => Slots;
    }

    internal record struct Slot(string Item, int ItemCount)
    {
    }
}
