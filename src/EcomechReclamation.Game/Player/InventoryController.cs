using EcomechReclamation.Collisions;
using EcomechReclamation.Player;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Engine.Events;
using Stride.Graphics;
using Stride.Rendering.Sprites;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Panels;
using System;
using System.Linq;

namespace EcomechReclamation;

public class InventoryController : SyncScript
{
    /// <summary>
    /// Occurs when an item is being collected into inventory.
    /// </summary>
    public static EventKey<Entity> CollectEntityEvent = new();

    /// <summary>
    /// Receives an event when the toggle inventory event key is triggered.
    /// </summary>
    private EventReceiver ToggleInventoryEvent { get; init; } = new(PlayerInput.ToggleInventoryEventKey);

    private EventReceiver<Entity> CollectiblePlayerCollisionAddEvent { get; init; } = new(CollectibleCollisionTrigger.CollectiblePlayerAddCollisionEvenKey);
    private EventReceiver<Entity> CollectiblePlayerCollisionRemoveEvent { get; init; } = new(CollectibleCollisionTrigger.CollectiblePlayerRemoveCollisionEvenKey);
    private Entity CollisionEntity { get; set; }

    /// <summary>
    /// Receives an event when the player interact key is triggered.
    /// </summary>
    private EventReceiver InteractEvent { get; init; } = new(PlayerInput.InteractEventKey);

    /// <summary>
    /// Inventory grid UI.
    /// </summary>
    private UIElement InventoryUI { get; set; }

    Prefab FlowerPrefab { get; set; }
    private Entity FlowerPrefabEntity { get; set; } = new("flower", new Vector3(-7.178f, 0, 2.539f));

    private Grid VacantGrid { get; set; }


    public override void Start()
    {
        base.Start();

        UIComponent playerUI = Entity.Get<UIComponent>();
        InventoryUI = playerUI.Page.RootElement.FindName(nameof(InventoryUI));
        VacantGrid = GetVacantGrid();

        FlowerPrefab = Content.Load<Prefab>("Magical Flower");
        AddFlower();
    }

    public override void Update()
    {
        AddCollision();
        RemoveCollision();
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

    /// <summary>
    /// Adds an entity to collision.
    /// </summary>
    private void AddCollision()
    {
        if (!CollectiblePlayerCollisionAddEvent.TryReceive(out Entity entity))
        {
            return;
        }

        CollisionEntity = entity;
    }

    /// <summary>
    /// Removes an entity from collision.
    /// </summary>
    private void RemoveCollision()
    {
        if (!CollectiblePlayerCollisionRemoveEvent.TryReceive(out Entity entity)
            || CollisionEntity != entity)
        {
            return;
        }

        CollisionEntity = null;
    }

    private void CollectEntity()
    {
        if (!InteractEvent.TryReceive())
        {
            return;
        }

        if (CollisionEntity == null)
        {
            // Nothing to collect.
            return;
        }

        if (PlayerManager.Instance.Inventory.IsFull)
        {
            return;
        }

        // Add item to inventory.
        InventoryManager inventory = PlayerManager.Instance.Inventory;
        if (!inventory.TryAddItem(CollisionEntity.Name, out Slot itemSlot))
        {
            // Could not add item to inventory.
            return;
        }

        if (itemSlot.ItemCount == 1)
        {
            // Add new item to inventory UI.
            ImageElement imageElement = VacantGrid.FindVisualChildOfType<ImageElement>();
            imageElement.Source = new SpriteFromSheet
            {
                Sheet = Content.Load<SpriteSheet>($"Sprites/Collectibles/{itemSlot.ItemName} spritesheet")
            };

            VacantGrid.Name = itemSlot.ItemName;
            VacantGrid = GetVacantGrid();
        }
        else
        {
            // Increase existing item count in inventory UI.
            Grid grid = GetOccupiedGrid(itemSlot.ItemName);
            grid.FindVisualChildOfType<TextBlock>().Text = itemSlot.ItemCount.ToString();
        }

        CollectEntityEvent.Broadcast(CollisionEntity);
        RemoveEntity(CollisionEntity);
        CollisionEntity = null;

        AddFlower();
    }

    /// <summary>
    /// Removes an entity from the scene.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    private void RemoveEntity(Entity entity)
    {
        foreach (Entity child in entity.GetChildren())
        {
            entity.RemoveChild(child);
        }

        Entity parent = entity.GetParent();
        Entity.Scene.Entities.Remove(parent);
    }

    private void AddFlower()
    {
        float x = new Random().Next() % 2 == 0 ? 1f : -1f;
        FlowerPrefabEntity.Transform.Position.X += x;
        foreach (Entity childEntity in FlowerPrefab.Instantiate()
            .Where(entity => FlowerPrefabEntity.FindChild(entity.Name) == null))
        {
            FlowerPrefabEntity.AddChild(childEntity);
        }
        Entity.Scene.Entities.Add(FlowerPrefabEntity);
    }

    private Grid GetVacantGrid() =>
        InventoryUI
            .FindVisualChildrenOfType<Grid>()
            .Where(grid => grid.FindVisualChildrenOfType<ImageElement>() != null)
            .FirstOrDefault();

    private Grid GetOccupiedGrid(string name) =>
        InventoryUI
            .FindVisualChildrenOfType<Grid>()
            .FirstOrDefault(grid => string.Equals(grid.Name, name));
}
