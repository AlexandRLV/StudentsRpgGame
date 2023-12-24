using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

namespace PropsPools
{
    public class WorldPoolController : MonoBehaviour
	{
		[SerializeField] private Color _worldGizmosColor;
		[SerializeField] private Color _chunkGizmosColor;
		[SerializeField] private Color _currentChunkGizmosColor;
		[SerializeField] private Vector2 _worldSize;
		[SerializeField] private Vector2 _worldOffset;
		[SerializeField] private int _rowAndLinesCount = 10;
		[SerializeField] private List<WorldPoolableObject> _objects;
		[SerializeField] private List<Chunk> _chunks;
		[SerializeField] private List<DistrictPropsPlaceInfo> _pathsToProrsCointainers;

		private bool _hasTarget;
		[SerializeField] private int _currentChunkId;
		[SerializeField] private int _lastChunkID = -1;
		
		private Chunk[] _chunksToUpdate;
		private List<Chunk> _chunksToDeactivate;
		private Dictionary<int, WorldObjectsPool> _poolDictionary;
		
		private void Start()
		{
			_chunksToUpdate = new Chunk[9];
			_chunksToDeactivate = new List<Chunk>();
			
			_poolDictionary = new Dictionary<int, WorldObjectsPool>();
			for (int i = 0; i < _objects.Count; i++)
			{
				_poolDictionary.Add(i, new WorldObjectsPool(10, _objects[i]));
			}
		}

		private void Update()
		{
			if (Camera.main == null) return;
			
			Vector3 currentPosition = Camera.main.transform.position;
			
			_currentChunkId = GetCurrentChunkID(currentPosition);
			if (_lastChunkID == _currentChunkId) return;
			
			_lastChunkID = _currentChunkId;
			FillAndActivateNearestChunks();
		}
		
		private void FillAndActivateNearestChunks(int x, int y)
		{
			Profiler.BeginSample("Set chunks to deactivate");
			_chunksToDeactivate.Clear();
			foreach (var chunk in _chunksToUpdate)
			{
				if (chunk == null) continue;
				
				chunk.NeedToDeactivate = true;
				_chunksToDeactivate.Add(chunk);
			}
			Profiler.EndSample();
			
			Profiler.BeginSample("Activate chunks");
			int iterator = 0;
			for (int i = x - 1; i <= x + 1; i++)
			{
				for (int j = y - 1; j <= y + 1; j++)
				{
					if (i < 0 || i > _rowAndLinesCount) continue;
					if (j < 0 || j > _rowAndLinesCount) continue;

					int id = GetArrayIndex(i, j);
					var chunk = _chunks[id];
					chunk.NeedToDeactivate = false;
					chunk.PlaceObjects(_poolDictionary, transform);
					_chunksToUpdate[iterator++] = chunk;
				}
			}
			Profiler.EndSample();

			Profiler.BeginSample("Deactivate inactive chunks");
			DeactivateInactiveChunks();
			Profiler.EndSample();
		}

		private void FillAndActivateNearestChunks()
		{
			var xy = Get2dIndexFromArrayID(_currentChunkId);
			FillAndActivateNearestChunks(xy.x, xy.y);
		}

		private void DeactivateInactiveChunks()
		{
			if (_chunksToDeactivate.Count == 0)
				return;
			
			foreach (var chunk in _chunksToDeactivate)
			{
				if (chunk == null || !chunk.NeedToDeactivate)
					continue;
				
				chunk.RemoveObjects(_poolDictionary);
			}
			
			_chunksToDeactivate.Clear();
		}

		private Chunk GetCurrentChunk(Vector3 worldPosition) => _chunks[GetCurrentChunkID(worldPosition)];

		private int GetCurrentChunkID(Vector3 worldPosition)
		{
			var clampedPos = Clamped2DPos(worldPosition);
			var xy = GetChunkCoords(clampedPos);
			var id = GetArrayIndex(xy.x, xy.y);
			return id;
		}

		private Vector2 Clamped2DPos(Vector3 worldPosition)
		{
			worldPosition -= new Vector3(_worldOffset.x, 0, _worldOffset.y);
			var x = Mathf.Clamp(worldPosition.x, 0, _worldSize.x - 1);
			var y = Mathf.Clamp(worldPosition.z, 0, _worldSize.y - 1);
			return new Vector2(x, y);
		}

		private Vector2Int GetChunkCoords(Vector2 pos)
		{
			var x = Mathf.FloorToInt(pos.x / (_worldSize.x / _rowAndLinesCount));
			var y = Mathf.FloorToInt(pos.y / (_worldSize.y / _rowAndLinesCount));
			return new Vector2Int(x, y);
		}

		private int GetArrayIndex(int x, int y) => x + y * _rowAndLinesCount;

		private Vector2Int Get2dIndexFromArrayID(int arrayId)
		{
			int y = 0;
			while (arrayId >= _rowAndLinesCount)
			{
				y++;
				arrayId -= _rowAndLinesCount;
			}
			int x = arrayId;
			return new Vector2Int(x, y);
		}

#if UNITY_EDITOR
		[ContextMenu("CreateChunks")]
		public void CreateChunks()
		{
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			_chunks.Clear();
			for (int x = 0; x < _rowAndLinesCount; x++)
			{
				for (int y = 0; y < _rowAndLinesCount; y++)
				{
					_chunks.Add(new Chunk());
				}
			}
			
			CollectObjects();
			CollectPropsContainers();
		}

		private void CollectObjects()
		{
			UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			var poolableObjects = FindObjectsOfType<WorldPoolableObject>(false);
			HashSet<GameObject> processedObjects = new HashSet<GameObject>();
			
			var prefabsWithPaths = ListPool<PoolableObjectCachedContainer>.Get();
			foreach (var obj in poolableObjects)
			{
				if (obj == null)
					continue;

				if (processedObjects.Contains(obj.gameObject))
					continue;

				processedObjects.Add(obj.gameObject);
				if (!IsPrefabInstance(obj) || IsDisconnectedPrefabInstance(obj))
					continue;
				
				var prefabOriginal = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
				string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefabOriginal);

				var container = prefabsWithPaths.FirstOrDefault(x => x.PathToPrefab.Equals(prefabPath));
				if (container == null)
				{
					container = new PoolableObjectCachedContainer
					{
						PathToPrefab = prefabPath,
						PoolableObject = prefabOriginal
					};
					prefabsWithPaths.Add(container);
				}
				
				var holder = obj.GetComponentInParent<PoolableObjectsHolder>();
				int holderID = holder != null ? holder.HolderID : -1;
				
				var index = prefabsWithPaths.IndexOf(container);
				var chunk = GetCurrentChunk(obj.transform.position);
				chunk.AddPlaceParams(new PlaceParams(index, obj.transform.position, obj.transform.rotation, obj.transform.localScale, holderID));
			}
			Debug.Log("found " + processedObjects.Count + " poolable objects");

			foreach (var container in prefabsWithPaths)
			{
				_objects.Add(container.PoolableObject);
			}
			
			prefabsWithPaths.Clear();
			ListPool<PoolableObjectCachedContainer>.Release(prefabsWithPaths);
		}

		private void CollectPropsContainers()
		{
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			var propsContainers = FindObjectsOfType<DistrictPropsContainer>(false);

			foreach (var propsContainer in propsContainers)
			{
				if (!IsPrefabInstance(propsContainer) || IsDisconnectedPrefabInstance(propsContainer))
					continue;

				var holder = propsContainer.GetComponentInParent<PoolableObjectsHolder>();
				int holderId = holder != null ? holder.HolderID : -1;
				
				string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(propsContainer);
				_pathsToProrsCointainers.Add(new DistrictPropsPlaceInfo
				{
					PathToContainer = path,
					HolderId = holderId
				});
				
				DestroyImmediate(propsContainer.gameObject);
			}
		}

		[ContextMenu("Place Objects")]
		public void EitorPlaceObjects()
		{
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			var holders = FindObjectsOfType<PoolableObjectsHolder>();
			
			var defaultParentGO = GameObject.Find("Districts");
			foreach (var placeInfo in _pathsToProrsCointainers)
			{
				var containerPrefab = AssetDatabase.LoadAssetAtPath<DistrictPropsContainer>(placeInfo.PathToContainer);
				var holder = holders.FirstOrDefault(x => x.HolderID == placeInfo.HolderId);
				var parent = holder != null ? holder.transform : defaultParentGO.transform;
				PrefabUtility.InstantiatePrefab(containerPrefab, parent);
			}
			
			_pathsToProrsCointainers.Clear();
			_chunks.Clear();
			_objects.Clear();
		}

		private bool IsPrefabInstance(Component obj) =>
			PrefabUtility.GetCorrespondingObjectFromSource(obj) != null &&
			PrefabUtility.GetPrefabInstanceHandle(obj.transform) != null;

		private bool IsPrefabOriginal(Component obj) =>
			PrefabUtility.GetCorrespondingObjectFromSource(obj) == null &&
			PrefabUtility.GetPrefabInstanceHandle(obj.transform) != null;

		private bool IsDisconnectedPrefabInstance(Component obj) =>
			PrefabUtility.GetCorrespondingObjectFromSource(obj) != null &&
			PrefabUtility.GetPrefabInstanceHandle(obj.transform) == null;
		
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = _worldGizmosColor;
			var pos = new Vector3(_worldOffset.x + _worldSize.x / 2, 0, _worldOffset.y + _worldSize.y / 2);
			var size = new Vector3(_worldSize.x, 200, _worldSize.y);
			Gizmos.DrawWireCube(pos, size);
			
			if (_chunks == null || _chunks.Count == 0)
				return;
			
			for (int i = 0; i < _chunks.Count; i++)
			{
				var xy = Get2dIndexFromArrayID(i);
				var sizeX = _worldSize.x / _rowAndLinesCount;
				var sizeY = _worldSize.y / _rowAndLinesCount;
				var center = new Vector3(xy.x * sizeX + sizeX / 2 + _worldOffset.x, 0, xy.y * sizeY + sizeY / 2 + _worldOffset.y);
				var boxSize = new Vector3(sizeX, sizeY, sizeY);
				
				Gizmos.color = i == _currentChunkId ? _currentChunkGizmosColor : _chunkGizmosColor;
				Gizmos.DrawWireCube(center, boxSize);
			}
		}
#endif
	}
}