using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraEffect : MonoBehaviour
{
    [SerializeField] Material effectMat;
    [SerializeField] Camera mainCamera;

    [SerializeField] EntityManager entityManager;
    [SerializeField] CardManager cardManager;

    private Vector2 movePosition;
    private Vector3 dragOrigin;

    public float maxPosition;
    public float speed;
    public float mouseSpeed;
    public bool canMove = true;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        //Move_Camera_Position();
        //MoveObjectFunc();
        //ZoomTest();
    }

    private void OnDestroy()
    {
        SetGrayScale(false);
    }

    private void OnRenderImage(RenderTexture _src, RenderTexture _dest)
    {
        if (effectMat == null)
            return;

        Graphics.Blit(_src, _dest, effectMat);
    }

    public void SetGrayScale(bool isGrayscale)
    {
        effectMat.SetFloat("_GrayscaleAmount", isGrayscale ? 1 : 0);
        effectMat.SetFloat("_DarkAmount", isGrayscale ? 0.12f : 0);
    }

    void Move_Camera_Position()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0))
            return;

        if (entityManager.selectState || cardManager.selectState)
            return;

        Vector3 dir = (Input.mousePosition - dragOrigin).normalized;
        Vector3 vec = new Vector3(dir.x, dir.y, 0);

        transform.position -= vec * mouseSpeed * Time.deltaTime;

        dragOrigin = Input.mousePosition;
        //Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        //Vector3 move = new Vector3(-pos.x * mouseSpeed, -pos.y * mouseSpeed, 0);

        //Vector3 movePoint = transform.position + move;

        //transform.DOMove(movePoint, 0f);
        //MaxCameraPosition();
    }

    void MoveObjectFunc()
    {
        float keyH = Input.GetAxis("Horizontal");
        float keyV = Input.GetAxis("Vertical");
        keyH = keyH * speed * Time.deltaTime * 2;
        keyV = keyV * speed * Time.deltaTime * 2;
        transform.Translate(Vector3.right * keyH);
        transform.Translate(Vector3.up * keyV);
        MaxCameraPosition();
    }

    void MaxCameraPosition()
    {
        if (transform.position.x > maxPosition)
            transform.position = new Vector3(maxPosition, transform.position.y, -100);
        if (transform.position.x < -maxPosition)
            transform.position = new Vector3(-maxPosition, transform.position.y, -100);

        if (transform.position.y > maxPosition)
            transform.position = new Vector3(transform.position.x, maxPosition, -100);
        if (transform.position.y < -maxPosition)
            transform.position = new Vector3(transform.position.x, -maxPosition, -100);
    }

    void ZoomTest()
    {
        mainCamera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * speed;

        maxPosition = maxPosition + (Input.GetAxis("Mouse ScrollWheel") * speed * 2);

        if (maxPosition < 0)
            maxPosition = 0;


        if ((mainCamera.orthographicSize * 2) + maxPosition > 130f)
        {
            mainCamera.orthographicSize = 65f;
            maxPosition = 0;
        }
        if (mainCamera.orthographicSize <= 10)
        {
            mainCamera.orthographicSize = 10;
            maxPosition =  - (mainCamera.orthographicSize * 2);
        }
    }
}
