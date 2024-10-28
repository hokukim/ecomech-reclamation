using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Physics;

namespace EcomechReclamation
{
    public class CollisionTrigger : SyncScript
    {
        private StaticColliderComponent Collider { get; set; }

        public override void Start()
        { 
            Collider = Entity.Get<StaticColliderComponent>();
        }

        public override void Update()
        {
            foreach (Collision collision in Collider.Collisions)
            {
                DebugText.Print($"A: {collision.ColliderA.Entity.Name}, B: {collision.ColliderB.Entity.Name}", new Int2(500, 300));
            }
        }
    }
}
