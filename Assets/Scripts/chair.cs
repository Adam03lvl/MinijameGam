using UnityEngine;

public class chair : MonoBehaviour
{
  public float speed = 4;
  public Vector3 direction;

  private void Awake()
  {
    direction = transform.forward;
  }

  private void Update()
  {
    transform.Translate(speed * direction * Time.deltaTime);
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.CompareTag("harpoon"))
    {
      Destroy(gameObject);
    }
  }
}
