﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    /* Defines behaviour of projectiles.
     */
    [RequireComponent (typeof (TrailRenderer))]
    public class Projectile : MonoBehaviour {

        public LayerMask collisionMask;
        public Color trailColor;
        public float damage;
        public float secsLifetime;

        [HideInInspector]
        public float speed;

        float overlapThreshold = 0.1f;

        void Start() {
            Destroy(gameObject, secsLifetime);

            CheckOverlapCollisions(overlapThreshold);

            GetComponent<TrailRenderer>().material.SetColor("_TintColor", trailColor);
        }

        void Update() {
            float moveDistance = Time.deltaTime * speed;

            CheckRayCollisions(moveDistance);

            transform.Translate(Vector3.forward * moveDistance);
        }

        void CheckOverlapCollisions(float radius) {
            Collider[] collisions = Physics.OverlapSphere(transform.position,
                    radius, collisionMask);
            if (collisions.Length > 0) {
                OnHitObject(collisions[0], transform.position);
            }
        }

        void CheckRayCollisions(float moveDistance) {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, moveDistance, collisionMask)) {
                OnHitObject(hit.collider, hit.point);
            }
        }

        void OnHitObject(Collider c, Vector3 hitPoint) {
            IDamageable damageableObject = c.GetComponent<IDamageable>();
            if (damageableObject != null) {
                damageableObject.TakeHit(damage, hitPoint, transform.forward);
            }
            GameObject.Destroy(gameObject);
        }

    }

}
