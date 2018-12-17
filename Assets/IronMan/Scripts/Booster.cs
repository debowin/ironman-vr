using System;
using UnityEngine;

public class Booster : MonoBehaviour {

    private Rigidbody rigidBody;
    private SteamVR_TrackedController controller;
    private ParticleSystem.EmissionModule fireEmission;
    private ParticleSystem.EmissionModule smokeEmission;
    private SteamVR_LaserPointer laserPointer;
    public AudioSource shootAudio;
    public AudioSource boostAudio;
    public float boostMultiplier;
    public float fireExhaustRate = 30f;
    public float smokeExhaustRate = 10f;
    public GameObject Fire;
    public GameObject Smoke;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponentInParent<Rigidbody>();
        laserPointer = GetComponent<SteamVR_LaserPointer>();
        ParticleSystem fireParticleSystem = Fire.GetComponent<ParticleSystem>();
        fireEmission = fireParticleSystem.emission;
        ParticleSystem smokeParticleSystem = Smoke.GetComponent<ParticleSystem>();
        smokeEmission = smokeParticleSystem.emission;
    }
	
	// Update is called once per frame, but FixedUpdate called once per Physics step, time between
    // calls more consistent than Update, hence a better place to add Physics calculations.
	void FixedUpdate () {
        // how much has the trigger been pressed: [0, 1]
        var boost = controller.controllerState.rAxis1.x;

        // ignore tiny presses
        if (boost > 0.1f) {
            var force = boost * transform.forward * boostMultiplier;

            fireEmission.rateOverTime = boost * fireExhaustRate;
            smokeEmission.rateOverTime = boost * smokeExhaustRate;

            rigidBody.AddForce(force);
            if (!boostAudio.isPlaying) {
                SteamVR_Controller.Input((int)controller.controllerIndex).TriggerHapticPulse(3999, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
                boostAudio.Play();
            }
        } else
        {
            fireEmission.rateOverTime = 0f;
            smokeEmission.rateOverTime = 0f;

            boostAudio.Stop();
        }

	}

    private void OnEnable()
    {
        controller = GetComponent<SteamVR_TrackedController>();
        controller.PadClicked += HandlePadClicked;
        controller.Gripped += HandleGripped;
        controller.PadTouched += HandlePadTouched;
        controller.PadUntouched += HandlePadUntouched;
    }

    private void HandlePadUntouched(object sender, ClickedEventArgs e)
    {
        laserPointer.active = false;
    }

    private void HandlePadTouched(object sender, ClickedEventArgs e)
    {
        laserPointer.active = true;
    }

    private void HandleGripped(object sender, ClickedEventArgs e)
    {
        rigidBody.transform.position += transform.forward;
    }

    private void HandlePadClicked(object sender, ClickedEventArgs e)
    {
        GameObject ammo = Instantiate(GameManager.instance.ammoPrefab, transform.position + 0.5f * transform.forward, Quaternion.LookRotation(transform.up, transform.forward));
        GameObject ammoExhaust = Instantiate(GameManager.instance.exhaustPrefab, ammo.transform);
        ammo.GetComponent<Rigidbody>().AddForce(transform.forward * 200f, ForceMode.Impulse);
        Destroy(ammoExhaust, 2);
        Destroy(ammo, 5);
        shootAudio.Play();
        SteamVR_Controller.Input((int)controller.controllerIndex).TriggerHapticPulse(1000, Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
    }

    private void OnDisable()
    {
        controller.PadClicked -= HandlePadClicked;
        controller.Gripped -= HandleGripped;
        controller.PadTouched -= HandlePadTouched;
        controller.PadUntouched -= HandlePadUntouched;
    }
}
