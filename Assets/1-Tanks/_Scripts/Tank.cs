using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks
{
    public class Tank : MonoBehaviour
    {
        public float speed = 20f;                    // Travel 1 distance over time
        public float fuelDuration = 2f;              // Time allowed for movement
        [Header("Bullets")]
        public float bulletSpeed = 10f;              // Speed to send bullet traveling
        public GameObject bulletPrefab;              // Original copy of bullet
        [Header("Explosion")]
        public float explosionForce = 5f;            // Force of explosion
        [Header("Fuel UI")]
        public Transform fuelSliderParent;           // The parent of the UI Canvas
        public GameObject fuelSliderPrefab;          // The slider prefab to spawn to UI Canvas
        public Vector3 offset = new Vector3(0, 2f, 0);  // Position of the fuel slider
        [Header("References")]
        public Transform gun;                        // Reference to gun for rotating the turret
        public Transform bulletSpawnPoint;                 // Transform point to spawn the bullet
        [Header("Components")]
        public Rigidbody2D rigid;                    // Reference to Rigidbody component
        public Health health;                        // Reference to Health component

        private float fuelTimer = 0f;                // Elapsed time of movement (fuel)
        private Slider fuelSlider;                   // Reference to newly spawned slider (UI)
        private bool isPlaying = false;              // Is this Tank currently playing?
        public bool IsPlaying
        {
            get { return isPlaying; }
            set { isPlaying = value;
                // if the bool "isPlaying" gets set to true
            if(isPlaying)
                {
                    // Reset the tank's values
                    Reset();
                }
            }
        }

        #region Unity Functions
        private void Start()
        {
            // Reset timer to move duration (in seconds)
            fuelTimer = fuelDuration;
            // Spawn the UI under canvas
            SpawnUI();
        }

        private void Update()
        {
            if(isPlaying)
            {
                 // Update UI's position
                //UpdateUI();

                // Handle movement for the Tank
                Move();
                RotateGunToMouse();
                if(Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                    // no longer playing (can't shoot bullets intil 1st one hits)
                    isPlaying = false;
                }
            }
            UpdateUI();
        }

        private void OnDestroy()
        {
            if(fuelSlider)
            {
                Destroy(fuelSlider.gameObject);
            }

            // Tell the GameManager that this tank has been destroyed
            if(GameManager.Instance)
            {
                // Switch Tanks when the bullet is destroyed
                GameManager.Instance.NextTank();
            }
           
        }

        private void Reset()
        {
            // Reset fuel
            fuelTimer = fuelDuration;
        }

        #endregion

        #region Custom Funcitons
        private void SpawnUI()
        {
            //Spawn the fuelslider intp the canvas
            GameObject clone = Instantiate(fuelSliderPrefab, fuelSliderParent);
            // Rename the slider
            clone.name = name + "_Fuel";
            // Store Slider component from clone
            fuelSlider = clone.GetComponent<Slider>();

        }
        private void UpdateUI()
        {
            // Convert to screen position
            Vector3 uiPos = Camera.main.WorldToScreenPoint(transform.position + offset);
            // Update slider position
            fuelSlider.transform.position = uiPos;
            // Update the value of the slider
            fuelSlider.value = fuelTimer / fuelDuration;
   
        }
        private void Move()
        {
            // Move timer hasn't reached zero yet?
            if (fuelTimer >= 0f)
            {
                // Get horizontal movement i.e, "W" and "D" keys
                float inputH = Input.GetAxis("Horizontal");
                // If the player is pressing one of those keys
                if (inputH != 0)
                {
                    // Count down the timer
                    fuelTimer -= Time.deltaTime;
                }

                // Move the rigidbody
                rigid.velocity = new Vector2(inputH * speed, rigid.velocity.y);
           
            }
        }
        private void Shoot()
        {
            // instantiate new bullet
            GameObject clone = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            // Get rigidbody from bullet
            Rigidbody2D rigid = clone.GetComponent<Rigidbody2D>();
            // add rigidbody force in the direction of the gun
            rigid.AddForce(gun.right * bulletSpeed, ForceMode2D.Impulse);
        }
        private void RotateGunToMouse()
        {
            // Convertr mouse screen to world position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Calculate direction (target - current)
            Vector3 direction = mousePos - gun.position;
            // Calculate gun angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // apply new rotation to turret
            gun.eulerAngles = new Vector3(0, 0, angle);
        }
        private void Explode()
        {
            // Search for all components in children that have the "breakable script attached
            Breakable[] parts = GetComponentsInChildren<Breakable>();
            // Loop through all breakable parts
            foreach (var part in parts)
            {
                // Detatch from parent
                part.transform.SetParent(null);
                // Check if it doesn't have a rigidBody2D
                if(part.transform.GetComponent<Rigidbody2D>() == null)
                {
                    // Add a RigidBody2D
                    Rigidbody2D partRigid = part.gameObject.AddComponent<Rigidbody2D>();
                    Vector3 force = (part.transform.position - health.lastHitPoint).normalized;
                    partRigid.AddForce(force * explosionForce, ForceMode2D.Impulse);
                }
                // Check if it doesn't have a Colider2D
                if(part.transform.GetComponent<Collider2D>() == null)
                {
                    // Add a PolygonCollider2D
                    part.gameObject.AddComponent<PolygonCollider2D>();
                }
            }
        }

        public void Died()
        {
            // plays death animation
            // Explodes
            Explode();
            Destroy(gameObject);
        }
        #endregion
    }
}
