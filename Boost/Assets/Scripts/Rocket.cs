using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] private float rotationCS;
    [SerializeField] private float flyCS = 600;
    [SerializeField] private AudioClip flyingAudio;
    [SerializeField] private AudioClip winningAudio;
    [SerializeField] private AudioClip losingAudio;
    
    [SerializeField] private ParticleSystem flyingParticle;
    [SerializeField] private ParticleSystem winningParticle;
    [SerializeField] private ParticleSystem losingParticle;

    [SerializeField] private float levelLoadDelay = 2f;

    private enum State {
        TRANSCENDING,
        ALIVE,
        DYING
    }
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
                print("Win");
                winningParticle.Play();
                PlayAudio(winningAudio);
                Invoke("LoadNextScene", levelLoadDelay);
                break;
            default:
                gameState = State.DYING;
                print("Lose");
                losingParticle.Play();
                PlayAudio(losingAudio);
                Invoke("LoadFirstScene", levelLoadDelay);
                break;
        }
    }

    private void LoadNextScene() {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
        gameState = State.ALIVE;
        winningParticle.Stop();
    }

    private void LoadFirstScene() {
        SceneManager.LoadScene(0);
        gameState = State.ALIVE;
        losingParticle.Stop();
    }

    private void ProcessFly() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            flyingParticle.Play();
        }
        if (Input.GetKey(KeyCode.Space)) {
            rocket.AddRelativeForce(Vector3.up * flyCS * Time.deltaTime);
            PlayAudio(flyingAudio);
        } else if (Input.GetKeyUp(KeyCode.Space)){
            flyingParticle.Stop();
            StopAudio(flyingAudio);
        }
    }

    private void PlayAudio (AudioClip audio) {
        if (!audioSource.isPlaying || audio != flyingAudio) {
            audioSource.Stop();
            audioSource.PlayOneShot(audio);
        }
    }

    private void StopAudio (AudioClip audio) {
        if (audioSource.isPlaying && audio == flyingAudio) {
            audioSource.Stop();
        }
    }
}
