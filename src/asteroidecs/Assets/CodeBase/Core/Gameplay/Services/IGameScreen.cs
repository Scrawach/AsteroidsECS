using CodeBase.Core.Common;

namespace CodeBase.Core.Gameplay.Services
{
    public interface IGameScreen
    {
        Vector2Data Size { get; }
        bool IsOutOfBorder(Vector2Data point);
    }
}