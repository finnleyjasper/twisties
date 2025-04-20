using UnityEngine;

public class Leak : MonoBehaviour
{
    private bool isLeaking = false;
    private bool active = false;
    [SerializeField] private float holdTime; // how long the leak needs to be held before it is deactivated
    [SerializeField] private float currentHoldTime = 0; // how much "hold progress" the pipe has at the moment
    private float lastSecondHeld = 0; // the last second the player was holding the leak... used to add to currentHoldTime

    [SerializeField] private KeyCode keyBinding; // set in inspector for each object
    private SpriteRenderer spriteRenderer;
    private GameObject heldPopupPrefab;

    private GameObject heldPopup;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        holdTime = Random.Range(3, 6); // need to be held between 3-9 seconds

        try
        {
            heldPopupPrefab = Resources.Load<GameObject>("HeldPopup");
        }
        catch
        {
            Debug.LogWarning("Prefab 'HeldPopup' could not be found for " + gameObject.name);
        }
    }

    void Update()
    {
        // is this leak being held?
        if (active) {
            if (Input.GetKeyDown(keyBinding)) // spawn the "held" popup on button down
            {
                heldPopup = Instantiate(heldPopupPrefab, transform.position, Quaternion.identity);
            }
            if (Input.GetKey(keyBinding)) // do stuff while the leak is held
            {
                isLeaking = false;
                spriteRenderer.color = Color.white;

                AddHoldTime();
                Debug.Log("Leak " + gameObject.name + " is being held");
            }

            // has the leak been let-go-of?
            if (Input.GetKeyUp(keyBinding))
            {
                Destroy(heldPopup);

                // reset the fixed-ness of this leak
                currentHoldTime = 0;

                isLeaking = true;
                spriteRenderer.color = Color.red;
                Debug.Log("Leak " + gameObject.name + " is active");
            }

            // if the leak is fixed destroy this leak and its popup
            if (currentHoldTime >= holdTime)
            {
                isLeaking = false; // i dont think we need this but whatever
                if (heldPopup != null)
                {
                    Destroy(heldPopup);
                }
                spriteRenderer.color = Color.red;
                currentHoldTime = 0;
                spriteRenderer.enabled = false;
                active = false;
            }
        }
        
    }

    private void AddHoldTime()
    {
        if (Time.time >= lastSecondHeld + 1) // if its been a second
        {
            currentHoldTime += 1;
            lastSecondHeld = Time.time;
        }
    }

    public bool IsLeaking
    {
        get { return isLeaking; }
    }

    public void SpawnLeak()
    {
        spriteRenderer.enabled = true;
        isLeaking = true;
        active = true;
    }
    
}
