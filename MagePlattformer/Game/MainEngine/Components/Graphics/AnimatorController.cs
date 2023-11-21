using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using CoreEngine;
using Engine;

namespace CoreAnimation
{
    public class Animator : Component
    {
        public Sprite sprite;
        public Dictionary<string, Animation> animations;
        public string currentAnimation = "";
        public int currentFrame;
        public float timer;

        public bool isPlaying;
        public Animator(Sprite sprite)
        {
            this.sprite = sprite;

            animations = new Dictionary<string, Animation>();
        }
        public void AddAnimation(string name, Animation animation)
        {
            animations[name] = animation;
        }

        public void PlayAnimation(string name)
        {
            if (animations.ContainsKey(name))
            {
                isPlaying = true;
                sprite.FrameIndex = animations[name].Frames[0];
                currentAnimation = name;
                currentFrame = 0;
                timer = 0;
                System.Console.WriteLine($"Playing animation: {name} for {gameEntity.name}");
            }
            else { System.Console.WriteLine($"Animation {name} doesn't exist!!!"); }
        }
        public override string PrintStats()
        {
            string animationsText = "";
            foreach (string t in animations.Keys)
            {
                animationsText += $"{t} ";
            }
            return $"Animations < {animationsText}> CurrentAnimation: {currentAnimation} IsPlaying{isPlaying}";
        }
    }
    public class Animation
    {
        public int[] Frames { get; set; }
        public float FrameDuration { get; set; }
        public bool loop;

        public Animation(int[] frames, float frameDuration, bool loop)
        {
            Frames = frames;
            FrameDuration = frameDuration;
            this.loop = loop;
        }
    }
}