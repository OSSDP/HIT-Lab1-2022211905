using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public chesstype chesscolor = chesstype.b;

    private void Update()
    {
        
        if (chesscolor == board.Instance.turn && board.Instance.timer > 0.5f) dochess();
        
    }

    public void dochess()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
            
            print((int)(pos.x + 9.5f)+ " " + ((int)(pos.y + 9.5f)));
            
            board.Instance.dochess(new int[2] { (int)(pos.x + 9.5f), ((int)(pos.y + 9.5f)) });
            

            board.Instance.timer = 0;


        }
    }
}
