using UnityEngine;

public class ToggleTrail : MonoBehaviour
{
    public GameObject trailHolder; // empty game object holding trail
    private TrailRenderer theTrail;
    private bool toggle;

    // Start is called before the first frame update
    void Awake()
    {
        theTrail = trailHolder.GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            toggle = !toggle;
        }
        if (toggle)
        {
            theTrail.enabled = false;
            theTrail.Clear();
        }
        else
        {
            theTrail.enabled = true;
        }
    }

    public void ClearTrail()
    {
        theTrail.Clear();
    }
}
