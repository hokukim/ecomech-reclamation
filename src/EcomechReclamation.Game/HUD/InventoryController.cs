using EcomechReclamation.Player;
using Stride.Engine;
using Stride.Engine.Events;
using System.Collections.Generic;
using System.Text;

namespace EcomechReclamation.HUD
{
    public class InventoryController : SyncScript
    {
        private int Nums = 0;

        /// <summary>
        /// Indicates if the inventory UI is open.
        /// </summary>
        private bool IsOpen = false;

        /// <summary>
        /// Receives <see cref="PlayerInput.ToggleInventoryEventKey"/> events.
        /// </summary>
        private EventReceiver ToggleInventoryEvent { get; init; } = new(PlayerInput.ToggleInventoryEventKey);

        private EventReceiver<Entity> CollectEntityEvent { get; init; } = new(PlayerController.CollectEntityEventKey);

        /// <summary>
        /// Items in the inventory.
        /// </summary>
        private List<Entity> Collectibles { get; } = [];

        public override void Start()
        {
            base.Start();
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
                IsOpen = !IsOpen;

                if (IsOpen) { Nums++; }
            }

            if (!IsOpen)
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
        }
    }
}
