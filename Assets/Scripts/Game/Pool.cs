using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool : MonoBehaviour
{
	public GameObject prefab;
	public int initialAmount = 0;
	private List<GameObject> list = new List<GameObject>();

	void Start()
	{
		for (int i = 0; i < initialAmount; i++)
		{
			GameObject instance = Instantiate(prefab,transform);
			instance.transform.position = new Vector3(0, -1, 0);
			instance.SetActive(false);
			list.Add(instance);
		}
	}

	public void ClearAll()
	{
		foreach (GameObject obj in list)
		{
			obj.transform.position = new Vector3(0, -1, 0);
			obj.SetActive(false);
		}
	}

	public T GetInstance<T>(bool activate)
	{
		return GetInstance(activate).GetComponent<T>();
	}

	public System.Collections.Generic.IEnumerable<T> Next<T>()
	{ 
		foreach(GameObject instance in list)
			if (instance.activeSelf)
				yield return instance.GetComponent<T>();
	}

	public GameObject GetInstance(bool activate)
	{
		foreach (GameObject obj in list)
			if (!obj.activeSelf)
			{
				obj.SetActive(activate);
				return obj;
			}

		GameObject instance = Instantiate(prefab, transform);
		instance.SetActive(activate);
		list.Add(instance);
		return instance;
	}

	public bool AnyActive()
	{
		foreach (GameObject obj in list)
			if (obj.activeSelf)
				return true;
		return false;
	}

	public int Count()
	{
		int c = 0;
		foreach (GameObject obj in list)
			if (obj.activeSelf)
				c++;
		return c;
	}
}