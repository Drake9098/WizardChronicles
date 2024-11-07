using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturntoMenu : MonoBehaviour
{
    [SerializeField] private GameObject obj;

    private void Update()
    {
        if (transform.position.y >= obj.transform.position.y)
        {
            SceneManager.LoadScene(0);
        }
    }
}
