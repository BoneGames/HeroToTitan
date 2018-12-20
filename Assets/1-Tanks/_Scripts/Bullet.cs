using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

namespace Tanks
{
    public class Bullet : MonoBehaviour
    {
        public int damage = 50;             // Damage to deal to health scripts
        public float explosionRadius = 5f;  // Radius of explosion
        public GameObject explosionPrefab;  // Original explosion prefab
        private Rigidbody2D rigid;          // Reference to Rigidbody2D

        // Use this for initialization
        void Start()
        {
            // Get reference to Rigidbody2D
            rigid = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            RotateToVelocity();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);

            int radius = (int)explosionRadius;
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    Vector3 hitPoint = transform.position + new Vector3(x, y) * .5f;
                    float distance = Vector3.Distance(transform.position, hitPoint);
                    if (distance <= explosionRadius * .5f)
                    {
                        Gizmos.DrawCube(hitPoint, Vector3.one * .1f);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            // If GameManager exists
            if (GameManager.Instance)
            {
                // Switch tanks when the bullet is destroyed
                GameManager.Instance.NextTank();
            }
        }

        void RotateToVelocity()
        {
            // Get Velocity
            Vector3 vel = rigid.velocity;
            // Get Angle from Velocity
            float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
            // Rotate bullet in that angle
            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        // Upon impact, detect health scripts within certain area
        void Explode()
        {
            // Spawn an explosion (particle system)
            // Play explosion audio 

            // Get all hit objects within area
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            // Search through list (Loop through)
            foreach (var hit in hits)
            {
                // Try getting Health script from thing we hit
                Health health = hit.GetComponent<Health>();
                // If health is attached to thing we hit
                if (health)
                {
                    // Create a tempDamage variable
                    // Calculate distance between ourselves and the hit thing
                    // Subtract tempDamage with distance
                    // Deal damage to that thing! - Note(Manny): Come back to this later
                    health.TakeDamage(damage, transform.position);
                }

                // Try getting Tilemap component from the thing we hit
                Tilemap tilemap = hit.GetComponent<Tilemap>();
                // If tilemap is attached to the thing we hit
                if (tilemap)
                {
                    // Destroy tile at it's position
                    //DestroyTiles(tilemap, transform.position, (int)explosionRadius);
                }
            }
        }

        void DestroyTiles(Tilemap tilemap, Vector3 point, int radius)
        {
            // If tilemap exists
            if (tilemap)
            {
                // Loop through -radius to radius on x and y
                for (int x = -radius; x <= radius; x++)
                {
                    for (int y = -radius; y <= radius; y++)
                    {
                        // Create a vector that's offset from the point with current radius
                        Vector3 hitPoint = point + new Vector3(x, y) * .5f;
                        // Get distance from bullet to hit point
                        float distance = Vector3.Distance(transform.position, hitPoint);
                        // Check if distance is less than explosion radius
                        if (distance <= explosionRadius * .5f)
                        {
                            // Convert point to tilemap's cell
                            Vector3Int hitPos = tilemap.WorldToCell(hitPoint);
                            // If there is a tile at that position
                            if (tilemap.GetTile(hitPos) != null)
                            {
                                // Remove that tile
                                tilemap.SetTile(hitPos, null);
                            }
                        }
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            // Detect hit objects
            Explode();

            // Destroy ourselves
            Destroy(gameObject);
        }
    }

}