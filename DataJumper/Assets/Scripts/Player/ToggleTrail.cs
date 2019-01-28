using UnityEngine;

public class ToggleTrail : MonoBehaviour
{
    public GameObject trailHolder; // empty game object holding trail
    private bool toggle;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            toggle = !toggle;
        }
        if (toggle)
        {
            trailHolder.SetActive(false);
        }
        else
        {
            trailHolder.SetActive(true);
        }
    }
}
