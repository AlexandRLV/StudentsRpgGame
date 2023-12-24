using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

namespace PropsPools
{
    [Serializable]
	internal class Chunk
	{
		internal bool NeedToDeactivate = false;
		
		[SerializeField] private List<PlaceParams> _chunkPlaceParams;
		
		private bool _isChunkActive = false;
		private bool _isInited = false;
		private List<WorldPoolableObject> _pulledObjects;

		internal void AddPlaceParams(PlaceParams placeParams)
		{
			_chunkPlaceParams ??= new List<PlaceParams>();
			_chunkPlaceParams.Add(placeParams);
		}
		
		private void TryInit()
		{
			if (_isInited) return;
			
			_pulledObjects = new List<WorldPoolableObject>(_chunkPlaceParams.Count);
			_isInited = true;
		}

		public void PlaceObjects(Dictionary<int, WorldObjectsPool> poolDictionary, Transform parent)
		{
			TryInit();
			
			if (_isChunkActive) return;
			if (_chunkPlaceParams == null) return;
			
			Profiler.BeginSample("Chunk place objects");
			var count = _chunkPlaceParams.Count;
			for (int i = 0; i < count; i++)
			{
				var placeParams = _chunkPlaceParams[i];
				var pool = poolDictionary[placeParams.ObjectID];
				var obj = pool.Get();

#if UNITY_EDITOR
				obj.Place(placeParams.Position, placeParams.Rotation,placeParams.Scale, parent);
#else
				obj.Place(placeParams.Position, placeParams.Rotation, placeParams.Scale);
#endif
				
				_pulledObjects.Add(obj);
			}
			Profiler.EndSample();
			
			_isChunkActive = true;
		}

		public void RemoveObjects(Dictionary<int, WorldObjectsPool> poolDictionary)
		{
			if (_isInited == false) return;
			if (_isChunkActive == false) return;
			
			Profiler.BeginSample("Chunk remove objects");
			var count = _pulledObjects.Count;
			for (int i = 0; i < count; i++)
			{
				var placeParams = _chunkPlaceParams[i];
				var obj = _pulledObjects[i];
				var pool = poolDictionary[placeParams.ObjectID];
				pool.Release(obj);
			}
			
			_pulledObjects.Clear();
			_isChunkActive = false;
			NeedToDeactivate = false;
			
			Profiler.EndSample();
		}

#if UNITY_EDITOR
		public void PlaceObjectsToEditor(List<WorldPoolableObject> objects, Transform parrent)
		{
			if (_chunkPlaceParams == null) return;
			
			var holders = GameObject.FindObjectsOfType<PoolableObjectsHolder>();
			var count = _chunkPlaceParams.Count;
			
			for (int i = 0; i < count; i++)
			{
				var placeParams = _chunkPlaceParams[i];
				var obj = objects[placeParams.ObjectID];
				
				var newObj = UnityEditor.PrefabUtility.InstantiatePrefab(obj, parrent) as WorldPoolableObject;
				newObj.gameObject.name = newObj.gameObject.name.Replace("(Clone)", "");
				newObj.Place(placeParams.Position, placeParams.Rotation, placeParams.Scale);
				
				var holder = holders.FirstOrDefault(x => x.HolderID == placeParams.HolderId);
				if (holder)
					newObj.transform.parent = holder.transform;
			}
		}
#endif
	}
}