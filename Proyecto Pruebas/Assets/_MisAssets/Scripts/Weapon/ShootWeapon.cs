using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootWeapon : MonoBehaviour
{
    [Tooltip("Pon el tiempo que tarda en disparar desde el último disparo")]
    public float cooldown = 0.1f;
    [Tooltip("Pon el objeto donde aparecen los disparos")]
    public Transform shotSpawn;
    [Tooltip("Pon la lista de los sonidos que pueden sonar cuando se dispara")]
    public AudioClip[] shotSounds;

    private AudioSource audioSource;
    private InputManager inputManager;
    private bool canShoot = true;   //variable que controla si se puede disparar

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        inputManager = GetComponentInParent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager.Shot())
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (!canShoot) return;
        canShoot = false;
        StartCoroutine(Cooldown());

        //sonido
        if(shotSounds.Length>0)
        {
            audioSource.PlayOneShot(shotSounds[Random.Range((int)0, (int)shotSounds.Length)]);
        }

        //disparo
        CastShot();


        //Onomatopeya



    }

    public abstract void CastShot();
    

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }
}
