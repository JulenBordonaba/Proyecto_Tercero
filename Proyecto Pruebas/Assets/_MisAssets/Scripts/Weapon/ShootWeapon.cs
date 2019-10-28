using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootWeapon : MonoBehaviour
{
    [Tooltip("Pon el tiempo que tarda en disparar desde el último disparo")]
    public float cooldown = 0.1f;
    [Tooltip("Pon el prefab del disparo")]
    public GameObject shotPrefab;
    [Tooltip("Pon la velocidad a la que se disparará")]
    public float shotForce = 100;
    [Tooltip("Pon la lista de los sonidos que pueden sonar cuando se dispara")]
    public AudioClip[] shotSounds;

    private AudioSource audioSource;
    private bool canShoot = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (InputManager.Shot())
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (!canShoot) return;
        canShoot = false;

        //sonido
        audioSource.PlayOneShot(shotSounds[Random.Range((int)0, (int)shotSounds.Length)]);

    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }
}
