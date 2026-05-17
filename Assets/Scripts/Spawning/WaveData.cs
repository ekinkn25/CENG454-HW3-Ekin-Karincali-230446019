using System;
using UnityEngine;

namespace CoreBreach.Spawning
{
    //this holds every wave's information
    [Serializable]
    public class WaveData
    {
        public enum StrategyType { Direct, Zigzag, Mixed }

        [Header("Dalga Bilgisi")]
        public int   waveNumber    = 1;
        public int   enemyCount    = 3;

        [Header("Zamanlama")]
        // waiting time for enemies to enemies
        public float spawnInterval = 1.5f;
        
        // waiting time until wave is not finished 
        public float breakDuration = 3f;

        [Header("Strateji")]
        public StrategyType strategyType = StrategyType.Direct;
    }
}