using UnityEngine;

public class Player : MonoBehaviour {

    private Rigidbody rigidBody;
    public float maxVelocity = 10f;

	// Use this for initialization
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
	}

    // Update is called once per frame, but FixedUpdate called once per Physics step, time between
    // calls more consistent than Update, hence a better place to add Physics calculations.
    private void FixedUpdate()
    {
        // limit maximum velocity
        if(rigidBody.velocity.magnitude > maxVelocity)
        {
            rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, maxVelocity);
        }
        
        // prevent player from passing through the ground
        if (transform.position.y < 0.1f)
        {
            rigidBody.MovePosition(new Vector3(transform.position.x, 0.1f, transform.position.z));
        }
    }
}
