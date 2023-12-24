using System;
using System.Collections.Generic;
using UnityEngine;

namespace PropsPools
{
    [Serializable]
    public class PoolablePropsContainer
    {
        [SerializeField] public int Id;
        [SerializeField] public GameObject Prefab;
        [SerializeField] public List<GameObject> Pool;
    }

    [Serializable]
    public class PoolablePositionContainer
    {
        [SerializeField] public int PropId;
        [SerializeField] public Vector3 Position;
        [SerializeField] public Quaternion Rotation;
    }
    
    public class PropsPoolController : MonoBehaviour
    {
        [SerializeField] private List<PoolablePropsContainer> _props;
        [SerializeField] private List<PoolablePositionContainer> _positions;

        [SerializeField] private bool _baked;
        
        [ContextMenu("Bake")]
        private void Bake()
        {
            if (_baked)
            {
                Debug.LogError("Don't rebake baked props!");
                return;
            }
            
            var props = FindObjectsOfType<PoolableProps>();
            var tempProps = new Dictionary<int, PoolablePropsContainer>();
            var tempPositions = new Dictionary<int, List<PoolablePositionContainer>>();

            foreach (var prop in props)
            {
                if (tempProps.ContainsKey(prop.Id))
                {
                    var positions = tempPositions[prop.Id];
                    positions.Add(new PoolablePositionContainer
                    {
                        PropId = prop.Id,
                        Position = prop.transform.position,
                        Rotation = prop.transform.rotation,
                    });
                    
                    DestroyImmediate(prop.gameObject);
                    continue;
                }

                var prefab = new PoolablePropsContainer
                {
                    Id = prop.Id,
                    Prefab = prop.gameObject,
                    Pool = new List<GameObject>()
                };
                prop.transform.parent = transform;
                tempProps.Add(prop.Id, prefab);
                tempPositions.Add(prop.Id, new List<PoolablePositionContainer>
                {
                    new PoolablePositionContainer
                    {
                        PropId = prop.Id,
                        Position = prop.transform.position,
                        Rotation = prop.transform.rotation
                    }
                });
            }

            _props = new List<PoolablePropsContainer>();
            _positions = new List<PoolablePositionContainer>();
            
            foreach (var container in tempProps.Values)
            {
                _props.Add(container);
            }

            foreach (var positions in tempPositions.Values)
            {
                _positions.AddRange(positions);
            }
        }

        [ContextMenu("Place")]
        private void Place()
        {
            var tempProps = new Dictionary<int, PoolablePropsContainer>();
            foreach (var container in _props)
            {
                tempProps.Add(container.Id, container);
            }

            foreach (var position in _positions)
            {
                var prefab = tempProps[position.PropId];
                Instantiate(prefab.Prefab, position.Position, position.Rotation);
            }
            
            _positions.Clear();
            _props.Clear();

            while (transform.childCount > 0)
            {
                var child = transform.GetChild(0);
                DestroyImmediate(child.gameObject);
            }
        }
    }
}