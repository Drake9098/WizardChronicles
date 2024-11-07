using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    
    private RectTransform rect;

    private void Awake() {
        rect = GetComponent<RectTransform>();
        if (options.Length > 0)
            rect.position = new Vector3(rect.position.x, options[0].position.y, rect.position.z);
    }

    private void Update() {
        //make arrow follow mouse hover
        for (int i = 0; i < options.Length; i++) {
            if (RectTransformUtility.RectangleContainsScreenPoint(options[i], Input.mousePosition)) {
                rect.position = new Vector3(rect.position.x, options[i].position.y, rect.position.z);
            }
        }
    }
}
