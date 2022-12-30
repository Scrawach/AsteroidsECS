using CodeBase.Core.Gameplay.Components.Meta;
using CodeBase.Core.Gameplay.Services;
using Leopotam.EcsLite;

namespace CodeBase.Core.Gameplay.Systems.MetaSystems
{
    public class GameOverSystem : IEcsRunSystem, IEcsDestroySystem
    {
        private readonly IUiFactory _uiFactory;

        public GameOverSystem(IUiFactory uiFactory) =>
            _uiFactory = uiFactory;

        public void Destroy(IEcsSystems systems) =>
            _uiFactory.CloseGameOverWindow();

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<GameOverEvent>().End();

            if (filter.GetEntitiesCount() > 0)
                _uiFactory.OpenGameOverWindow(world);

            foreach (var index in filter)
                world.DelEntity(index);
        }
    }
}