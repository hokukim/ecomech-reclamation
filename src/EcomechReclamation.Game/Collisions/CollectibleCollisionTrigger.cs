using EcomechReclamation.Player;
using Stride.Engine;
using Stride.Physics;
using System.Linq;

namespace EcomechReclamation.Collisions
{
    public class CollectibleCollisionTrigger : SyncScript
    {
        private StaticColliderComponent Collider { get; set; }

        public override void Start()
        {
            Collider = Entity.Get<StaticColliderComponent>();
            Collider.Collisions.CollectionChanged += Collisions_CollectionChanged;
        }

        private void Collisions_CollectionChanged(object sender, Stride.Core.Collections.TrackingCollectionChangedEventArgs e)
        {
            Collision collision = (Collision)e.Item;
            PhysicsComponent otherCollider = Collider == collision.ColliderA ? collision.ColliderB : collision.ColliderA;

            EntityComponent playerController = otherCollider.Entity.Components.FirstOrDefault(component => component.GetType() == typeof(PlayerController));
            if (playerController != null)
            {
                (playerController as PlayerController).CollectibleEntityCollision(Collider.Entity, e.Action);
            }
        }

        public override void Update()
        {
        }
    }
}
