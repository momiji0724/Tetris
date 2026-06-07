using System.IO.IsolatedStorage;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.VFX;

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

    // 着地猶予設定
    private bool isGrounded = false;
    private float lockTimer = 0f;
    public float lockDelay = 0.5f;

    [Header("Ghost Settings")]
    public GameObject ghostPrefab;
    private GameObject ghostObject;

    void Start()
    {
        if(this.enabled && ghostPrefab != null) 
        {
            ghostObject = Instantiate(ghostPrefab, transform.position, transform.rotation);
        }

        foreach(Transform child in transform) 
        {
            GameObject ghostChild = Instantiate(child.gameObject, ghostObject.transform);
            ghostChild.transform.localPosition = child.localPosition;

            SpriteRenderer sr = ghostChild.GetComponent<SpriteRenderer>();
            if(sr != null) 
            {
                Color c = sr.color;
                sr.color = new Color(c.r, c.g, c.b, 0.35f);
            }
        }
    }
    void Update()
    {
        MinoMovement();
        HandleLockDelay();

        UpdateGhostPosition();
    }

    private void UpdateGhostPosition ()
    {
        if (ghostObject == null) return;
        ghostObject.transform.rotation = transform.rotation;

        Vector3 ghostPosition = transform.position;
        int loopSafety = 0;

        while (true) 
        {
            loopSafety++;
            if(loopSafety > 30) 
            {
                break;
            }
            ghostPosition += new Vector3(0, -1, 0);

            if (!ValidGhostMovement(ghostPosition)) 
            {
                ghostPosition -= new Vector3(0, -1, 0);
                break;
            }
        }
        ghostObject.transform.position = ghostPosition;
    }

    private void OnDisable()
    {
        if (ghostObject != null) 
        {
            Destroy(ghostObject);
        }
    }
    private void OnDestroy()
    {
        if (ghostObject != null)
        {
            Destroy(ghostObject);
        }
    }

    public static void ClearGrid()
    {
        grid = new Transform[width, height];
    }

    private void MinoMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.C))
        {
            SpawnMino spawner = FindObjectOfType<SpawnMino>();
            if (spawner != null)
            {
                spawner.HoldMino();
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            isGrounded = false;

            while (true)
            {
                transform.position += new Vector3(0, -1, 0);

                if (!ValidMovement())
                {
                    transform.position -= new Vector3(0, -1, 0);
                    break;
                }

            }

            isGrounded = true;

            LockMinoImmediately();
            return;
        }

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
                else if (isGrounded) 
                {
                    lockTimer = 0f;
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
                else if (isGrounded)
                {
                    lockTimer = 0f;
                }

                horizontalTimer = 0;

            }
        }
        if (Time.time - previousTime >= fallTime)
        {
            if (!isGrounded) 
            {
                MoveDown();
            }
            
            previousTime = Time.time;

        }

        downTimer += Time.deltaTime;
        if (downTimer >= downInterval)
        {
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                if (!isGrounded)
                {
                    MoveDown();
                }
                
            }

            downTimer = 0;
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
                if (isGrounded) 
                {
                    lockTimer = 0f;
                    
                }
                return;
            }

            transform.position += Vector3.right;

            if (ValidMovement())
            {
                if (isGrounded)
                {
                    lockTimer = 0f;
                    
                }
                return;
            }

            transform.position += Vector3.left * 2;
            if (ValidMovement())
            {
                if (isGrounded)
                {
                    lockTimer = 0f;
                    
                }
                return;
            }

            transform.position = oldPosition;
            transform.rotation = oldRotation;

        }

        CheckIfGrounded();

    }

    public void MoveDown()
    {
        transform.position += new Vector3(0, -1, 0);
        if (!ValidMovement())
        {
            transform.position -= new Vector3(0, -1, 0);
            isGrounded = true;
        }
    }

    private void CheckIfGrounded() 
    {
        transform.position += new Vector3(0, -1, 0);

        if (!ValidMovement()) 
        {
            isGrounded = true;
        }
        else 
        {
            isGrounded = false;
        }
        transform.position -= new Vector3(0, -1, 0);
    }

    private void HandleLockDelay()
    {
        if (isGrounded) 
        {
            lockTimer += Time.deltaTime;
            if(lockTimer >= lockDelay) 
            {
                LockMinoImmediately();
            }
            
        }
        else
        {
            lockTimer = 0f;
        }
    }

    private bool isLocked = false;
    private void LockMinoImmediately()
    {
        if (isLocked)
        {
            Debug.LogError("DOUBLE LOCK!");
            return;
        }

        isLocked = true;

        Debug.Log($"Lock called {name}");

        this.enabled = false;

        AddToGrid();
        CheckLines();

        FindObjectOfType<SpawnMino>().NewMino();
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
               roundY >= 0 && roundY < height) 
            {
                
                if (grid[roundX, roundY] != null)
                {
                    Debug.LogError($"REAL OVERLAP {roundX},{roundY}");
                }

                grid[roundX, roundY] = children;

            }



        }
    }


    public bool ValidMovement()
    {
        foreach (Transform child in transform)
        {
            int x = Mathf.RoundToInt(child.position.x);
            int y = Mathf.RoundToInt(child.position.y);

            if (x < 0 || x >= width || y < 0)
            {
                Debug.Log($"Wall collision x={x} y={y}");
                return false;
            }

            if (y < height)
            {
                if (grid[x, y] != null)
                {
                    Debug.Log($"Grid collision x={x} y={y}");
                    return false;
                }
            }
        }

        return true;
    }

    public bool ValidGhostMovement(Vector3 ghostPos) 
    {
        foreach(Transform child in transform) 
        {
            Vector3 relativePos = child.position - transform.position;

            int x = Mathf.RoundToInt(ghostPos.x + relativePos.x);
            int y = Mathf.RoundToInt(ghostPos.y + relativePos.y);

            if(x < 0 || x >= width || y< 0) 
            {
                return false;
            }

            if(y < height) 
            {
                if (grid[x,y]!= null) 
                {
                    return false;
                }
            }
        }
        return true;
    }
}

