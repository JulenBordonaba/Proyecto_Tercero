using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootWeapon : MonoBehaviour
{
    [Tooltip("Pon el tiempo que tarda en disparar desde el último disparo")]
    public float cooldown = 0.1f;
    [Tooltip("Pon el tiempo que quieres que transcurra entre una onomatopeya y otra")]
    public float onomatopoeiaCooldown = 0.7f;
    [Tooltip("Pon el objeto donde aparecen los disparos")]
    public Transform shotSpawn;
    [Tooltip("Arrastra el sistema de particulas de la onomatopeya de disparar para instanciarlo")]
    public ParticleSystem prefabShotOnomatopoeia;
    [Tooltip("Pon la lista de los sonidos que pueden sonar cuando se dispara")]
    public AudioClip[] shotSounds;
    [Tooltip("Pon el audio source del que saldrá el sondo del disparo")]
    public AudioSource audioSource;

    private InputManager inputManager;
    private bool canShoot = true;   //variable que controla si se puede disparar
    private bool onomatipoeiaActive = true;   //variable que controla si se reproduce la onomatopeya

    private void Start()
    {
        inputManager = GetComponentInParent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.inPause) return;
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
        if (onomatipoeiaActive)
        {
            ParticleSystem onomatopoeia = Instantiate(prefabShotOnomatopoeia, shotSpawn);
            Destroy(onomatopoeia, onomatopoeia.main.duration);
            onomatipoeiaActive = false;
            StartCoroutine(OnomatopoeiaCooldown());
        }


    }

    public abstract void CastShot();
    

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }

    private IEnumerator OnomatopoeiaCooldown()
    {
        yield return new WaitForSeconds(onomatopoeiaCooldown);
        onomatipoeiaActive = true;
    }
}
