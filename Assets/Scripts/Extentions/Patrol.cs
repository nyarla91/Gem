using System.Collections;
using UnityEngine;

namespace Extentions
{
    public class Patrol : Transformable
    {
        [SerializeField] private Vector3[] _path;
        [SerializeField] private float _speed;
        [SerializeField] private float _nextPointDelay;
        [SerializeField] private LoopType _loopType;
        [SerializeField] private float _cudeSize;
        
        public enum LoopType
        {
            Return,
            Lap
        }
        private Vector3 origin;

        private void Awake()
        {
            origin = Transform.position;
        }

        private void Start()
        {
            StartCoroutine(Launch());
        }

        private IEnumerator Launch()
        {
            while (true)
            {
                for (int i = 0; i < _path.Length; i++){
                    yield return StartCoroutine(GoToNextPoint(_path[i]));
                }
                if (_loopType.Equals(LoopType.Return))
                {
                    for (int i = _path.Length - 2; i >= 0; i--){
                        yield return StartCoroutine(GoToNextPoint(_path[i]));
                    }
                }
                yield return StartCoroutine(GoToNextPoint(Vector3.zero));
            }
        }

        private IEnumerator GoToNextPoint(Vector3 point)
        {
            point += origin;
            while (!Transform.position.Equals(point)){
                Transform.position = Vector3.MoveTowards(Transform.position, point, _speed * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(_nextPointDelay);
        }

        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < _path.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(_path[i] + gameObject.transform.position, new Vector3(_cudeSize, _cudeSize, _cudeSize));
                
                Vector3 lineOrigin;
                if (i == 0)
                    lineOrigin = Vector3.zero;
                else
                    lineOrigin = _path[i - 1];
                
                Gizmos.color = Color.green;
                Gizmos.DrawLine(_path[i] + gameObject.transform.position, lineOrigin + gameObject.transform.position);
            }
        }
    }

}