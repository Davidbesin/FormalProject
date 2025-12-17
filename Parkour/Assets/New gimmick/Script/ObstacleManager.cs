using UnityEngine;
using System.Collections;

public class ObstacleManager : MonoBehaviour
{
    private enum ObstacleType
    {
        Jump,
        Roll,
        Blocked,
        Free
    }

    float middle = 0;
    float left = -2f;
    float right = 2f;

    private ObstacleType[] obstacleRandomiser = new ObstacleType[3];

    // Separate pools for each obstacle type
    [SerializeField] private GameObject[] jumpObstacles;
    [SerializeField] private GameObject[] rollObstacles;
    [SerializeField] private GameObject[] blockedObstacles;
    [SerializeField] private GameObject[] freeObstacles;

    // Z‑spacing
    private float nextSpawnZ = 0f;
    public float zSpacing = 15f;


    void Start()
    {
        DisablePool(jumpObstacles);
        DisablePool(rollObstacles);
        DisablePool(blockedObstacles);
        DisablePool(freeObstacles);

        StartCoroutine(RandomizeObstaclesRoutine());
    }

    private void DisablePool(GameObject[] pool)
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (pool[i] != null)
                pool[i].SetActive(false);
        }
    }

    IEnumerator RandomizeObstaclesRoutine()
    {
        while (true)
        {
            for (int i = 0; i < obstacleRandomiser.Length; i++)
            {
                obstacleRandomiser[i] = GetRandomObstacle();
            }

            SpawnObstacles();

            Debug.Log(obstacleRandomiser[0] + ", " + obstacleRandomiser[1] + ", " + obstacleRandomiser[2]);

            yield return new WaitForSeconds(20f);
        }
    }

    private void SpawnObstacles()
    {
        for (int lane = 0; lane < obstacleRandomiser.Length; lane++)
        {
            ObstacleType type = obstacleRandomiser[lane];
            GameObject[] pool = GetPool(type);

            if (pool == null || pool.Length == 0)
                continue;

            // Find an inactive object
            GameObject obj = null;
            for (int i = 0; i < pool.Length; i++)
            {
                if (!pool[i].activeInHierarchy)
                {
                    obj = pool[i];
                    break;
                }
            }

            if (obj == null)
                continue; // Pool exhausted

            float laneX;
            if (lane == 0)
                laneX = left;
            else if (lane == 1)
                laneX = middle;
            else
                laneX = right;

            Vector3 pos = obj.transform.position;
            obj.transform.position = new Vector3(laneX, pos.y, nextSpawnZ);

            obj.SetActive(true);
        }

        // Move Z forward for next wave
        nextSpawnZ += zSpacing;
    }

    private GameObject[] GetPool(ObstacleType type)
    {
        switch (type)
        {
            case ObstacleType.Jump: return jumpObstacles;
            case ObstacleType.Roll: return rollObstacles;
            case ObstacleType.Blocked: return blockedObstacles;
            case ObstacleType.Free: return freeObstacles;
        }
        return null;
    }

    private ObstacleType GetRandomObstacle()
    {
        ObstacleType[] values = (ObstacleType[])System.Enum.GetValues(typeof(ObstacleType));
        int index = Random.Range(0, values.Length);
        return values[index];
    }
}
