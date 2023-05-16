using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extentions
{
    public class OverlapTrigger2D : Transformable
    {
        [SerializeField] private List<Collider2D> _colliders = new List<Collider2D>();

        public Collider2D[] Content => _colliders.ToArray();
        
        public T[] GetContent<T>() where T : Component
            => GetContent<T>(int.MaxValue);
        
        public T[] GetContent<T>(LayerMask layerMask) where T : Component =>
            _colliders
                .Where(c => layerMask == (layerMask | (1 << c.gameObject.layer)) && c.gameObject.activeInHierarchy)
                .Where(c => c.GetComponent<T>() != null)
                .Select(c => c.GetComponent<T>()).ToArray();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if ( ! _colliders.Contains(other))
                _colliders.Add(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_colliders.Contains(other))
                _colliders.Remove(other);
        }
    }
}