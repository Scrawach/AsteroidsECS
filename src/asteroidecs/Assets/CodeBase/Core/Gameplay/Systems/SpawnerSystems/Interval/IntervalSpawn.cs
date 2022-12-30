using CodeBase.Core.Gameplay.Services.Time;
using Leopotam.EcsLite;

namespace CodeBase.Core.Gameplay.Systems.SpawnerSystems.Interval
{
    public class IntervalSpawn : IEcsRunSystem
    {
        private readonly float _respawnTime;
        private readonly ISpawnStrategy _spawnStrategy;
        private readonly ITime _time;

        private float _elapsedTime;

        public IntervalSpawn(ITime time, float respawnTime, ISpawnStrategy spawnStrategy)
        {
            _time = time;
            _respawnTime = respawnTime;
            _spawnStrategy = spawnStrategy;
        }

        public void Run(IEcsSystems systems)
        {
            UpdateCooldown();

            if (IsCooldownDone())
            {
                _spawnStrategy.Spawn(systems.GetWorld());
                ResetCooldown();
            }
        }

        private void ResetCooldown() =>
            _elapsedTime -= _respawnTime;

        private void UpdateCooldown() =>
            _elapsedTime += _time.DeltaFrame;

        private bool IsCooldownDone() =>
            _respawnTime <= _elapsedTime;
    }
}