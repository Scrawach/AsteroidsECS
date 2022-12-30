using System.Threading.Tasks;
using CodeBase.Core.Gameplay.Services;
using CodeBase.Core.Gameplay.Services.Meta;
using CodeBase.Engine.Services.AssetManagement;
using CodeBase.Engine.UI;
using Leopotam.EcsLite;
using UnityEngine;

namespace CodeBase.Engine.Services.Factory
{
    public class UiFactory : IUiFactory
    {
        private readonly IAssets _assets;
        private readonly IWallet _wallet;
        
        private GameOverWindow _gameOverWindow;
        private GameplayHud _gameplayHud;

        public UiFactory(IAssets assets, IWallet wallet)
        {
            _assets = assets;
            _wallet = wallet;
        }

        public async Task OpenGameplayHud()
        {
            _gameplayHud = await OpenAsync<GameplayHud>(nameof(GameplayHud));
            _gameplayHud.Construct(_wallet);
        }

        public async Task OpenGameOverWindow(EcsWorld world)
        {
            _gameOverWindow = await OpenAsync<GameOverWindow>(nameof(GameOverWindow));
            _gameOverWindow.Construct(world);
        }

        public void CloseGameplayHud()
        {
            Object.Destroy(_gameplayHud.gameObject);
            _gameplayHud = null;
        }

        public void CloseGameOverWindow()
        {
            Object.Destroy(_gameOverWindow.gameObject);
            _gameOverWindow = null;
        }

        private async Task<TWindow> OpenAsync<TWindow>(string address) where TWindow : MonoBehaviour
        {
            var prefab = await _assets.Load<GameObject>(address);
            return Object.Instantiate(prefab).GetComponent<TWindow>();
        }
    }
}