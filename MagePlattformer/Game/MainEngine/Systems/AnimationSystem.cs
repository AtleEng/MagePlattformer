using System.Numerics;
using CoreEngine;
using Engine;

namespace Animation
{
    public class AnimationSystem : GameSystem
    {
        public override void Update(float delta)
        {
            foreach (GameEntity gameEntity in Core.activeGameEntities)
            {
                Animator? animator = gameEntity.GetComponent<Animator>();
                if (animator != null)
                {
                    if (animator.currentAnimation != null && animator.animations.ContainsKey(animator.currentAnimation) && animator.isPlaying)
                    {
                        animator.timer += delta;
                        if (animator.timer >= animator.animations[animator.currentAnimation].FrameDuration)
                        {
                            animator.currentFrame = (animator.currentFrame + 1) % animator.animations[animator.currentAnimation].Frames.Length;

                            animator.timer = 0;
                            //Set the correct spriteIndex
                            animator.sprite.FrameIndex = animator.animations[animator.currentAnimation].Frames[animator.currentFrame];

                            if (animator.currentFrame == 0 && !animator.animations[animator.currentAnimation].loop)
                            {
                                animator.isPlaying = false;
                            }
                        }
                    }
                }
            }
        }
    }
}