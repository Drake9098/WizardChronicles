using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCredits : MonoBehaviour
{
    [SerializeField] private GameObject[] credits;

    private void Update() {
        foreach (GameObject credit in credits) {
            if (Input.GetKey(KeyCode.Space)) {
                credit.transform.position += new Vector3(0, 500f * Time.deltaTime, 0);
            }
            else {
                credit.transform.position += new Vector3(0, 120f * Time.deltaTime, 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(0);
        }
    }
}
