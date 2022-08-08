﻿using System.Collections;
using System.Collections.Generic;
using Boss;
using TMPro;
using UnityEngine;

namespace Stage
{
    public class StageScoreCounter: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI stageClearedTextUI;
        [SerializeField] private TextMeshProUGUI stagePointsTextUI;

        [SerializeField] private int stageNumber;
        [SerializeField] private int stagePoints;
        [SerializeField] private int noSpecialUsePoints;

        
        [SerializeField] private GameObject additionalPointsUIPrefab;
        [SerializeField] private Canvas canvas;

        private int _cumulativeValue;
        private bool _specialUsed = false;

        private void Start()
        {
            BossTimer.TranslateTimerData.AddListener(SetScoreForTimeRemaining);
            GlobalEvents.OnSpecialChange.AddListener((int x) => _specialUsed = true); // Rewrite it to OnSpecialUsed
        }

        private void SetScoreForTimeRemaining(Dictionary<int, int> phasesTime)
        {
            stageClearedTextUI.text = $"Stage {stageNumber} Cleared";
            stagePointsTextUI.text = $"{stagePoints}";

            stageClearedTextUI.gameObject.SetActive(true);
            stagePointsTextUI.gameObject.SetActive(true);
            StartCoroutine(SpawnAdditionalPointsText(phasesTime));
            StartCoroutine(AddAdditionalPoints(phasesTime));
        }

        private IEnumerator SpawnAdditionalPointsText(Dictionary<int, int> phasesTime)
        {
            List<GameObject> additionalPointsList = new List<GameObject>();
            
            yield return new WaitForSeconds(2);

            foreach (var item in phasesTime)
            {
                var newHolder = CreateAdditionalPointsHolder();
                newHolder.gameObject.GetComponent<TextMeshProUGUI>().text = $"+{item.Value}: {item.Key} sec";
                additionalPointsList.Add(newHolder);
                
                yield return new WaitForSeconds(1);
            }

            if (!_specialUsed)
            {
                var newHolder = CreateAdditionalPointsHolder();
                newHolder.gameObject.GetComponent<TextMeshProUGUI>().text = $"+{noSpecialUsePoints}: no special";
                additionalPointsList.Add(newHolder);
            }
            
            yield return new WaitForSeconds(2);

            foreach (var item in additionalPointsList)
            {
                Destroy(item);
            }
        }
        
        private IEnumerator AddAdditionalPoints(Dictionary<int, int> phasesTime)
        {
            yield return new WaitForSeconds(2);
            _cumulativeValue = stagePoints;
            
            yield return new WaitForSeconds(1);

            foreach (var item in phasesTime)
            {
                yield return new WaitForSeconds(1);
                _cumulativeValue += phasesTime[item.Key];
                stagePointsTextUI.text = $"{_cumulativeValue}";
            }

            if (!_specialUsed)
            {
                yield return new WaitForSeconds(1);
                _cumulativeValue += noSpecialUsePoints;
                stagePointsTextUI.text = $"{_cumulativeValue}";
            }
        }

        private GameObject CreateAdditionalPointsHolder()
        {
            var newHolder = Instantiate(additionalPointsUIPrefab, 
                additionalPointsUIPrefab.transform.position, Quaternion.identity);
            newHolder.transform.SetParent(canvas.transform, false);
            
            return newHolder;
        }
    }
}