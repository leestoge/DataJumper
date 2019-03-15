using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject explosionPrefab;
    GameObject player;
    public LayerMask whatIsSolid;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<Collider>());

        Invoke("Explode", 3);
    }

    void Explode()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        GetComponent<AudioSource>().Play();
        Destroy(explosion, 1f);
        Destroy(gameObject, 1f);
    }

    void OnCollisionEnter(Collision other)
    {
        Explode();
    }
}
