using CodeBase.Core.Gameplay.Services.Time;
using CodeBase.Core.Infrastructure.Systems.Abstract;
using Leopotam.EcsLite;
using UnityEditor;

namespace CodeBase.Engine.Systems
{
    public class UpdateTimeSystems : IConnectableSystem
    {
        private readonly WatchTime _watchTime;

        public UpdateTimeSystems(WatchTime watchTime) =>
            _watchTime = watchTime;

        public IEcsSystems ConnectTo(IEcsSystems systems) =>
            systems.Add(new UpdateTimeImplementation(_watchTime));

        private class UpdateTimeImplementation : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
        {
            private readonly WatchTime _watchTime;

            public UpdateTimeImplementation(WatchTime watchTime)
            {
                _watchTime = watchTime;

#if UNITY_EDITOR
                EditorApplication.pauseStateChanged += state =>
                {
                    switch (state)
                    {
                        case PauseState.Paused:
                            _watchTime.Stop();
                            break;
                        case PauseState.Unpaused:
                            _watchTime.Start();
                            break;
                    }
                };
#endif
            }

            public void Destroy(IEcsSystems systems) =>
                _watchTime.Reset();

            public void Init(IEcsSystems systems) =>
                _watchTime.Start();

            public void Run(IEcsSystems systems) =>
                _watchTime.Update();
        }
    }
}