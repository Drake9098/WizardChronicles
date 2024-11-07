using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeholderSpell : MonoBehaviour
{

    private void Start() {
        gameObject.SetActive(false);
    }
    public void Activate(){
        gameObject.SetActive(true);
    }

    private void Deactivate() {
        gameObject.SetActive(false);
    }
}
