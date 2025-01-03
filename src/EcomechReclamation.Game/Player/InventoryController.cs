using EcomechReclamation.Player;
using Stride.Engine;
using Stride.Engine.Events;
using Stride.UI;
using Stride.UI.Controls;
using System.Linq;

namespace EcomechReclamation;

public class InventoryController : SyncScript
{
    /// <summary>
    /// Receives <see cref="PlayerInput.ToggleInventoryEventKey"/> events.
    /// </summary>
    private EventReceiver ToggleInventoryEvent { get; init; } = new(PlayerInput.ToggleInventoryEventKey);

    /// <summary>
    /// Receives <see cref="CollectEntityEventKey"/> events.
    /// </summary>
    private EventReceiver<Entity> CollectEntityEvent { get; init; } = new(PlayerController.CollectEntityEventKey);

    /// <summary>
    /// Inventory grid UI.
    /// </summary>
    private UIElement InventoryUI { get; set; }

    public override void Start()
    {
        base.Start();

        UIComponent playerUI = Entity.Get<UIComponent>();
        InventoryUI = playerUI.Page.RootElement.FindName(nameof(InventoryUI));
    }

    public override void Update()
    {
        CollectEntity();
        ToggleInventory();
    }

    /// <summary>
    /// Opens or closes the inventory UI.
    /// </summary>
    private void ToggleInventory()
    {
        if (!ToggleInventoryEvent.TryReceive())
        {
            return;
        }

        InventoryUI.Visibility = InventoryUI.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
    }

    private void CollectEntity()
    {
        if (!CollectEntityEvent.TryReceive(out Entity entity))
        {
            return;
        }

        UIElement slot = InventoryUI.VisualChildren.FirstOrDefault(child => !child.IsVisible);

        if (slot == null)
        {
            // Inventory is full.
            return;
        }

        slot.FindVisualChildOfType<TextBlock>().Text = entity.Name;
        slot.Visibility = Visibility.Visible;
    }
}
