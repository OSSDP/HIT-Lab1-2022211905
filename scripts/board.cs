using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
//这是一次修改
public class board : MonoBehaviour
{

    static board _instance;

    public chesstype turn = chesstype.b;
    public int[,] grid;
    public GameObject[] prefab;
    public float timer = 0;
    public int record = 0;

    public static board Instance { get => _instance; }
    private void Awake()
    {
        if(Instance == null)
        {
            _instance = this;
        }
    }

    public void Start()
    {
        grid = new int[19, 19];
        
    }
    private void Update()
    {
        timer += Time.deltaTime; 
    }
    public bool dochess(int[] pos)
    {
        record++;


        
        pos[0] = Mathf.Clamp(pos[0], 0, 18);
        pos[1] = Mathf.Clamp(pos[1], 0, 18);
        
        if (grid[pos[0], pos[1]] != 0) return false;
        
        if(turn == chesstype.b)
        {
            
            Instantiate(prefab[0],new Vector3(pos[0] - 9, pos[1] - 9),Quaternion.identity);
            
            grid[pos[0], pos[1]] = 1;
            
          
            checkline(pos, new int[2]{1,0});
            checkline(pos, new int[2]{0,1});
            if (!go_on(turn) && record >2)
            {
                Debug.Log(turn + "win!!!");
                record = 0;
            }
            
            turn = chesstype.w;

        }
        else if(turn == chesstype.w)
        {
            Instantiate(prefab[1], new Vector3(pos[0] - 9, pos[1] - 9), Quaternion.identity);
            grid[pos[0], pos[1]] = 2;
            
            checkline(pos, new int[2] { 1, 0 });
            checkline(pos, new int[2] { 0, 1 });
            if (!go_on(turn) && record > 2)
            {
                Debug.Log(turn + "win!!!");
                record = 0; 
            }
            turn = chesstype.b;
        }
        return true;
    }

    public bool go_on(chesstype type)
    {
        int m;
        for (int i = 0; i < 19; i++)
        {
            for (int j = 0; j < 19; j++)
            {
                if (grid[i, j] == (m = ((int)turn == 1) ? 2 : 1)) 
                { return true; }

                

                
            }
        }
        return false;
    }
    
    public bool win( int[] pos)
    {
        return false;
    }
    
    public bool checkline(int[] pos, int[] offset)
    {
        int[,] visit = new int[19, 19];
       
        int index = 1;
  
        int m =  ((int)turn == 1) ? 2 : 1 ;
        visit[pos[0], pos[1]] = 1;
        if (grid[pos[0] + 1, pos[1]] == m) visit[pos[0] + 1, pos[1]] = 1;
        if (grid[pos[0] , pos[1]+1 ] == m) visit[pos[0] , pos[1] + 1] = 1;
        if (grid[pos[0] - 1, pos[1]] == m) visit[pos[0] - 1, pos[1]] = 1;
        if (grid[pos[0] , pos[1] - 1] == m) visit[pos[0], pos[1] - 1] = 1;
        //right
        for (int i = offset[0], j = offset[1]; (pos[0] + i >= 0 && pos[0] + i < 19) && (pos[1] + j >= 0 && pos[1] + j < 19); i += offset[0] ,j += offset[1])
        {
            if (grid[pos[0] + i, pos[1] + j] == (int)turn)
            {
                index++;
                if (grid[pos[0] +i + 1, pos[1]+j] == m) visit[pos[0] + i + 1, pos[1] + j] = 1;
                if (grid[pos[0] + i, pos[1] + j + 1] == m) visit[pos[0] + i, pos[1] + 1 + j] = 1;
                if (grid[pos[0] + i - 1, pos[1] + j] == m) visit[pos[0] + i - 1, pos[1] + j] = 1;
                if (grid[pos[0] + i, pos[1] - 1 + j] == m) visit[pos[0] + i, pos[1] - 1 + j] = 1;
                visit[pos[0] + i, pos[1] + j] = 1;

            }
            else if (grid[pos[0] + i, pos[1] + j] == m )
            {
                visit[pos[0] + i, pos[1] + j] = 1;
                break;
            }
            else
            {
                break;
            }
            
        }
        //left
        for (int i = -offset[0], j = -offset[1]; (pos[0] + i >= 0 && pos[0] + i < 19) && (pos[1] + j >= 0 && pos[1] + j < 19); i -= offset[0], j -= offset[1])
        {
            if (grid[pos[0] + i, pos[1] + j] == (int)turn)
            {
                index++;
                if (grid[pos[0] + i + 1, pos[1] + j] == m) visit[pos[0] + i + 1, pos[1] + j] = 1;
                if (grid[pos[0] + i, pos[1] + j + 1] == m) visit[pos[0] + i, pos[1] + 1 + j] = 1;
                if (grid[pos[0] + i - 1, pos[1] + j] == m) visit[pos[0] + i - 1, pos[1] + j] = 1;
                if (grid[pos[0] + i, pos[1] - 1 + j] == m) visit[pos[0] + i, pos[1] - 1 + j] = 1;
                visit[pos[0] + i, pos[1] + j] = 1;
            }
            else if (grid[pos[0] + i, pos[1] + j] == m )
            {
                visit[pos[0] + i, pos[1] + j] = 1;
                break;
            }
            else
            {
                break;
            }
        }
        
        if (index >= 3)//消除
        {
            
            for (int i = 0; i < 19; i++) 
            {
                for (int j = 0; j < 19; j++) 
                {
                    if (visit[i, j] == 1)
                    {
                        
                        Vector3 positionToDestroy = new Vector3(i - 9, j - 9); 
                        GameObject[] instances = GameObject.FindObjectsOfType<GameObject>();

                        foreach (GameObject obj in instances)
                        {
                            if (obj.transform.position == positionToDestroy)
                            {
                                Destroy(obj);
                                
                                break; 
                            }
                        }

                        grid[i, j] = 0;

                    }
                }
                
            }
        }

        return false;
    }
}
public enum chesstype
{
    o,//meaningless
    b,
    w
}
