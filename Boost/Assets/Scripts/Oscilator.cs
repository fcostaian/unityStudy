using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscilator : MonoBehaviour { 
    [SerializeField] private Vector3 moviment;

    [SerializeField] [Range(0,15)] private float period = 2f;

    Vector3 startPosition;

    void Start() {
        startPosition = transform.position;    
    }

    void Update() {
        if (period <= Mathf.Epsilon) {
            return;
        }

        float cycles = Time.time / period;
        float sineWave = Mathf.Sin(cycles * 2 * Mathf.PI);
    
        Vector3 offset = moviment * (sineWave / period + 0.5f);
        transform.position = startPosition + offset;
    }
}