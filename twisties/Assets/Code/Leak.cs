using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Leak : MonoBehaviour
{
    // used for the holding sound in soundmanager
    // different from isLeaking -- isLeaking == false when first spawned, but its also not being held so should play the sound
    private bool currentlyHeld;

    private bool hasBurst = false; // becomes active, then bursts, then leaks

    private bool isLeaking = false;
    private bool isActive = false;
    [SerializeField] private float holdTime; // how long the leak needs to be held before it is deactivated
    [SerializeField] private float currentHoldTime = 0; // how much "hold progress" the pipe has at the moment
    private float lastSecondHeld = 0; // the last second the player was holding the leak... used to add to currentHoldTime

    [SerializeField] private KeyCode keyBinding; // set in inspector for each object
    private SpriteRenderer spriteRenderer;
    private GameObject heldPopupPrefab;
    private GameObject warningPopupPrefab;
    private ParticleSystem leak;
    private GameObject heldPopup;
    private GameObject warningPopup;

    // sound
    private AudioSource audioSource;
    [SerializeField] private AudioClip spawnSound;
    [SerializeField] private AudioClip aboutToBreakSound;
    [SerializeField] private AudioClip[] fixedSounds;

    [SerializeField] private DecalProjector decalProjector;
    [SerializeField] private GameObject brokenModel;

    void Awake()
    {
        decalProjector = GetComponentInChildren<DecalProjector>();
        decalProjector.enabled = false;
        brokenModel.SetActive(false);

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        leak = GetComponentInChildren<ParticleSystem>();
        holdTime = Random.Range(3, 6); // need to be held between 3-6 seconds

        try
        {
            heldPopupPrefab = Resources.Load<GameObject>("HeldPopup");
            warningPopupPrefab = Resources.Load<GameObject>("WarningPopup");
        }
        catch
        {
            Debug.LogWarning("Prefab(s) ould not be found for " + gameObject.name);
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
    }

    void Update()
    {
        // is this leak being held?
        if (isActive) {

            Debug.Log("Leak is active!");

            if (Input.GetKeyDown(keyBinding)) // spawn the "held" popup on button down
            {
                leak.Stop();
                heldPopup = Instantiate(heldPopupPrefab, transform.position, Quaternion.identity);
            }
            if (Input.GetKey(keyBinding)) // do stuff while the leak is held
            {
                isLeaking = false;
                currentlyHeld = true;
                spriteRenderer.color = Color.white;
                leak.Stop();
                AddHoldTime();
                Debug.Log("Leak " + gameObject.name + " is being held");
            }

            // has the leak been let-go-of?
            if (Input.GetKeyUp(keyBinding))
            {
                Destroy(heldPopup);
                leak.Play();
                spriteRenderer.color = Color.red;
                // reset the fixed-ness of this leak
                currentHoldTime = 0;

                isLeaking = true;
                currentlyHeld = false;

                Debug.Log("Leak " + gameObject.name + " is active");
            }

            // if the leak is fixed destroy this leak and its popup
            if (currentHoldTime >= holdTime)
            {
                // play sfx
                int sfxIndex = Random.Range(0, 2);
                AudioClip clip = fixedSounds[sfxIndex];
                // hard coded name <3
                GameObject.Find("Game/Leak Manager").GetComponent<SoundManager>().PlayFixedSound(clip);

                isLeaking = false; // i dont think we need this but whatever
                currentlyHeld = false;
                if (heldPopup != null)
                {
                    Destroy(heldPopup);
                }
                spriteRenderer.color = Color.red;
                currentHoldTime = 0;
                leak.Stop();
                spriteRenderer.enabled = false;
                decalProjector.enabled = true;
                brokenModel.SetActive(false);
                isActive = false;
            }
        }

    }

    private void AddHoldTime()
    {
        if (Time.timeSinceLevelLoad >= lastSecondHeld + 1) // if its been a second
        {
            currentHoldTime += 1;
            lastSecondHeld = Time.timeSinceLevelLoad;
        }
    }

    public bool IsLeaking
    {
        get { return isLeaking; }
    }

    public bool IsActive
    {
        get { return isActive; }
    }

    public bool IsHeld
    {
        get { return currentlyHeld; }
    }

    public void SpawnLeak()
    {
        audioSource.clip = aboutToBreakSound;
        audioSource.Play();

        // temp
       //spriteRenderer.enabled = true;

        warningPopup = Instantiate(warningPopupPrefab, transform.position, Quaternion.identity);

        isActive = true;
        Invoke("StartLeak", 1);
    }

    private void StartLeak()
    {
        hasBurst = true;
        Destroy(warningPopup);
        brokenModel.SetActive(true);

        if (!Input.GetKey(keyBinding))
        {
            audioSource.clip = spawnSound;
            audioSource.Play();
            leak.Play();
            isLeaking = true;
        }

        if (decalProjector.enabled)
        {
            decalProjector.enabled = false;
        }

    }

}
