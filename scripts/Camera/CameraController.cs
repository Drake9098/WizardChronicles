using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{

    private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float speed;
    private float lookAhead;

    private void Start(){
        player = Player.instance.transform;
    }

    private void Update(){
        if (player.position.x < 310f)
            transform.position = new Vector3(player.position.x, player.position.y+1, transform.position.z);
        else transform.position = new Vector3(player.position.x, player.position.y + 2, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, aheadDistance * player.localScale.x, Time.deltaTime * speed);
    }

    public void AdjustCameraSize() {
        Camera cam = GetComponent<Camera>();
        float targetAspect = 16f / 9f;
        float windowAspect = Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        // Se l'aspetto attuale della finestra è più stretto del target, ridimensiona la camera
        if (scaleHeight < 1.0f)
        {
            cam.orthographicSize /= scaleHeight;
        }
    }
}
