using Stride.Engine;
using Stride.Engine.Events;
using Stride.Physics;
using System.Collections.Specialized;

namespace EcomechReclamation.Collisions
{
    public class CollectibleCollisionTrigger : SyncScript
    {
        /// <summary>
        /// Broadcasts when collectible and player enter a collision.
        /// </summary>
        public static readonly EventKey<Entity> CollectiblePlayerAddCollisionEvenKey = new();

        /// <summary>
        /// Broadcasts when collectible and player exit a collision.
        /// </summary>
        public static readonly EventKey<Entity> CollectiblePlayerRemoveCollisionEvenKey = new();

        private RigidbodyComponent Rigidbody { get; set; }

        public override void Start()
        {
            Rigidbody = Entity.Get<RigidbodyComponent>();
            Rigidbody.Collisions.CollectionChanged += Collisions_CollectionChanged;
        }

        private void Collisions_CollectionChanged(object sender, Stride.Core.Collections.TrackingCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                CollectiblePlayerAddCollisionEvenKey.Broadcast(Rigidbody.Entity);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                CollectiblePlayerRemoveCollisionEvenKey.Broadcast(Rigidbody.Entity);
            }
        }

        public override void Update()
        {
        }
    }
}
