using UnityEngine;

namespace PropsPools
{
    [DisallowMultipleComponent]
	internal class PoolableRenderedObject : WorldPoolableObject
	{
		internal bool IsActive
		{
			get => _active;
			set
			{
				var length = _renderers.Length;
				for (int i = 0; i < length; i++)
				{
					_renderers[i].enabled = value;
				}

				length = _colliders.Length;
				for (int i = 0; i < length; i++)
				{
					_colliders[i].enabled = value;
				}

				_active = value;
			}
		}

		internal override GameObject GetGameObject => CachedGameObject;

		[SerializeField] internal Transform CachedTransform;
		[SerializeField] internal GameObject CachedGameObject;
		[SerializeField] private Renderer[] _renderers;
		[SerializeField] private Collider[] _colliders;

		[HideInInspector] [SerializeField] private bool _active;

		[ContextMenu("Setup")]
		internal void Setup()
		{
			Setup(transform);
		}

		internal void Setup(Transform root, bool active = true)
		{
			CachedTransform = root;
			CachedGameObject = root.gameObject;
			_renderers = root.GetComponentsInChildren<Renderer>();
			_colliders = root.GetComponentsInChildren<Collider>();
			IsActive = active;
		}

		internal override void OnTakeFromPool()
		{
			IsActive = true;
		}

		internal override void OnReturnedToPool()
		{
			IsActive = false;
		}

		internal override void OnDestroyCall()
		{
#if UNITY_EDITOR
			if (Application.isPlaying == false)
			{
				DestroyImmediate(CachedTransform);
				return;
			}
#endif
			Destroy(CachedTransform);
		}

		internal override void Place(Vector3 pos, Quaternion rot, Vector3 scale, Transform parent)
		{
			CachedTransform.localScale = scale;
			CachedTransform.position = pos;
			CachedTransform.rotation = rot;
			CachedTransform.SetParent(parent, true);
		}

		internal override void Place(Vector3 pos, Quaternion rot, Vector3 scale)
		{
			CachedTransform.localScale = scale;
			CachedTransform.position = pos;
			CachedTransform.rotation = rot;
		}

		internal override void Place(Vector3 pos, Quaternion rot, Transform parent)
		{
			CachedTransform.position = pos;
			CachedTransform.rotation = rot;
			CachedTransform.SetParent(parent, true);
		}

		internal override void Place(Vector3 pos, Quaternion rot)
		{
			CachedTransform.position = pos;
			CachedTransform.rotation = rot;
		}

		internal override void Place(Vector3 pos)
		{
			CachedTransform.position = pos;
		}
	}
}