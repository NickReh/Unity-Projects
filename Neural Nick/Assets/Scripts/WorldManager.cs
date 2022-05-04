using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {
    public GameObject Floor;
    public GameObject WallPrefab;
    public GameObject FoodPreFab;

    public static int[] Grid { get; set; }

    public int size;
    public static int Size { get; set; }

    public Nick nick;

    void Start () {
        Size = size; //assign inspector variable to static variable since static variables can't be viewed or assigned from ide.

        Camera.main.transform.position = new Vector3(size/2, size, size/2);

        InitFloor();

        InitGrid();

        nick.Init(size);
    }

    void Update () {

    }

    void InitFloor() {
        //resize floor
        Floor.transform.localScale = new Vector3(size/10-0.1f, 1f, size/10-0.1f);
        Floor.transform.position = new Vector3(size/2, 0, size/2);
    }

    void InitGrid() {
        System.Random rnd = new System.Random();

        Grid = new int[size*size];

        for (var row = 0; row < this.size - 1; row++) 
            {
                for (var col = 0; col < this.size - 1; col++) 
                {
                    if (row <= 3 || row >= this.size - 4 || col <= 3 || col >= this.size - 4 || rnd.NextDouble() < .05) 
                    {
                        Grid[row*this.size + col] = -1;
                        Grid[row*this.size + col + 1] = -1;
                        Grid[(row+1)*this.size + col] = -1;
                        Grid[(row+1)*this.size + col + 1] = -1;
                        Instantiate(WallPrefab, new Vector3(row + 0.5f, 0.5f, col + 0.5f), Quaternion.identity);
                    }
                    else if (rnd.NextDouble() < .1) 
                    {
                        Grid[row*this.size + col] = 1;
                        Grid[row*this.size + col + 1] = 1;
                        Grid[(row+1)*this.size + col] = 1;
                        Grid[(row+1)*this.size + col + 1] = 1;
                        Instantiate(FoodPreFab, new Vector3(row + 0.5f, 0.125f, col + 0.5f), Quaternion.identity);
                    }
                    else //if (!Grid[row*this.size + col]) 
                    {
                        Grid[row*this.size + col] = 0;
                    }
                }
            }
        }
}