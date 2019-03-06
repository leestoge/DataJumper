using UnityEngine;

public class FireRocket : MonoBehaviour
{
    public GameObject rocketPrefab;
    public Transform shootPoint;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FindObjectOfType<AudioManager>().Play("RocketFire");
            Quaternion rocketRot = Quaternion.Euler(shootPoint.rotation.eulerAngles.x + 90, shootPoint.rotation.eulerAngles.y, shootPoint.rotation.eulerAngles.z);
            GameObject rocket = Instantiate(rocketPrefab, shootPoint.position, rocketRot);
            Rigidbody rocketRb = rocket.GetComponent<Rigidbody>();
            rocketRb.velocity = shootPoint.forward * 25f;
        }
    }
}
