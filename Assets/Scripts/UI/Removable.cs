using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class Removable : MonoBehaviour
    {
        public void Remove()
        {
            List<IRemovableHandler> handlers = new List<IRemovableHandler>();
            handlers.AddRange(transform.GetComponents<IRemovableHandler>().ToList());
            handlers.AddRange(transform.GetComponentsInChildren<IRemovableHandler>().ToList());
            
            foreach (IRemovableHandler handler in handlers)
            {
                handler?.OnRemoved();
            }
            Destroy(gameObject);
        }
    }
}