using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonPrefab<T> : MonoBehaviour where T : SingletonPrefab<T> {
	private const string assetPath ="Singletons/";
	private static T instance = null;
	
	public static T Instance
	{
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType<T>();
				// fallback
				if(instance == null)
				{
					// lets get this from resources then..
					var prefab = Resources.Load<T>(assetPath + typeof(T).Name);
					if(prefab == null)
						Debug.LogError("singleton prefab missing: + " + assetPath + typeof(T).Name);
					else
					{
						instance = Instantiate(prefab);
						instance.name = typeof(T).Name + " (Singleton)";
					}
				}
				DontDestroyOnLoad(instance.gameObject);
			}
			return instance;
		}
	}
}
