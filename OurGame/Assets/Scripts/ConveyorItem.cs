using UnityEngine;

public class ConveyorItem : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Vector3 moveDir = Vector3.down;

    private void Update()
    {
        transform.position += moveDir * speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 newDir)
    {
        moveDir = newDir;
    }
}
