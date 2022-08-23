using CodeBase.Core.Gameplay.Components.Moves;
using CodeBase.Core.Gameplay.Components.Tags;
using CodeBase.Core.Gameplay.Services;
using Leopotam.EcsLite;

namespace CodeBase.Core.Gameplay.Systems.LifecycleSystems
{
    public class KillOutOfBorderObjects : IEcsRunSystem
    {
        private readonly IGameScreen _screen;

        public KillOutOfBorderObjects(IGameScreen screen) =>
            _screen = screen;

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<Position>().Exc<DestroyTag>().End();

            var positions = world.GetPool<Position>();
            var dead = world.GetPool<DestroyTag>();

            foreach (var index in filter)
            {
                ref var position = ref positions.Get(index);

                if (_screen.IsOutOfBorder(position.Value))
                    dead.Add(index);
            }
        }
    }
}