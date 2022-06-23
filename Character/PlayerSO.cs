﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Character
{
    [CreateAssetMenu(fileName = "new PlayerConfig", menuName = "PlayerConfig")]
    public class PlayerSO: ScriptableObject
    {
        public float health;
        public float maxValue;
        public float maxLevel;
        public int level;
        public float special;
        public float specialCooldown;
        public float speed;
        public int exp = default;
        public int points = default;
        public GameObject bullet;
        public GameObject targetBullet;
        public float targetBulletFrequency;
        public KeyMap levelUpMap = new KeyMap();
    }
}