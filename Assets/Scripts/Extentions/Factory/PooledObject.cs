using System;

namespace Extentions.Factory
{
    public class PooledObject : Transformable
    {
        private PoolFactory _factory;

        public event Action<PooledObject> PoolDisabled;
        public event Action<PooledObject> PoolEnabled;

        public virtual void PoolInit(PoolFactory factory)
        {
            _factory = factory;
        }

        public virtual void OnPoolEnable()
        {
            PoolEnabled?.Invoke(this);
        }

        public virtual void PoolDisable()
        {
            PoolDisabled?.Invoke(this);
            if (_factory != null)
                _factory.DisableObject(this);
            else
                Destroy(gameObject);
        }
    }
}