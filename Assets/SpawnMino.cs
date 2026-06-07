using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMino : MonoBehaviour
{
    public GameObject[] Minos;

    [Header("UI / Preview Positions")]
    public Transform nextPosition; // NextÉ~ÉmÇï\é¶
    public Transform holdPosition; // ÉzÅ[ÉãÉhÉ~ÉmÇï\é¶

    private List<int> bag = new List<int>();

    private int currentMinoIndex = -1;
    private int nextMinoIndex = -1;
    private int holdMinoIndex = -1;
    private bool hasHeldThisTurn = false;

    private GameObject nextPreviewObject;
    private GameObject holdPreviewObject;

    private GameObject activeMinoObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FillBag();

        nextMinoIndex = GetNextFromBag();
        UpdateNextPreview();

        NewMino();
    }

    private int GetNextFromBag() 
    {
        if(bag.Count == 0) 
        {
            FillBag();
        }
        int index = bag[0];
        bag.RemoveAt(0);
        return index;
    }

    public void NewMino() 
    {

        hasHeldThisTurn = false;

        currentMinoIndex = nextMinoIndex;

        nextMinoIndex = GetNextFromBag();
        UpdateNextPreview();

        SpawnActiveMino(currentMinoIndex);
        
    }

    private void SpawnActiveMino(int index) 
    {
        activeMinoObject = Instantiate(Minos[index], transform.position, Quaternion.identity);

        Mino minoScript = activeMinoObject.GetComponent<Mino>();
        minoScript.enabled = true;

        if (!minoScript.ValidMovement()) 
        {
            FindObjectOfType<GameManagement>().GameOver();
        }
    }

    public void HoldMino() 
    {
        if (hasHeldThisTurn) return;
        hasHeldThisTurn = true;

        if(activeMinoObject != null) 
        {
            Destroy(activeMinoObject);
        }

        if(holdMinoIndex == -1) 
        {
            holdMinoIndex = currentMinoIndex;
            UpdateHoldPreview();

            NewMino();
        }
        else 
        {
            int temp = currentMinoIndex;
            currentMinoIndex = holdMinoIndex;
            holdMinoIndex = temp;

            UpdateHoldPreview();
            SpawnActiveMino(currentMinoIndex);
        }
        Debug.Log("HOLD");
    }

    private void UpdateNextPreview() 
    {
        if(nextPreviewObject != null) 
        {
            Destroy(nextPreviewObject);
        }
        if(nextPosition != null && nextMinoIndex != -1) 
        {
            Vector3 spawnPos = nextPosition.position;

            if(nextMinoIndex == 0)  // IÉ~Ém
            {
                spawnPos += new Vector3(-0.5f, -0.5f, 0);
            }
            else if (nextMinoIndex == 2)  // LÉ~Ém
            {
                spawnPos += new Vector3(-1.0f, 0f, 0);
            }
            else if (nextMinoIndex == 3)  // OÉ~Ém
            {
                spawnPos += new Vector3(-1.0f, 0.5f, 0);
            }
            else if (nextMinoIndex == 4)  // SÉ~Ém
            {
                spawnPos += new Vector3(-0.5f, 0.5f, 0);
            }
            else if (nextMinoIndex == 5)  // TÉ~Ém
            {
                spawnPos += new Vector3(-0.5f,0.5f, 0);
            }

            else if (nextMinoIndex == 6)  // ZÉ~Ém
            {
                spawnPos += new Vector3(-0.5f, 0f, 0);
            }


            nextPreviewObject = Instantiate(Minos[nextMinoIndex], spawnPos, Quaternion.identity);

            Mino minoScript = nextPreviewObject.GetComponent<Mino>();
            if (minoScript != null)
            {
                Destroy(minoScript);
            }
        }
    }

    private void UpdateHoldPreview() 
    {
        if (holdPreviewObject != null) 
        {
            Destroy(holdPreviewObject);
        }
        if(holdPosition != null && holdMinoIndex != -1) 
        {
            Vector3 spawnPos = holdPosition.position;

            if (holdMinoIndex == 0)  // IÉ~Ém
            {
                spawnPos += new Vector3(0.5f, -0.5f, 0);
            }
            else if (holdMinoIndex == 1)  // JÉ~Ém
            {
                spawnPos += new Vector3(1.0f, 0, 0);
            }
            else if (holdMinoIndex == 3)  // OÉ~Ém
            {
                spawnPos += new Vector3(0f, 0.5f, 0);
            }
            else if (holdMinoIndex == 4)  // SÉ~Ém
            {
                spawnPos += new Vector3(0.5f, 0.5f, 0);
            }
            else if (holdMinoIndex == 5)  // TÉ~Ém
            {
                spawnPos += new Vector3(0.5f, 0.5f, 0);
            }
            else if (holdMinoIndex == 6)  // ZÉ~Ém
            {
                spawnPos += new Vector3(0.5f, 0.5f, 0);
            }



            holdPreviewObject = Instantiate(Minos[holdMinoIndex], spawnPos, Quaternion.identity);

            Mino minoScript = holdPreviewObject.GetComponent<Mino>();
            if (minoScript != null)
            {
                Destroy(minoScript);
            }

        }
    }

    // ãœìôóêêîÇÃÇΩÇﬂÇÃbagçÏÇË
    public void FillBag() 
    {
        bag.Clear();

        for(int i = 0; i< Minos.Length; i++) 
        {
            bag.Add(i);
        }

        for(int i = 0; i< bag.Count; i++) 
        {
            int randomIndex = Random.Range(i, bag.Count);

            int temp = bag[i];
            bag[i] = bag[randomIndex];
            bag[randomIndex] = temp;
        }
    } 
    // Update is called once per frame
    void Update()
    {
        
    }
}
