﻿using Character;
using Environment;
using Interfaces;
using ObjectPool;
using Unity.Mathematics;
using UnityEngine;
using Utils;

namespace Bullets
{
    public class Bullet : MonoBehaviour, IDestroyable
    {
        [SerializeField] private GameObject destroyEffect;
        [SerializeField] private BulletType bulletType;
        [SerializeField] protected float startSpeed = 0.1f;
        
        public Vector3 Direction { get; set; }
        public ObjectPoolTags objectTag;

        protected BulletType BulletType { get => bulletType; set => bulletType = value; }
        protected float StartSpeed { get; set; }

        /*private void Start()
        {
            GlobalEvents.OnClearBullets.AddListener(DestroySelf);
        }*/

        private void FixedUpdate()
        {
            Moving();
        }

        protected void Moving()
        {
            transform.Translate(Direction.normalized * startSpeed, Space.World);
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            collision.gameObject.HasComponent<PlayerBase>(component =>
            {
                if (component.IsInvulnerable) return;
                
                PlayerBase.TakeDamage(1);
                GlobalEvents.HealthChanged(component.Health);
                DestroySelf();
            });

            collision.gameObject.HasComponent<Border>(component =>
            {
                if (bulletType != BulletType.Chain)
                    gameObject.SetActive(false);
            });
        }

        public void SetColor(Color color)
        {
            GetComponent<SpriteRenderer>().color = color;
        }

        public void DestroySelf()
        {
            Instantiate(destroyEffect, transform.position, quaternion.identity);
            gameObject.SetActive(false);
        }
    }
}
