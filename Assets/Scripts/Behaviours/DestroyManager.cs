using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyManager : MonoBehaviour
{
	public UnityEvent onDestroy;
	public UnityEvent onDestroyEnemy;
    private ObjectPool enemyObjectPool;

    [Serializable]
	public class PrefabEntry
	{
		public GameObject prefabEnemy;
		[Range(0f, 1f)] public float spawnProbability;
	}
	public List<PrefabEntry> prefabs;


	private void OnDestroy()
	{
		onDestroy.Invoke();
		

	}

	//public void SpawnObject()
	//{
	//	Instantiate(prefab, transform.position, Quaternion.identity);
	//}
	public void SpawnObject()
	{
		// Tính tổng tỷ lệ xuất hiện
		float totalProbability = 0f;
		foreach (var entry in prefabs)
		{
			totalProbability += entry.spawnProbability;
		}

		// Chọn một giá trị ngẫu nhiên trong khoảng 0-1
		float randomValue = UnityEngine.Random.value;

		// Chọn prefab dựa trên tỷ lệ xuất hiện
		float cumulativeProbability = 0f;
		for (int i = 0; i < prefabs.Count; i++)
		{
			cumulativeProbability += prefabs[i].spawnProbability / totalProbability;
			if (randomValue <= cumulativeProbability)
			{
				Instantiate(prefabs[i].prefabEnemy, transform.position, Quaternion.identity);
				break;
			}
		}
	}
	public void autoDestroy()
	{
		Destroy(gameObject);
	}

}
