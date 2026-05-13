using System;
using UnityEngine;

namespace CoreBreach.Patterns.Observer
{
    //observer is like event handler for game
    //no system dont have to know directly other systems
    //when something happens, it will reported here and listeners reacts
    public static class GameEvents
    {

        // Oyunun bitip bitmediğini tutan global bayrak
        // TODO: Phase 8 — GameManager bu flag'i yönetecek
        public static bool IsGameOver = false;


        //core events:

        //it will get triggered when core get take damage or recover 
        //float currentHealth -> healt at the moment, float maxHealth -> max healt is used for HUD percentage calculations
        //who fires: CoreHealth.TakeDamage()
        //who listens: HUDController, AudioManager
        public static Action<float, float> OnCoreHealthChanged;

        //it will get triggered when core's health is zero
        //who fires: CoreHealth.TakeDamage()
        //who listens: GameManager (used for "you losed" screen)
        public static Action OnCoreDead;


        //enemy events: 

        //triggered when an enemy dies
        //Vector3 position -> posiition for efect or loot spawn
        //who fires: EnemyHealth.TakeDamage()
        //who listens: ScoreManager, WaveManager(remainin number)
        //TODO: you can add score number for parameter -> Action<Vector3, int>
        public static Action<Vector3, bool> OnEnemyDied;

        //wawe GameEvents
        //it triggers when a wawe is completed
        //int waweNumber ->  which wawe is completed
        //who fires: WaveManager
        //who listens: GameManager, HUDController
        //TODO: if it is last wave it may be trigger the OnGameWon
        public static Action<int> OnWaveCompleted;

        //it triggers when all waves are finished and game is woned
        //who fires: WaveManager
        //who listens: GameManager (wining screen)
        //TODO when WaveManager added, invoke that event from there
        public static Action OnGameWon;


        //security: cleaning in screen changes
        //when screen loads again old subscribers are cleaned.
        //it is called in GameManager
        //TODO: when we write GameManager, OnEnable can be called inside
        public static void ResetAllEvents()
        {
            IsGameOver          = false;
            OnCoreHealthChanged = null;
            OnCoreDead          = null;
            OnEnemyDied         = null;
            OnWaveCompleted     = null;
            OnGameWon           = null;

            Debug.Log("[GameEvents] All events cleaned.");
        }
    
    }
}
