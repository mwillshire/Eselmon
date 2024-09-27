using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GridInfo : MonoBehaviour
{
    [Header("Inscribed")]
    public int firstGridIndex; //Scene Index

    public List<Chest> chests { get; private set; }
    public List<Enemy> enemies { get; private set; }


    private bool firstPassDone = false; //For the csv reader
    private bool spawnedChests = false;
    private bool spawnedEnemy = false;

    private void Awake()
    {
        chests = new List<Chest>();
        enemies = new List<Enemy>();
    }
    private void Start()
    {
        ReadChestCSVFile();
        ReadEnemyCSVFile();
        ReadGridCSVFile();

        if (!spawnedChests)
        {
            spawnedChests = true;

            foreach (Chest chest in chests)
            {
                chest.SpawnChests();
            }
        }

        if (!spawnedEnemy)
        {
            spawnedEnemy = true;

            foreach (Enemy enemy in enemies)
            {
                enemy.SpawnEnemy();
            }
        }

    }
    private void ReadChestCSVFile()
    {
        StreamReader strReader = new StreamReader("Assets/Resources/CSV/ChestID.csv");
        bool endOfFile = false;
        firstPassDone = false;

        while (!endOfFile)
        {
            string dataString = strReader.ReadLine();
            if (dataString == null)
            {
                endOfFile = true;
                break;
            }

            var dataValues = dataString.Split(",");

            if (firstPassDone)
            {
                chests.Add(new Chest(dataValues[0], dataValues[1], dataValues[2]));

            }

            firstPassDone = true;
        }

        strReader.Close();
    }

    private void ReadEnemyCSVFile()
    {
        StreamReader strReader = new StreamReader("Assets/Resources/CSV/EnemyID.csv");
        bool endOfFile = false;
        firstPassDone = false;

        while (!endOfFile)
        {
            string dataString = strReader.ReadLine();
            if (dataString == null)
            {
                endOfFile = true;
                break;
            }

            var dataValues = dataString.Split(",");

            if (firstPassDone)
            {
                enemies.Add(new Enemy(dataValues[0], dataValues[1], dataValues[2]));

            }

            firstPassDone = true;
        }

        strReader.Close();
    }
    private void ReadGridCSVFile()
    {
        StreamReader strReader = null;
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 10:
                strReader = new StreamReader("Assets/Resources/CSV/Grid00.csv");
                break;
            
        }
        if (strReader == null)
        {
            strReader.Close();
            return;
        }

        bool endOfFile = false;
        firstPassDone = false;

        while (!endOfFile)
        {
            string dataString = strReader.ReadLine();
            if (dataString == null)
            {
                endOfFile = true;
                break;
            }

            var dataValues = dataString.Split(",");

            if (firstPassDone)
            {
                float xLoc = (float)Convert.ToDouble(dataValues[1]);
                float yLoc = (float)Convert.ToDouble(dataValues[2]);

                if (dataValues[0].Substring(0,1) == "C")
                {
                    foreach (var chest in chests)
                    {
                        if (dataValues[0] == chest.chestID)
                        {
                            chest.NewChest(new Vector2(xLoc, yLoc));
                        }
                    }
                }
                else if (dataValues[0].Substring(0,1) == "E")
                {
                    foreach (var enemy in enemies)
                    {
                        if (dataValues[0] == enemy.enemyID)
                        {
                            enemy.AddEnemy(new Vector2(xLoc, yLoc));
                        }
                    }
                }
                else
                {
                    Debug.Log("Unknown enemy or chest found in Grid CSVFile");
                }

            }

            firstPassDone = true;
        }

        strReader.Close();
    }
}
