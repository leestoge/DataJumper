using UnityEngine;

public class CameraPan : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
