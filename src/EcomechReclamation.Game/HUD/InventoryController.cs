using EcomechReclamation.Player;
using Stride.Engine;
using Stride.Engine.Events;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Panels;
using System.Collections.Generic;
using System.Text;

namespace EcomechReclamation.HUD
{
    public class InventoryController : SyncScript
    {
        /// <summary>
        /// Indicates if the inventory UI is open.
        /// </summary>
        private bool IsOpen = false;

        /// <summary>
        /// Receives <see cref="PlayerInput.ToggleInventoryEventKey"/> events.
        /// </summary>
        private EventReceiver ToggleInventoryEvent { get; init; } = new(PlayerInput.ToggleInventoryEventKey);

        /// <summary>
        /// Receives <see cref="CollectEntityEventKey"/> events.
        /// </summary>
        private EventReceiver<Entity> CollectEntityEvent { get; init; } = new(PlayerController.CollectEntityEventKey);

        /// <summary>
        /// Items in the inventory.
        /// </summary>
        private List<Entity> Collectibles { get; } = [];

        /// <summary>
        /// Inventory grid UI.
        /// </summary>
        private Grid InventoryGrid { get; set; }

        public override void Start()
        {
            base.Start();

            UIComponent playerUI = Entity.Get<UIComponent>();
            InventoryGrid = (Grid)playerUI.Page.RootElement.FindName(nameof(InventoryGrid));
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
            if (ToggleInventoryEvent.TryReceive())
            {
                InventoryGrid.Visibility = InventoryGrid.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            }

            if (InventoryGrid.Visibility == Visibility.Hidden)
            {
                return;
            }

            // Draw inventory.
            StringBuilder print = new();
            print.Append("Inventory:");
            foreach (Entity entity in Collectibles)
            {
                print.Append($"\n{entity.Name}");
            }

            DebugText.Print(print.ToString(), new(500, 200));
        }

        private void CollectEntity()
        {
            if (!CollectEntityEvent.TryReceive(out Entity entity))
            {
                return;
            }

            Collectibles.Add(entity);
            TextBlock textBlock = (TextBlock)InventoryGrid.Children[0];
            textBlock.Text = entity.Name;

            InventoryGrid.Children.Add(textBlock);
        }
    }
}
