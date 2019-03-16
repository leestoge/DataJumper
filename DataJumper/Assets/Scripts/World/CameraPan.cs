using UnityEngine;

public class CameraPan : MonoBehaviour
{
    public float speed;
    private Transform MyTransform;

    void Awake()
    {
        MyTransform = transform;
    }
    // Update is called once per frame
    void Update()
    {
        MyTransform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
