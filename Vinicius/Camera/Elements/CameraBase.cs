﻿using Extensions;
using UnityEngine;
using Axes = Metadatas.Input.Axes;

namespace Elements
{

    public class CameraBase : MonoBehaviour
    {
        [Tooltip("GameObject that will serve as a point of view for the camera." +
            " It should be an object within the character the camera is looking at.")]
        [HideInInspector] public Transform target;

        [Header("Parameters")]
        [SerializeField] private float movementSpeed = 120f;
        [SerializeField] private float maxClampAngle = 60f;
        [SerializeField] private float minClampAngle = 30f;
        [SerializeField] private float baseInputSensitivity = 600f;
        [SerializeField] private float inputSensitivity;
        private const float RotZ = 0f;
        private float rotY;
        private float rotX;

        private void Awake()
        {
            // Init();
            SetupCursor();
            SetRotation();
            // SetPosition();
        }

        private void Update()
        {
            inputSensitivity = baseInputSensitivity * OptionsManager.sensiblity;

            SetRotation();
        }

        private void Init()
        {
            var rot = transform.localRotation.eulerAngles;
            rotY = rot.y;
            rotX = rot.x;
        }

        private void SetupCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void SetRotation()
        {
            UpdateRotY();
            UpdateRotX();

            // Quaternion rot = Quaternion.Euler(rotX, rotY, RotZ);
            
            transform.eulerAngles = new Vector3(rotX, rotY, RotZ);
        }

        private void UpdateRotY()
        {
            var mouseX = Axes.MouseX.GetAxisRaw();

            var finalInputX = mouseX;

            rotY += finalInputX * inputSensitivity * Time.deltaTime;
        }

        private void UpdateRotX()
        {
            var mouseY = Axes.MouseY.GetAxisRaw();

            var finalInputZ = mouseY;

            rotX += finalInputZ * inputSensitivity * Time.deltaTime;
            rotX = Mathf.Clamp(rotX, minClampAngle, maxClampAngle);
        }

        private void SetPosition() =>
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                GetMaxDistanceDelta()
            );

        private float GetMaxDistanceDelta() => movementSpeed * Time.deltaTime;

    }

}