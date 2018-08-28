using CBFrame.Core;
using CBFrame.Sys;
using System;
using UnityEngine;

namespace CompleteProject
{
    public class GameOverManager : MonoBehaviour
    {
        public PlayerHealth playerHealth;       // Reference to the player's health.


        Animator anim;                          // Reference to the animator component.

        private void Start()
        {
            CBGlobalEventDispatcher.Instance.AddEventListener<PlayerHealth>((int)EVENT_ID.EVENT_PLAYER_HEALTH, AttachHealth);
        }

        public void AttachHealth(PlayerHealth h)
        {
            playerHealth = h;
        }

        public void RemoveHealth()
        {
            playerHealth = null;
        }

        void Awake ()
        {
            // Set up the reference.
            anim = GetComponent <Animator> ();
        }


        void Update ()
        {
            // If the player has run out of health...
            if(playerHealth && playerHealth.isActiveAndEnabled && playerHealth.currentHealth <= 0)
            {
                // ... tell the animator the game is over.
                anim.SetTrigger ("GameOver");
            }
        }
    }
}