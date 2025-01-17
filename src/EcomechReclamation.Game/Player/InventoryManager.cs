using System.Collections.Generic;

namespace EcomechReclamation.Player;

internal class InventoryManager
{
    private const int MAX_SLOTS = 10;

    public int SlotCount => Slots.Count;
    public bool IsFull => Slots.Count >= MAX_SLOTS;

    private List<Slot> Slots { get; } = []; // Ordered slots.
    private Dictionary<string, int> ItemSlots { get; } = []; // Lookup slots: <item, Slots index>.

    /// <summary>
    /// Adds an item to an inventory slot if possible.
    /// </summary>
    /// <param name="itemName">The item to add.</param>
    /// <returns>True if the item was added. False otherwise.</returns>
    public bool TryAddItem(string itemName, out Slot slot)
    {
        slot = default;

        if (Slots.Count >= MAX_SLOTS)
        {
            // Inventory is full.
            return false;
        }

        if (ItemSlots.TryGetValue(itemName, out int slotIndex))
        {
            // Add to existing slot.
            slot = Slots[slotIndex];
            slot.ItemCount++;
            return true;
        }

        // Add item to new slot.
        slot = new(itemName);
        Slots.Add(slot);
        ItemSlots.Add(itemName, Slots.Count - 1);

        return true;
    }

    /// <summary>
    /// Gets inventory slots.
    /// </summary>
    /// <returns>Inventory slots.</returns>
    public IEnumerable<Slot> GetSlots() => Slots;
}

internal class Slot(string itemName)
{
    public string ItemName { get; init; } = itemName;
    public int ItemCount { get; set; } = 1;
}