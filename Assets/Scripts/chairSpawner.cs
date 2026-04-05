using UnityEngine;

public class chairSpawner : MonoBehaviour
{
  public GameObject chair;

  void Awake()
  {
  }

  public void spawn(float xMin, float xMax, float zMin, float zMax)
  {
    float randomX = Random.Range(xMin, xMax);
    float randomZ = Random.Range(zMin, zMax); ;
    Instantiate(chair, new Vector3(randomX, 4, randomZ), chair.transform.rotation);
  }
}
