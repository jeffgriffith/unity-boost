using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    Material m;

    AudioSource audioSource;

    [SerializeReference] float thrustRate = 1000f;
    [SerializeReference] float rotationRate = 20f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m = GetComponent<Renderer>().material;
        audioSource = GetComponent<AudioSource>();

        Debug.Log("got the rigid body at startup...");
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ApplyRotation();
    }

    void ProcessThrust() {

        bool thrusting = Input.GetKey(KeyCode.Space);
        AdjustSound(thrusting);

        if (thrusting) {
            bool fuelRemaining = AdjustForFuel();
            if (fuelRemaining)
                rb.AddRelativeForce(Vector3.up * thrustRate * Time.deltaTime);
        }
    }

    void AdjustSound(bool thrusting) {
        if (thrusting) {
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }
        } else if (audioSource.isPlaying) {
            audioSource.Pause();
        }
    }

    bool AdjustForFuel() {
        bool hasRemainingFuel = true;

        Vector3 scale = transform.localScale;
        float newY = scale.y;
        newY -= 0.001f;
        if (newY > 0.25f) {
            Vector3 newScale = new Vector3(scale.x, newY, scale.z);
            transform.localScale = newScale;
        } else {
            hasRemainingFuel = false;
            m.color = Color.red;
        }
        return hasRemainingFuel;
    }

    void ApplyRotation() {
        
        if (Input.GetKey(KeyCode.A)) {
            Rotate(rotationRate);
        } else if (Input.GetKey(KeyCode.D)) {
            Rotate(-rotationRate);
        }
    }

    void Rotate(float rotationRate) {
        rb.freezeRotation = true; // Freeze physics rotation so that we can do it manually.
        transform.Rotate(rotationRate * Vector3.forward * Time.deltaTime);
        rb.freezeRotation = false;
    }
}
