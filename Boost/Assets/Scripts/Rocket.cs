using UnityEngine;

public class Rocket : MonoBehaviour {

    [SerializeField]
    private float rotationCS = 100;

    [SerializeField]
    private float flyCS = 100;
    private Rigidbody rocket;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start() {
        rocket = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        ProcessFly();
        ProcessRotation();
    }

    private void ProcessRotation() {

        rocket.freezeRotation = true;

        float rotationSpeed = rotationCS * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }

        rocket.freezeRotation = false;
    }

    void OnCollisionEnter(Collision other) {
        switch (other.gameObject.tag) {
            case "Friendly" : 
                print("ok");
                break;
            default:
                print("dead");
                break;
        }
    }

    private void ProcessFly() {
        float flySpeed = flyCS * Time.deltaTime * 10;
        if (Input.GetKey(KeyCode.Space)) {
            rocket.AddRelativeForce(Vector3.up * flySpeed);

            if (!audioSource.isPlaying) {
                audioSource.Play();
            }
        } else {
            audioSource.Stop();
        }
    }
}
