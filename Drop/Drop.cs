﻿using System;
using Character;
using Enviroment;
using UnityEngine;
using Utils;

namespace Drop
{
    public class Drop: MonoBehaviour
    {
        public DropSO dropSo;

        private int Value => dropSo.value;
        private DropType DropType => dropSo.dropType;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            other.gameObject.HasComponent<PlayerModel>(component =>
            {
                PlayerModel.GetDrop(DropType, Value);
                Destroy(gameObject);
            });
            
            other.gameObject.HasComponent<Border>(component => Destroy(gameObject));
        }
    }
}