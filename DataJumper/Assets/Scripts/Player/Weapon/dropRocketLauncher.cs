using UnityEngine;

public class dropRocketLauncher : MonoBehaviour
{
    public GameObject instantiatedProp;
    public GameObject actualRL;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (actualRL.activeInHierarchy)
            {
                actualRL.SetActive(false);
                FindObjectOfType<AudioManager>().Play("Throw");
                Instantiate(instantiatedProp, actualRL.transform.position, Quaternion.identity);
                instantiatedProp.GetComponent<Rigidbody>().AddForce(actualRL.transform.forward * 900f);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Rocket Launcher is already not active.");
            }
        }
    }
}
