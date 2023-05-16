using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private AssetReference _loadingReference;
        [SerializeField] private AssetReference _mainMenuReference;
        [SerializeField] private AssetReference _gameplayReference;
        [SerializeField] private float _transitionDuration;
        [SerializeField] private CanvasGroup _blackScreen;

        private bool _loadingScene;
    
        public void LoadMainMenu() => LoadScene(_mainMenuReference);
        public void LoadGameplay() => LoadScene(_gameplayReference);

        private async void LoadScene(AssetReference scene)
        {
            if (_loadingScene)
                return;
            _loadingScene = true;

            _blackScreen.DOKill();
            _blackScreen.DOFade(1, _transitionDuration);
            await Task.Delay((int) (_transitionDuration * 1000));
        
            await _loadingReference.LoadSceneAsync().Task;
            await scene.LoadSceneAsync().Task;
        
            _blackScreen.DOFade(0, _transitionDuration);
            await Task.Delay((int) (_transitionDuration * 1000));
        
            _loadingScene = false;
        }
    }
}