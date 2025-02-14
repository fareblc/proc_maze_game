﻿using UnityEngine;
using UnityEngine.Audio;

public class MazeConstructor : MonoBehaviour
{
    public bool showDebug;

    [SerializeField] private Material wallMaterial; //Wall Material
    [SerializeField] private Material floorMaterial; //Floor and Ceiling Material
    [SerializeField] private Material startMat;
    [SerializeField] private Material treasureMat;
    [SerializeField] private AudioClip clipForTreasure;
    [SerializeField] private GameObject enemy;
    public GameObject prefab;
    public GameObject prefabStart;



    public int[,] data
    {
        get; private set;
    }

    public float hallWidth
    {
        get; private set;
    }
    public float hallHeight
    {
        get; private set;
    }

    public int startRow
    {
        get; private set;
    }
    public int startCol
    {
        get; private set;
    }

    public int goalRow
    {
        get; private set;
    }
    public int goalCol
    {
        get; private set;
    }

    public int enemyRow
    {
        get; private set;
    }
    public int enemyCol
    {
        get; private set;
    }

    private MazeDataGenerator dataGenerator;
    private MazeMeshGenerator meshGenerator;

    private void Start()
    {

    }

    void Awake()
    {



        dataGenerator = new MazeDataGenerator();
        meshGenerator = new MazeMeshGenerator();



        // default to walls surrounding a single empty cell
        data = new int[,]
        {
            {1, 1, 1},
            {1, 0, 1},
            {1, 1, 1}
        };
    }

    public void GenerateNewMaze(int sizeRows, int sizeCols,
        TriggerEventHandler startCallback=null, TriggerEventHandler goalCallback=null, int reDo=0)
    {
        //For the future
        //Material[] listOfWallMats = (Material[])Resources.LoadAll("Assets/MaterialsToUse/WallMats", typeof(Material));
        //Material[] listOfFloorMats = (Material[])Resources.LoadAll("Assets/MaterialsToUse/WallMats", typeof(Material));

        ////wallMaterial = listOfWallMats[Random.Range(0, listOfWallMats.Length - 1)];
        ////floorMaterial = listOfFloorMats[Random.Range(0, listOfFloorMats.Length - 1)];


        if (reDo == 0)
        {
            if (sizeRows % 2 == 0 && sizeCols % 2 == 0)
            {
                Debug.LogError("Odd numbers work better for maze size.");
            }

            DisposeOldMaze();

            data = dataGenerator.FromDimensions(sizeRows, sizeCols);

            FindStartPosition();
            FindGoalPosition();
            EnemyPlacePosition();


            // store values used to generate this mesh
            hallWidth = meshGenerator.width;
            hallHeight = meshGenerator.height;

            DisplayMaze();

            PlaceStartTrigger(startCallback);
            PlaceGoalTrigger(goalCallback);
            GetNewEnemy();
        }
    }

    public void ReplaceTreasure(int sizeRows, int sizeCols,
        TriggerEventHandler startCallback = null, TriggerEventHandler goalCallback = null)
    {
        if (sizeRows % 2 == 0 && sizeCols % 2 == 0)
        {
            Debug.LogError("Odd numbers work better for maze size.");
        }
        hallWidth = meshGenerator.width;
        hallHeight = meshGenerator.height;
        PlaceGoalTrigger(goalCallback);
        GoalReplacePosition();
        EnemyPlacePosition();
        GetNewEnemy();
    }

    private void DisplayMaze()
    {

        GameObject go = new GameObject();
        go.transform.position = Vector3.zero;
        go.name = "Procedural Maze";
        go.tag = "Generated";

        MeshFilter mf = go.AddComponent<MeshFilter>();
        mf.mesh = meshGenerator.FromData(data);

        MeshCollider mc = go.AddComponent<MeshCollider>();
        mc.sharedMesh = mf.mesh;

        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        mr.materials = new Material[2] {wallMaterial, floorMaterial};
    }

    public void DisposeOldMaze()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Generated");
        foreach (GameObject go in objects) {
            Destroy(go);
        }
    }

    private void FindStartPosition()
    {
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        for (int i = 0; i <= rMax; i++)
        {
            for (int j = 0; j <= cMax; j++)
            {
                if (maze[i, j] == 0)
                {
                    startRow = i;
                    startCol = j;
                    return;
                }
            }
        }
    }

    private void FindGoalPosition(int justReDo = 0)
    {
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);
        int rMin = maze.GetLowerBound(0);
        int cMin = maze.GetLowerBound(1);

        if (justReDo == 0)
        {
            // loop top to bottom, right to left
            for (int i = rMax; i >= 0; i--)
            {
                for (int j = cMax; j >= 0; j--)
                {
                    if (maze[i, j] == 0)
                    {
                        goalRow = i;
                        goalCol = j;
                        return;
                    }
                }
            }
        }
    }

    private void GoalReplacePosition()
    {
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);
        int rMin = maze.GetLowerBound(0);
        int cMin = maze.GetLowerBound(1);

        int a = UnityEngine.Random.Range(rMin, rMax);
        int b = UnityEngine.Random.Range(cMin, cMax);
        for (int i = rMax; i >= 0; i--)
        {
            for (int j = cMax; j >= 0; j--)
            {
                if ((i + a < rMax) && (j + b < cMax))
                {
                    if (maze[i, j] != 1)
                    {
                        goalRow = i;
                        goalCol = j;
                        return;
                    }
                }
            }
        }
    }

    private void EnemyPlacePosition()
    {
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);
        int rMin = maze.GetLowerBound(0);
        int cMin = maze.GetLowerBound(1);

        int a = UnityEngine.Random.Range(rMin, rMax);
        int b = UnityEngine.Random.Range(cMin, cMax);
        for (int i = goalRow; i >= 0; i--)
        {
            for (int j = goalCol; j >= 0; j--)
            {
                    if (maze[i, j] != 1 && (i + j != goalRow + goalCol))
                    {
                        enemyRow = i;
                        enemyCol = j;
                        return;
                    }
            }
        }
    }

    private void PlaceStartTrigger(TriggerEventHandler callback)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Plane);
        go.AddComponent<BoxCollider>();
        go.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        go.transform.position = new Vector3(startCol * hallWidth, .1f, startRow * hallWidth);
        go.name = "Start Trigger";
        go.tag = "Generated";

        go.GetComponent<BoxCollider>().isTrigger = true;
        go.GetComponent<MeshRenderer>().sharedMaterial = startMat;

        TriggerEventRouter tc = go.AddComponent<TriggerEventRouter>();
        tc.callback = callback;
    }

    private void PlaceGoalTrigger(TriggerEventHandler callback)
    {
        GameObject go = Instantiate(prefab);
        go.transform.position = new Vector3(goalCol * hallWidth, .1f, goalRow * hallWidth);
        go.AddComponent<BoxCollider>();
        go.AddComponent(typeof(AudioSource));
        go.name = "Treasure";
        go.tag = "Generated";

        go.GetComponent<BoxCollider>().isTrigger = true;
        go.GetComponent<MeshRenderer>().sharedMaterial = treasureMat;
        go.GetComponent<AudioSource>().clip = clipForTreasure;
        go.GetComponent<AudioSource>().loop = true;
        go.GetComponent<AudioSource>().spatialBlend = 3f;
        go.GetComponent<AudioSource>().Play();

        TriggerEventRouter tc = go.AddComponent<TriggerEventRouter>();
        tc.callback = callback;
    }

    private void GetNewEnemy() 
    {
        GameObject go = enemy;
        go.SetActive(true);
        go.transform.position = new Vector3(enemyCol * hallWidth, .5f, enemyRow * hallWidth);
        go.name = "Enemy";
        go.tag = "Generated";
    }

    void OnGUI()
    {
        if (!showDebug)
        {
            return;
        }

        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        string msg = "";

        for (int i = rMax; i >= 0; i--)
        {
            for (int j = 0; j <= cMax; j++)
            {
                if (maze[i, j] == 0)
                {
                    msg += "....";
                }
                else
                {
                    msg += "==";
                }
            }
            msg += "\n";
        }

        GUI.Label(new Rect(20, 20, 500, 500), msg);
    }
}
