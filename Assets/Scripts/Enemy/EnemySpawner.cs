using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<GameObject> special1EnemyPrefabs;

    [SerializeField] private float radius = 5f;
    [SerializeField] private int maxEnemiesSpawn = 10;
    [SerializeField] private float spawnCooldown = 5f;
    [SerializeField] private FloatReference maxEnemiesMultipliyer;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float timeToSpawnType2 = 300f;
    [SerializeField] private float timeToSpawnType3 = 600f;
    private float m_nextSpawnTime;
    private float m_nextChangeAngleTime;
    private float m_angle;
    private const float ANGLE_DELTA = 5;
    private float timeElapsed;
    private Dictionary<TypeOfEnemy, GameObject> enemyTypeToPrefab = new Dictionary<TypeOfEnemy, GameObject>();
    private bool isSpawned = false;
    private void Start()
    {
        resetAngle();
        m_nextSpawnTime = spawnCooldown;
        timeElapsed = 0;

    }

    private void Update()
    {


        m_nextSpawnTime -= Time.deltaTime;
        m_nextChangeAngleTime -= Time.deltaTime;
        if (!isSpawned) // Kiểm tra xem đã spawn hay chưa
        {
            if (timeElapsed < timeToSpawnType2)
            {
                StartCoroutine(spawnEnemies(TypeOfEnemy.EnemyType1));
                isSpawned = true; // Đánh dấu là đã spawn
            }
            else if (timeElapsed < timeToSpawnType3)
            {
                int randomType = Random.Range(1, 3); // Randomly choose between type 1 and type 2
                if (randomType == 1)
                {
                    StartCoroutine(spawnEnemies(TypeOfEnemy.EnemyType1));
                }
                else
                {
                    StartCoroutine(spawnEnemies(TypeOfEnemy.EnemyType2));
                }
                isSpawned = true; // Đánh dấu là đã spawn
            }
            else
            {
                int randomType = Random.Range(1, 4); // Randomly choose between type 1, type 2, and type 3
                if (randomType == 1)
                {
                    StartCoroutine(spawnEnemies(TypeOfEnemy.EnemyType1));
                }
                else if (randomType == 2)
                {
                    StartCoroutine(spawnEnemies(TypeOfEnemy.EnemyType2));
                }
                else
                {
                    StartCoroutine(spawnEnemies(TypeOfEnemy.EnemyType3));

                }
                isSpawned= true;
            }
        }

        if (timeElapsed >= 300f && timeElapsed < 600f)
        {
            SpawnNewTypeOfEnemy(TypeOfEnemy.EnemyTypeSpecial1);
        }
        else if (timeElapsed >= 600f && timeElapsed < 900f)
        {
            SpawnNewTypeOfEnemy(TypeOfEnemy.EnemyTypeSpecial2);
        }
        else if (timeElapsed >= 900f)
        {
            SpawnNewTypeOfEnemy(TypeOfEnemy.EnemyTypeSpecial3);
        }
        if (m_nextSpawnTime <= 0)
        {
            isSpawned = false; // Reset lại biến khi đến thời điểm spawn tiếp theo
        }
        if (m_nextSpawnTime <= 0)
        {
            //StartCoroutine(spawnEnemies(TypeOfEnemy.EnemyType1));
           

        }

        if (m_nextChangeAngleTime <= 0)
        {
            resetAngle();
        }
        timeElapsed += Time.deltaTime;
    }

    /// <summary>
    /// Instantiate an enemy in a random position around the player
    /// </summary>
    private void spawnEnemy(TypeOfEnemy enemyType)
    {
        float maxAngle = m_angle + ANGLE_DELTA;
        float minAngle = m_angle - ANGLE_DELTA;
        Vector3 center = transform.position;
        m_angle = Random.Range(minAngle, maxAngle);
        float angleInRadian = m_angle * Mathf.Deg2Rad;
        Vector3 pos = new Vector3(Mathf.Cos(angleInRadian), Mathf.Sin(angleInRadian), 0) * radius;
        //Instantiate(enemyPrefabs[0], center + pos, Quaternion.identity);
        Instantiate(enemyPrefabs[(int)enemyType], center + pos, Quaternion.identity);

    }

    private void resetAngle()
    {
        switch (playerController.playerDirection)
        {
            case PlayerDirection.East:
                m_angle = Random.Range(0 - ANGLE_DELTA, 0 + ANGLE_DELTA);
                break;
            case PlayerDirection.West:
                m_angle = Random.Range(180 - ANGLE_DELTA, 180 + ANGLE_DELTA);
                break;
            case PlayerDirection.North:
                m_angle = Random.Range(90 - ANGLE_DELTA, 90 + ANGLE_DELTA);
                break;
            case PlayerDirection.South:
                m_angle = Random.Range(270 - ANGLE_DELTA, 270 + ANGLE_DELTA);
                break;
        }
        m_nextChangeAngleTime = 10f;
    }

    private IEnumerator spawnEnemies(TypeOfEnemy enemyType)
    {
        m_nextSpawnTime = spawnCooldown;
        //for (int i = 0; i < maxEnemiesSpawn; i++)
        //{
        //    spawnEnemy(enemyType);
        //    yield return new WaitForSeconds(0.6f);
        //}
        int spawnedEnemies = 0; 
        while (spawnedEnemies < maxEnemiesSpawn)
        {
            spawnEnemy(enemyType);
            spawnedEnemies++;
            if (spawnedEnemies >= maxEnemiesSpawn) 
            {
                break;
            }
            yield return new WaitForSeconds(0.6f);
        }
    }

    public void setNewMaxEnemiesAmount()
    {
        maxEnemiesSpawn *= (int)maxEnemiesMultipliyer.value;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    private void SpawnNewTypeOfEnemy(TypeOfEnemy enemyType)
    {
        GameObject selectedEnemyPrefab = null;

        switch (enemyType)
        {
            case TypeOfEnemy.EnemyType1:
                selectedEnemyPrefab = enemyPrefabs[0];
                break;
            case TypeOfEnemy.EnemyType2:
                selectedEnemyPrefab = enemyPrefabs[1];
                break;
            case TypeOfEnemy.EnemyType3:
                selectedEnemyPrefab = enemyPrefabs[2];
                break;
            case TypeOfEnemy.EnemyTypeSpecial1:
                selectedEnemyPrefab = enemyPrefabs[0];// Assign the special enemy prefab for type 1;
                break;
            case TypeOfEnemy.EnemyTypeSpecial2:
                selectedEnemyPrefab = enemyPrefabs[2];// Assign the special enemy prefab for type 2;
                break;
            case TypeOfEnemy.EnemyTypeSpecial3:
                selectedEnemyPrefab = enemyPrefabs[3];// Assign the special enemy prefab for type 3;
                break;
        }

        if (selectedEnemyPrefab != null)
        {
            Vector3 center = transform.position;
            float maxAngle = m_angle + ANGLE_DELTA;
            float minAngle = m_angle - ANGLE_DELTA;
            m_angle = Random.Range(minAngle, maxAngle);
            float angleInRadian = m_angle * Mathf.Deg2Rad;
            Vector3 pos = new Vector3(Mathf.Cos(angleInRadian), Mathf.Sin(angleInRadian), 0) * radius;
            Instantiate(selectedEnemyPrefab, center + pos, Quaternion.identity);
        }
    }
    public enum TypeOfEnemy
    {
        EnemyType1,
        EnemyType2,
        EnemyType3,
        EnemyTypeSpecial1,
        EnemyTypeSpecial2,
        EnemyTypeSpecial3
    }
}