using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    private AudioSource explosion_audio;

    private void Start()
    {
        explosion_audio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.rigidbody && collision.rigidbody.name == "Ammo(Clone)")
        {
            // Destroy the ammo object and create explosion with sound.
            Destroy(collision.rigidbody.gameObject);
            explosion_audio.PlayDelayed(0.05f);
            GameObject explosion = Instantiate(GameManager.instance.blastPrefab, transform);
            Destroy(explosion, 2);
        }
    }
}
