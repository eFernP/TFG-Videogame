using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSpawn : MonoBehaviour
{

    public GameObject floor;
    private float[] FloorSize = new float[2];

    public GameObject Attack;
    public GameObject Stone;


    public static int MAX_STONE_NUMBER = 5;
    //public static int MAX_PROJECTILES_NUMBER = 30;
    //private int projectilesCounter = 0;

    private List<GameObject> currentStones = new List<GameObject>(MAX_STONE_NUMBER);


    private BattleManager BattleScript;

    float SPAWN_TIME = 0.2f;
    float SpawnTimer;
    float SpawnStoneTimer;
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer FloorRenderer = floor.GetComponent<MeshRenderer>();
        FloorSize[0] = FloorRenderer.bounds.size.x;
        FloorSize[1] = FloorRenderer.bounds.size.z;
        SpawnTimer = SPAWN_TIME;
        SpawnStoneTimer = Random.Range(SPAWN_TIME * 10, SPAWN_TIME * 20);
        BattleScript = this.GetComponent<BattleManager>();
    }


    Vector3 getRandomPosition(float margin)
    {
        return new Vector3(Random.Range(this.transform.position.x - FloorSize[0]/2 + margin, this.transform.position.x + FloorSize[0] / 2 - margin), 12f, Random.Range(this.transform.position.z - FloorSize[1] / 2 +margin, this.transform.position.z + FloorSize[1] / 2-margin));
    }

    void SpawnProjectile()
    {
        Instantiate(Attack, getRandomPosition(0f), Quaternion.identity);
    }

    void SpawnStone()
    {
        currentStones.Add(Instantiate(Stone, getRandomPosition(10f), Quaternion.identity));
    }

    // Update is called once per frame
    void Update()
    {
        if (BattleScript.GetCurrentPhase() == 2)
        {
            SpawnTimer -= Time.deltaTime;
            SpawnStoneTimer -= Time.deltaTime;
            if (SpawnTimer <= 0)
            {
                SpawnProjectile();
                //projectilesCounter++;
                SpawnTimer = SPAWN_TIME;
            }


            if (SpawnStoneTimer <= 0)
            {
                currentStones.RemoveAll(item => item == null);
                if (currentStones.Count < MAX_STONE_NUMBER)
                {
                    SpawnStone();
                    SpawnStoneTimer = Random.Range(SPAWN_TIME * 10, SPAWN_TIME * 20);
                }
            }
        }
        //else
        //{
        //    if(projectilesCounter > 0)
        //    {
        //        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("MagicDamage");
        //        foreach (GameObject p in projectiles)
        //        {
        //            Destroy(p);
        //        }
        //        projectilesCounter = 0;
        //    }

        //}

    }
}
