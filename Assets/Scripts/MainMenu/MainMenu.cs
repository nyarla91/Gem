using Core;
using UnityEngine;
using Zenject;

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [Inject] private SceneLoader SceneLoader {get; set; }
        
        public void Quit() => Application.Quit();
        public void StartGame() => SceneLoader.LoadGameplay();
    }
}