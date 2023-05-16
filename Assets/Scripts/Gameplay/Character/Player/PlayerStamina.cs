using Extentions;
using Extentions.Pause;
using UnityEngine;
using Zenject;

namespace Gameplay.Character.Player
{
    public class PlayerStamina : MonoBehaviour
    {
        [SerializeField] private Resource _stamina;
        [SerializeField] private float _restorationTime;

        private Timer _restoration;
        
        public ResourceWrap Stamina => _stamina.Wrap;
        
        [Inject] private Pause Pause { get; set; }

        public bool TrySpendStamina() => _stamina.TrySpend(1);

        private void Restore()
        {
            _stamina.Value = _stamina.MaxValue;
            _restoration.Stop();
        }

        private void Awake()
        {
            _restoration = new Timer(this, _restorationTime, Pause);
            _restoration.Expired += Restore;
        }
    }
}