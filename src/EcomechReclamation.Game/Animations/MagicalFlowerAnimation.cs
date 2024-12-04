using Stride.Animations;
using Stride.Engine;
using System;

namespace EcomechReclamation.Animations
{
    public class MagicalFlowerAnimation : SyncScript
    {
        private readonly string BloomAnimationName = "Bloom";

        private AnimationComponent AnimationComponent { get; set; }
        private PlayingAnimation PlayingAnimation { get; set; }
        private bool Played = false;

        private DateTime StartTime { get; set; } = DateTime.Now;

        public override void Start()
        {
            Played = false;
            StartTime = DateTime.UtcNow;
            AnimationComponent = Entity.Get<AnimationComponent>();

        }

        public override void Update()
        {
            if (!Played && !AnimationComponent.IsPlaying(BloomAnimationName) && DateTime.UtcNow >= StartTime.AddSeconds(5))
            {
                PlayingAnimation = AnimationComponent.Play(BloomAnimationName);
                Played = true;
            }
        }
    }
}
