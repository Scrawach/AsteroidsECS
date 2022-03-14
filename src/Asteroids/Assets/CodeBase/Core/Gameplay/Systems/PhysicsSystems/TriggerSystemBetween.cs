using CodeBase.Core.Gameplay.Components.Events;
using CodeBase.Core.Gameplay.Components.Tags;
using CodeBase.Core.Gameplay.Systems.PhysicsSystems.Strategies;
using Leopotam.EcsLite;

namespace CodeBase.Core.Gameplay.Systems.PhysicsSystems
{
    public class TriggerSystemBetween<TSender, TTarget> : IEcsRunSystem
        where TSender : struct
        where TTarget : struct
    {
        private readonly IEnterTriggerStrategy _strategy;

        public TriggerSystemBetween(IEnterTriggerStrategy strategy) =>
            _strategy = strategy;

        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<OnTriggerEnterEvent>().End();

            var events = world.GetPool<OnTriggerEnterEvent>();
            var calculated = world.GetPool<PhysicsAlreadyCalculatedTag>();

            foreach (var index in filter)
            {
                ref var enter = ref events.Get(index);

                if (UnpackEvent(enter, world, out var sender, out var trigger))
                    if (IsCollideBetween<TSender, TTarget>(world, sender, trigger) ||
                        IsCollideBetween<TTarget, TSender>(world, sender, trigger))
                    {
                        if (calculated.Has(sender) || calculated.Has(trigger))
                            continue;

                        _strategy.OnEnter(world, sender, trigger);
                        calculated.Add(sender);
                        calculated.Add(trigger);
                    }
            }
        }

        private static bool UnpackEvent(OnTriggerEnterEvent enterEvent, EcsWorld world, out int sender, out int trigger)
        {
            var isUnpackSender = enterEvent.Sender.Unpack(world, out sender);
            var isUnpackTrigger = enterEvent.Trigger.Unpack(world, out trigger);
            return isUnpackSender && isUnpackTrigger;
        }

        private bool IsCollideBetween<TEnterSender, TEnterTrigger>(EcsWorld world, int sender, int trigger)
            where TEnterSender : struct where TEnterTrigger : struct
        {
            var senders = world.GetPool<TEnterSender>();
            var triggers = world.GetPool<TEnterTrigger>();

            return senders.Has(sender) &&
                   triggers.Has(trigger);
        }
    }
}