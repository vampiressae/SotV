using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    public class ActorHolderParent : MonoBehaviour
    {
        [SerializeField] private List<Transform> _land;

        public Transform GetParent(int index)
        {
            if (index < 0 || index >= _land.Count || _land[index] == null)
                return transform;
            return _land[index];
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_land.IsNullOrEmpty()) AutoGet();
        }

        [Button]
        private void AutoGet()
        {
            _land.Clear();
            foreach(Transform t in transform) 
                if (t != transform)
                    _land.Add(t);
        }
#endif
    }
}