using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField]
    private float rotationCS = 100;

    private enum State {
        TRANSCENDING,
        ALIVE,
        DYING
    }

    [SerializeField]
    private float flyCS = 100;
    private Rigidbody rocket;
    private AudioSource audioSource;
    private State gameState = State.ALIVE;

    // Start is called before the first frame update
    void Start() {
        rocket = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (gameState == State.ALIVE) {
            ProcessFly();
            ProcessRotation();
        } else if (audioSource.isPlaying) {
            audioSource.Stop();
        }
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
        if (gameState != State.ALIVE) {
            return;
        }

        switch (other.gameObject.tag) {
            case "Friendly" : 
                print("ok");
                break;
            case "Finish" : 
                gameState = State.TRANSCENDING;
                Invoke("LoadNextScene", 1f);
                break;
            default:
                gameState = State.DYING;
                Invoke("LoadFirstScene", 1f);
                break;
        }
    }

    private void LoadNextScene() {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
        gameState = State.ALIVE;
    }

    private void LoadFirstScene() {
        SceneManager.LoadScene(0);
        gameState = State.ALIVE;
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
