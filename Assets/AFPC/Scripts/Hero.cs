using UnityEngine;
using AFPC;
using System;
using System.Collections.Generic;


/// <summary>
/// Example of setup AFPC with Lifecycle, Movement and Overview classes.
/// </summary>
public class Hero : MonoBehaviour {

    /* UI Reference */
    public HUD HUD;

    /* Lifecycle class. Damage, Heal, Death, Respawn... */
    public Lifecycle lifecycle;

    /* Movement class. Move, Jump, Run... */
    public Movement movement;

    /* Overview class. Look, Aim, Shake... */
    public Overview overview;

    public AudioClip fireSound;
    private AudioSource audioSource;
    public GameObject decalPrefab;
    public Transform checkpoint;

    public int maxDecals = 10;

    private List<GameObject> decals = new List<GameObject>();

    /* Optional assign the HUD */
    private void Awake () {
        if (HUD) {
            HUD.hero = this;
        }
    }

    /* Some classes need to initizlize */
    private void Start () {

        if(PlayerPrefs.GetInt("CheckPoint") == 1)
        {
            transform.position = checkpoint.position;
        }
        /* a few apllication settings for more smooth. This is Optional. */
        QualitySettings.vSyncCount = 0;
        Cursor.lockState = CursorLockMode.Locked;

        /* Initialize lifecycle and add Damage FX */
        lifecycle.Initialize();
        lifecycle.AssignDamageAction (DamageFX);

        /* Initialize movement and add camera shake when landing */
        movement.Initialize();
        movement.AssignLandingAction (()=> overview.Shake(0.5f));

        audioSource = GetComponent<AudioSource>();
    }

    private void Update () {

        /* Read player input before check availability */
        ReadInput();

        Shoot();

        /* Block controller when unavailable */
        if (!lifecycle.Availability()) return;

        /* Mouse look state */
        overview.Looking();

        /* Change camera FOV state */
        overview.Aiming();

        /* Shake camera state. Required "physical camera" mode on */
        overview.Shaking();

        /* Control the speed */
        movement.Running();

        /* Control the jumping, ground search... */
        movement.Jumping();

        /* Control the health and shield recovery */
        lifecycle.Runtime();
    }

    private void FixedUpdate () {

        /* Block controller when unavailable */
        if (!lifecycle.Availability()) return;

        /* Physical movement */
        movement.Accelerate();

        /* Physical rotation with camera */
        overview.RotateRigigbodyToLookDirection (movement.rb);
    }

    private void LateUpdate () {

        /* Block controller when unavailable */
        if (!lifecycle.Availability()) return;

        /* Camera following */
        overview.Follow (transform.position);
    }

    private void ReadInput () {
        if (Input.GetKeyDown (KeyCode.R)) lifecycle.Damage(50);
        if (Input.GetKeyDown (KeyCode.H)) lifecycle.Heal(50);
        if (Input.GetKeyDown (KeyCode.T)) lifecycle.Respawn();
        overview.lookingInputValues.x = Input.GetAxis("Mouse X");
        overview.lookingInputValues.y = Input.GetAxis("Mouse Y");
        overview.aimingInputValue = Input.GetMouseButton(1);
        movement.movementInputValues.x = Input.GetAxis("Horizontal");
        movement.movementInputValues.y = Input.GetAxis("Vertical");
        movement.jumpingInputValue = Input.GetButtonDown("Jump");
        movement.runningInputValue = Input.GetKey(KeyCode.LeftShift);
    }

    private void DamageFX () {
        if (HUD) HUD.DamageFX();
        overview.Shake(0.75f);
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            audioSource.PlayOneShot(fireSound);
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0)),out hit)){

                if(!hit.collider.isTrigger)
                {
              
                    if (hit.collider.transform.GetComponent<enemyAIDemo>())
                    {
                        hit.collider.transform.GetComponent<enemyAIDemo>().Hit(20); // Giving damage to the drone
                    }
                    GameObject newDecal = GameObject.Instantiate(decalPrefab, hit.point + hit.normal * 0.01f, Quaternion.FromToRotation(Vector3.forward, -hit.normal), hit.collider.transform);

                    // Add the new decal to the list
                    decals.Add(newDecal);

                    // Check if the number of decals exceeds the maximum limit
                    if (decals.Count > maxDecals)
                    {
                        // Destroy the oldest decal (the first one in the list)
                        Destroy(decals[0]);

                        // Remove the destroyed decal from the list
                        decals.RemoveAt(0);
                    }
                }
            }
        }
    }

    public void Hit(float damage)
    {
        Debug.Log("I got hit by the drone");
        lifecycle.Damage(damage);
    }


    private void OnTriggerStay(Collider other)
    {
       
        if (other.CompareTag("Lava"))
        {
            lifecycle.Damage(1);
        }
        if (other.CompareTag("CheckPoint"))
        {
            PlayerPrefs.SetInt("CheckPoint", 1);
            Debug.Log("CheckPoint Set");
        }

        if (other.CompareTag("Ammo"))
        {
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Health"))
        {
            Destroy(other.gameObject);
        }
    }
}
