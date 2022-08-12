using System;
using System.Collections;
using System.Collections.Generic;
using Bullets;
using Spells;
using Unity.Mathematics;
using UnityEngine;
using Utils;

namespace Enemy
{
    public class CommonEnemy : EnemyBase, IShoot, IDestroyable
    {
        public EnemySO enemySo;
        private float Cooldown => enemySo.cooldown;
        private GameObject Bullet => enemySo.bullet;
        private int BulletCount => enemySo.counter;
        private float Speed => enemySo.speed;
        private Spells Spell => enemySo.spell;
        private MoveSet MoveSet => enemySo.moveSet;
        
        private float _innerTimer;
        
        private void Awake()
        {
            CurrentHp = enemySo.maxHp;
            _innerTimer = Cooldown;
            
            OnTakingDamageEvent.AddListener(OnTakingDamage);
        }

        private void Start()
        {
            switch (MoveSet)
            {
                case MoveSet.MoveAround:
                    StartCoroutine(MoveAroundRoutine(targetPosition, enemySo.radius, Speed, enemySo.angularSpeed));
                    break;
                case MoveSet.ToPosition:
                    StartCoroutine(MovementToPosition(targetPosition, Speed));
                    break;
                case MoveSet.ToPoint:
                    //
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void FixedUpdate()
        {
            CheckHealth(enemySo.lootSettings, enemySo.destroyEffect);

            if (_innerTimer > 0)
            {
                _innerTimer -= Time.deltaTime;
            }
            else
            {
                switch (Spell)
                {
                    case Spells.Circle:
                        CommonSpells.CircleBulletSpawn(new SpellSettingsWithCount(Bullet, transform.position, 1, BulletCount));
                        break;
                    case Spells.RandomShooting:
                        CommonSpells.RandomShooting(new CommonSpellSettings(Bullet, transform.position, 1));
                        break;
                    case Spells.DirectTarget:
                        CommonSpells.TargetPositionShooting(new CommonSpellSettingsWithTarget(Bullet, transform.position,
                            1, UtilsBase.GetDirection(UtilsBase.GetNewPlayerPosition(), transform.position), 0.01f));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                _innerTimer = Cooldown;
            }
        }
        
        public IEnumerator Shoot()
        {
            /*if (shootingIsAvailable)
            {
                _animator.SetBool(IsCasting, false);

                yield return new WaitForSeconds(Cooldown);

                _animator.SetBool(IsCasting, true);
                
                for (var i = 0; i < BulletCount; i++)
                {
                    yield return new WaitForSeconds(0.1f);
                    
                    CommonSpells.TargetPositionShooting(new CommonSpellSettingsWithTarget(Bullet, transform.position,
                        1,GetDirection(GetNewTargetPosition(), transform.position)));
                }

                yield return Shoot();
            }*/
            
            yield return new WaitForSeconds(Cooldown);
        }

        public void DestroySelf()
        {
            Instantiate(enemySo.destroyEffect, transform.position, quaternion.identity);
            Destroy(gameObject);
        }
    }
}
