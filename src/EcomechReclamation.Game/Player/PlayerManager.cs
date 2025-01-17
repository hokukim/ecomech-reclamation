namespace EcomechReclamation.Player;

internal class PlayerManager
{
    public static PlayerManager Instance { get; } = new();

    public InventoryManager Inventory { get; } = new();

    private PlayerManager() { }
}
