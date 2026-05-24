using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMino : MonoBehaviour
{
    public GameObject[] Minos;

    private List<int> bag = new List<int>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FillBag();
        NewMino();
    }

    public void NewMino() 
    {
        // bag‚Ì’†g‚ª0‚É‚È‚Á‚½‚ç•â[
        if(bag.Count == 0) 
        {
            FillBag();
        }

        int index = bag[0];
        bag.RemoveAt(0);

        GameObject newMino =
        Instantiate(Minos[index], transform.position, Quaternion.identity);

        if (!newMino.GetComponent<Mino>().ValidMovement()) 
        {
            FindObjectOfType<GameManagement>().GameOver();
        }
    }

    // ‹Ï“™—”‚Ì‚½‚ß‚Ìbagì‚è
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
