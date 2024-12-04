using EcomechReclamation.Player;
using Stride.Engine;
using Stride.Engine.Events;

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

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
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
            DebugText.Print($"Inventory: {Nums}", new(500, 200));
        }
    }
}
