using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject explosionPrefab;
    GameObject player;
    public LayerMask whatIsSolid;
    private Transform MyTransform;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<Collider>());
        MyTransform = transform;
        Invoke("Explode", 3);
    }

    void Explode()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        GameObject explosion = Instantiate(explosionPrefab, MyTransform.position, MyTransform.rotation);
        GetComponent<AudioSource>().Play();
        Destroy(explosion, 1f);
        Destroy(gameObject, 1f);
    }

    void OnCollisionEnter(Collision other)
    {
        Explode();
    }
}
