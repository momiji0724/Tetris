using UnityEngine;

public class Mino : MonoBehaviour
{
    private float horizontalTimer;
    public float horizontalInterval = 0.1f;
    private float downTimer;
    public float downInterval = 0.05f;

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
        horizontalTimer += Time.deltaTime;

        if (horizontalTimer >= horizontalInterval)
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                transform.position += new Vector3(-1, 0, 0);

                if (!ValidMovement())
                {
                    transform.position -= new Vector3(-1, 0, 0);
                }


                horizontalTimer = 0;

            }

            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(1, 0, 0);

                if (!ValidMovement())
                {
                    transform.position -= new Vector3(1, 0, 0);
                }

                horizontalTimer = 0;

            }
        }
        if (Time.time - previousTime >= fallTime)
        {
            MoveDown();
            previousTime = Time.time;

        }

        downTimer += Time.deltaTime;
        if (downTimer >= downInterval)
        {
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                MoveDown();
            }

            downTimer = 0;
        }



        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            while (true)
            {
                transform.position += new Vector3(0, -1, 0);

                if (!ValidMovement())
                {
                    transform.position -= new Vector3(0, -1, 0);
                    break;
                }

            }
            AddToGrid();
            CheckLines();

            this.enabled = false;
            FindObjectOfType<SpawnMino>().NewMino();



        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 oldPosition = transform.position;
            Quaternion oldRotation = transform.rotation;

            transform.RotateAround
                (transform.TransformPoint(rotationPoint),
                new Vector3(0, 0, 1),
                90);

            transform.position = new Vector3
                (
                    Mathf.Round(transform.position.x),
                    Mathf.Round(transform.position.y),
                    0
                );


            if (ValidMovement())
            {
                return;
            }

            transform.position += Vector3.right;

            if (ValidMovement())
            {
                return;
            }

            transform.position += Vector3.left * 2;
            if (ValidMovement())
            {
                return;
            }

            transform.position = oldPosition;
            transform.rotation = oldRotation;

        }



    }

    public void MoveDown()
    {
        transform.position += new Vector3(0, -1, 0);
        if (!ValidMovement())
        {
            transform.position -= new Vector3(0, -1, 0);

            AddToGrid();
            CheckLines();

            this.enabled = false;
            FindObjectOfType<SpawnMino>().NewMino();
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
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    public void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null)
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
        foreach (Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.position.x);
            int roundY = Mathf.RoundToInt(children.position.y);

            if(roundX >= 0 && roundX < width &&
               roundY >= 0 && roundX < height) 
            {
                grid[roundX, roundY] = children;
            }
            

            
        }
    }

    //void UpdateGrid() 
    //{
    //    RemoveFromGrid();

    //    foreach(Transform child in transform) 
    //    {
    //        int x = Mathf.RoundToInt(child.position.x);
    //        int y = Mathf.RoundToInt(child.position.y);

    //        if (x >= 0 && x < width && y >= 0 && y < height)
    //        {
    //                grid[x, y] = child;
    //        }
    //    }
    //}

    //void RemoveFromGrid() 
    //{
    //    foreach (Transform child in transform) 
    //    {
    //        int x = Mathf.RoundToInt(child.position.x);
    //        int y = Mathf.RoundToInt(child.position.y);

    //        if(x >= 0 && x < width && y>= 0 && y < height)
    //        {
    //            if (grid[x,y]== child) 
    //            {
    //                grid[x,y] = null;
    //            }
    //        }
    //    }
    //}

    public bool ValidMovement()
    {
        foreach (Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            if (roundX < 0 || roundX >= width || roundY < 0)
            {
                return false;
            }
            if (roundY < height)
            {
                if (grid[roundX, roundY] != null)
                {
                    return false;
                }
            }

        }
        return true;
    }
}

