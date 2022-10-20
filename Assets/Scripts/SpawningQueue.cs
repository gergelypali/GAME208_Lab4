using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// enum for the different enemy types
public enum UnitType { unit1, unit2, unit3 }
// struct to store the spawning location and the enemy itself
public struct Unit { public UnitType type; public Enemy enemy; public Vector3 spawnVector; }
public class SpawningQueue : MonoBehaviour
{
    // for the spawning mechanic
    public Enemy unit1Prefab;
    public Enemy unit2Prefab;
    public Enemy unit3Prefab;
    public Transform unitSpawnPoint;

    // textmeshpros to display data on the screen
    // spawn timer in seconds
    public TextMeshProUGUI unit1SpawnTimer;
    public TextMeshProUGUI unit2SpawnTimer;
    public TextMeshProUGUI unit3SpawnTimer;

    // input field to set the max number of the enemy in the spawn queue
    public TMP_InputField unit1MaxQueueInput;
    public TMP_InputField unit2MaxQueueInput;
    public TMP_InputField unit3MaxQueueInput;

    // display the enemy's current number in the spawn queue
    public TextMeshProUGUI unit1InQueue;
    public TextMeshProUGUI unit2InQueue;
    public TextMeshProUGUI unit3InQueue;

    // buttons to change what to spawn
    public Button unit1Button;
    public Button unit2Button;
    public Button unit3Button;

    // counter for each type of unit to shown on the screen because we cannot search in the Queue
    private int unit1Count = 0, unit2Count = 0, unit3Count = 0;
    private int unit1MaxInQueue = 0, unit2MaxInQueue = 0, unit3MaxInQueue = 0;

    // store in this what to spawn at this time
    private UnitType whatToSpawn;

    // to force wait each spawn, so wont be any spawning at the same time
    bool spawnActive;

    // create a queue with Enemy type
    public Queue<Unit> spawningQueue;
    // Start is called before the first frame update
    void Start()
    {
        spawnActive = false;
        spawningQueue = new Queue<Unit>();
        // get the initial values from the prefabs
        unit1MaxInQueue = unit1Prefab.maxQueue;
        unit2MaxInQueue = unit2Prefab.maxQueue;
        unit3MaxInQueue = unit3Prefab.maxQueue;

        //check for missing public parameters
        if (!unit1Prefab)
        {
            Debug.Log(name + ": Missing unit1Prefab");
        }

        if (!unit2Prefab)
        {
            Debug.Log(name + ": Missing unit2Prefab");
        }

        if (!unit3Prefab)
        {
            Debug.Log(name + ": Missing unit3Prefab");
        }

        if (!unitSpawnPoint)
        {
            Debug.Log(name + ": Missing unitSpawnPoint");
        }

        // add the listener functions to the button and to the input field
        unit1Button.onClick.AddListener(unit1Choosen);
        unit2Button.onClick.AddListener(unit2Choosen);
        unit3Button.onClick.AddListener(unit3Choosen);

        unit1MaxQueueInput.onSubmit.AddListener(unit1MaxQueueSet);
        // set the default text to the input field as the initial value from prefab
        unit1MaxQueueInput.placeholder.GetComponent<TextMeshProUGUI>().text = unit1MaxInQueue.ToString();
        unit2MaxQueueInput.onSubmit.AddListener(unit2MaxQueueSet);
        unit2MaxQueueInput.placeholder.GetComponent<TextMeshProUGUI>().text = unit2MaxInQueue.ToString();
        unit3MaxQueueInput.onSubmit.AddListener(unit3MaxQueueSet);
        unit3MaxQueueInput.placeholder.GetComponent<TextMeshProUGUI>().text = unit3MaxInQueue.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // update the queue timer for each prefab
        unit1InQueue.text = unit1Count.ToString();
        unit2InQueue.text = unit2Count.ToString();
        unit3InQueue.text = unit3Count.ToString();
        // use keypad 1-2-3 to spawn the three enemy types

        if (Input.GetButtonDown("Fire2"))
        {
            switch (whatToSpawn)
            {
                case UnitType.unit1:
                    // can spawn if below the max queue value
                    if (unit1Count < unit1MaxInQueue)
                    {
                        spawnUnit(UnitType.unit1);
                        Debug.Log("Spawn Unit1!");
                    }
                    else
                    {
                        Debug.Log("Cannot spawn Unit1! Max queue reached. Wait for spawn!");
                    }
                    break;
                case UnitType.unit2:
                    if (unit2Count < unit2MaxInQueue)
                    {
                        spawnUnit(UnitType.unit2);
                        Debug.Log("Spawn Unit2!");
                    }
                    else
                    {
                        Debug.Log("Cannot spawn Unit2! Max queue reached. Wait for spawn!");
                    }
                    break;
                case UnitType.unit3:
                    if (unit3Count < unit3MaxInQueue)
                    {
                        spawnUnit(UnitType.unit3);
                        Debug.Log("Spawn Unit3!");
                    }
                    else
                    {
                        Debug.Log("Cannot spawn Unit3! Max queue reached. Wait for spawn!");
                    }
                    break;
                default:
                    break;
            }
        }

        // if there anything in the spawningqueue, it will be spawned to the scene
        if (!spawnActive && spawningQueue != null && spawningQueue.Count > 0)
        {
            spawnActive = true;
            StartCoroutine(spawnUnitWithDelay(spawningQueue.Dequeue()));
        }
    }
    void spawnUnit(UnitType unit)
    {
        // differentiate the enemy types so the correct prefab will be spawned
        switch (unit)
        {
            case UnitType.unit1:
                // get the current location vector of the spawnpoint object and store it with the prefab to spawn later
                // so the prefab will spawn to this location and not the actual location of the spawnpoint
                Vector3 unit1Vec = new Vector3(unitSpawnPoint.position.x, unit1Prefab.yOffset, unitSpawnPoint.position.z);
                Unit queuable1 = new Unit() { type = UnitType.unit1, enemy = unit1Prefab, spawnVector = unit1Vec };
                spawningQueue.Enqueue(queuable1);
                unit1Count++;
                break;
            case UnitType.unit2:
                Vector3 unit2Vec = new Vector3(unitSpawnPoint.position.x, unit2Prefab.yOffset, unitSpawnPoint.position.z);
                Unit queuable2 = new Unit() { type = UnitType.unit2, enemy = unit2Prefab, spawnVector = unit2Vec };
                spawningQueue.Enqueue(queuable2);
                unit2Count++;
                break;
            case UnitType.unit3:
                Vector3 unit3Vec = new Vector3(unitSpawnPoint.position.x, unit2Prefab.yOffset, unitSpawnPoint.position.z);
                Unit queuable3 = new Unit() { type = UnitType.unit3, enemy = unit3Prefab, spawnVector = unit3Vec };
                spawningQueue.Enqueue(queuable3);
                unit3Count++;
                break;
            default:
                Debug.Log("this is the default, so it is bad!");
                break;
        }
    }

    // helper functions that are triggered from the UI buttons or input fields
    void unit1Choosen()
    {
        whatToSpawn = UnitType.unit1;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void unit2Choosen()
    {
        whatToSpawn = UnitType.unit2;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void unit3Choosen()
    {
        whatToSpawn = UnitType.unit3;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void unit1MaxQueueSet(string arg0)
    {
        unit1MaxInQueue = int.Parse(arg0);
    }
    void unit2MaxQueueSet(string arg0)
    {
        unit2MaxInQueue = int.Parse(arg0);
    }
    void unit3MaxQueueSet(string arg0)
    {
        unit3MaxInQueue = int.Parse(arg0);
    }

    // coroutine to spawn with a delay coming from the prefab; vector is needed so the spawned enemy is on the ground
    // and not inside it(and shoot up into the sky)
    IEnumerator spawnUnitWithDelay(Unit prefab)
    {
        // display the timers for each spawn
        int i = prefab.enemy.spawnTime;
        while (i > 0) {
            switch (prefab.type)
            {
                case UnitType.unit1:
                    unit1SpawnTimer.text = i.ToString();
                    break;
                case UnitType.unit2:
                    unit2SpawnTimer.text = i.ToString();
                    break;
                case UnitType.unit3:
                    unit3SpawnTimer.text = i.ToString();
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(1);
            i--;
        }
        
        Rigidbody tempUnit = Instantiate(prefab.enemy.rb, prefab.spawnVector, unitSpawnPoint.rotation);
        switch (prefab.type)
        {
            case UnitType.unit1:
                unit1Count--;
                unit1SpawnTimer.text = "0";
                break;
            case UnitType.unit2:
                unit2Count--;
                unit2SpawnTimer.text = "0";
                break;
            case UnitType.unit3:
                unit3Count--;
                unit3SpawnTimer.text = "0";
                break;
            default:
                break;
        }
        spawnActive = false;
    }

}
