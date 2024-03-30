using Ammo;
using Infrastructure.Services;
using Infrastructure.Services.Input;
using Infrastructure.States;
using Logic;
using MyScreen;
using UnityEngine;

namespace Infrastructure
{
    public class Game
    {
        public readonly GameStateMachine StateMachine;
        public static IInputService InputService;

        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain curtain, Camera camera,
            SpriteRenderer spriteRenderer, BulletContainer bulletParent, CameraShake cameraShake)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), curtain, AllServices.Container,
                camera, spriteRenderer, bulletParent, cameraShake);
            RegisterInputService();
        }

        private static void RegisterInputService()
        {
            if (Application.isEditor)
                InputService = new StandaloneInputService();
            else
                InputService = new MobileInputService();
        }
    }
}