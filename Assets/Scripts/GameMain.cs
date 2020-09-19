using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Click1()
	{
		Debug.Log("HOLA");
		Board board = new Board();
		board.GetAllMovements(true);
	}
}
