using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class Mino:MonoBehaviour
{
    public float previousTime;
    // minoの落ちる時間
    public float fallTime = 1f;

    // ステージの大きさ
    private const int width = 10;
    private const int height = 20;


    // mino回転
    public Vector3 rotationPoint;

    // グリッドの追加
    private static Transform[,] grid = new Transform[width, height];

    void Update()
    {
        MinoMovement();
    }
    
    private void MinoMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) 
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!VaildMovement()) 
            {
                transform.position -= new Vector3(-1, 0, 0);
            }
        }
        if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            transform.position += new Vector3(1, 0, 0);

            if (!VaildMovement())
            {
                transform.position -= new Vector3(1, 0, 0);
            }

        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Time.time-previousTime >= fallTime) 
        {
            transform.position += new Vector3(0, -1, 0);
            if (!VaildMovement())
            {
                transform.position -= new Vector3(0, -1, 0);

                AddToGrid();
                CheckLines();

                this.enabled = false;
                FindObjectOfType<SpawnMino>().NewMino();
            }

            previousTime = Time.time;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint),new Vector3(0,0,1),90);
        }
    }

    public void CheckLines() 
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i)) 
            {
                DeleteLine(i);
                RowDown(i);
                FindObjectOfType<GameManagement>().AddScore();
                i++;
            }
        }
    }

    bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null) 
            {
                return false;
            }
        }
        

        return true;
    }

    void DeleteLine(int i) 
    {
        for(int j =0; j < width; j++) 
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    public void RowDown(int i) 
    {
        for(int y = i; y< height; y++) 
        {
            for(int j =0; j<width; j++) 
            {
                if (grid[j,y]!= null) 
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    void AddToGrid() 
    {
        foreach(Transform children in transform) 
        {
            int roundX = Mathf.RoundToInt(children.position.x);
            int roundY = Mathf.RoundToInt(children.position.y);

            grid[roundX,roundY] = children;

            if(roundY >= height - 1) 
            {
                FindObjectOfType<GameManagement>().GameOver();
            }
        }
    }

    bool VaildMovement() 
    {
        foreach (Transform children in transform) 
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            if(roundX < 0 || roundX >= width || roundY < 0 || roundY >= height) 
            {
                return false;
            }
            if (grid[roundX,roundY]!= null) 
            {
                return false;
            }
        }
        return true;
    }
}

