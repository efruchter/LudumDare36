using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class Pool : MonoBehaviour
{
	Dictionary<Type, Stack<Poolable>> pools = new Dictionary<Type, Stack<Poolable>>();

	static Pool instance;
	static Pool I
	{
		get
		{
			if (instance == null)
			{
				instance = new GameObject("Pool").AddComponent<Pool>();
			}
			return instance;
		}
	}

	void OnDestroy()
	{
		instance = null;
	}

	public static void PoolSelf<T>(T poolable) where T : MonoBehaviour, Poolable
	{
		Stack<Poolable> pool;
		if (!I.pools.TryGetValue(typeof(T), out pool))
		{
			I.pools[typeof(T)] = pool = new Stack<Poolable>(1);
		}

		poolable.OnPool();
		poolable.gameObject.SetActive(false);
		poolable.transform.SetParent(I.transform);
		pool.Push (poolable);
	}

	public static T Get<T>() where T : MonoBehaviour, Poolable
	{
		Stack<Poolable> pool;
		if (I.pools.TryGetValue(typeof(T), out pool) && (pool.Count > 0))
		{
			T t = pool.Pop() as T;
			t.transform.SetParent(null);
			t.gameObject.SetActive(true);
			t.OnSpawn();
			return t;
		}
		else
		{
			return null;
		}
	}

	public static T Get<T>(T prefab) where T : MonoBehaviour, Poolable
	{
		T t = Get<T>();
		if (t == null)
		{
			t = Instantiate<T>(prefab);
			t.OnSpawn();
		}
		return t;
	}
}

public interface Poolable
{
	void OnPool();
	void OnSpawn();
}
