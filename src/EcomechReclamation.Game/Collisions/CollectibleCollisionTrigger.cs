using EcomechReclamation.Player;
using Stride.Engine;
using Stride.Physics;
using System.Linq;

namespace EcomechReclamation.Collisions
{
    public class CollectibleCollisionTrigger : SyncScript
    {
        private RigidbodyComponent Rigidbody { get; set; }

        public override void Start()
        {
            Rigidbody = Entity.Get<RigidbodyComponent>();
            Rigidbody.Collisions.CollectionChanged += Collisions_CollectionChanged;
            //Collider = Entity.Get<StaticColliderComponent>();
            //Collider.Collisions.CollectionChanged += Collisions_CollectionChanged;
        }

        private void Collisions_CollectionChanged(object sender, Stride.Core.Collections.TrackingCollectionChangedEventArgs e)
        {
            Collision collision = (Collision)e.Item;
            PhysicsComponent otherCollider = Rigidbody == collision.ColliderA ? collision.ColliderB : collision.ColliderA;

            EntityComponent playerController = otherCollider.Entity.Components.FirstOrDefault(component => component.GetType() == typeof(PlayerController));
            if (playerController != null)
            {
                (playerController as PlayerController).CollectibleEntityCollision(Rigidbody.Entity, e.Action);
            }
        }

        public override void Update()
        {
        }
    }
}
