using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    public BoardView boardView;

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
        boardView.GetComponent<BoardView>().Initialize(BoardSerialization.FromFile("Text/Boards/test4"));

    }
}
