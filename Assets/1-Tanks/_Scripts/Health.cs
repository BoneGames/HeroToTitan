using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using UnityEngine.UI;

namespace Tanks
{
    public class Health : MonoBehaviour
    {
        public float maxHealth = 100f;
        public UnityEvent onDeath;
        [Header("UI")]
        public Transform healthSliderParent;
        public GameObject healthSliderPrefab;
        public Vector3 offset = new Vector3(0, 2f, 0);

        [HideInInspector] public Vector3 lastHitPoint;

        private float currentHealth = 100f;
        private Slider healthSlider;

        #region Unity Events
        private void Start()
        {
            SpawnUI();
        }
        private void Update()
        {
            UpdateUI();
        }
        private void OnDestroy()
        {
            // If health slider exists
            if (healthSlider)
            {
                // Destroy HealthSlider UI
                Destroy(healthSlider.gameObject);
            }
        }
        #endregion

        #region Custom Functions
        private void SpawnUI()
        {
            // Create instance of UI and attach as child to UI parent
            GameObject clone = Instantiate(healthSliderPrefab, healthSliderParent);
            // name health, e.g. "Tank_1_Health"
            clone.name = name + "_Health";
            // Get Slider component from clone
            healthSlider = clone.GetComponent<Slider>();
        }
        private void UpdateUI()
        {
            // Convert world position to screen
            Vector3 uiPos = Camera.main.WorldToScreenPoint(transform.position + offset);
            // Update slider position
            healthSlider.transform.position = uiPos;
            // Convert hellath to a 0-1 value and update slider value
            healthSlider.value = currentHealth / maxHealth;
        }
        private void Dead()
        {
            // Invoke all events
            onDeath.Invoke();
        }
        public void TakeDamage(float damage, Vector2 hitFrom)
        {
            lastHitPoint = hitFrom; // Record last hit position
            currentHealth -= damage; // Reducing health with damage
            // If health is depleted
            if (currentHealth <= 0)
            {
                // Ya dead
                Dead();
            }
        }
        // test function
        public void TestDies()
        {
            TakeDamage(1000000, transform.position);
        }
        #endregion

    }
}
