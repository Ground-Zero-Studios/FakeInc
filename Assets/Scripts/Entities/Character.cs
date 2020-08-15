using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GroundZero.Managers;
using UnityEngine.UIElements;

/* --------------------------------------------------------------------------------------------
 * 
 *                                      Character Entity
 *
 * --------------------------------------------------------------------------------------------
 * Description: Handle information that's relevant to a game character. This means the player
 * and NPCs will (or should) be using this class. 
 * 
 * 
 * 
 * Usage: Put it on a GameObject and set it up.
 * Use Character.HasFlag and Character.SetFlag to read/modify the character flags.
 * "velocity" can be changed from outside scripts for movement.
 * 
 * 
 * This class is part of the "Reuse" project.
 * This means this class can be reused for other projects from Ground Zero Studios.
 */


namespace GroundZero.Entities
{
    [DisallowMultipleComponent] // We dont want a GO with more than 1 Character script.
    [RequireComponent(typeof(CharacterController))] // Requires a Unity CharacterController
    public class Character : MonoBehaviour
    {
        // The max health of the character
        public float maxHealth;

        // The current health of the character
        private float health;

        // The Unity CharacterController
        CharacterController controller;

        // The amount to multiply the fall damage by. See more in ApplyFallDamage()
        public float fallDamageMultiplier = 10f;

        // The maximum height before taking fall damage
        public float maxFallHeight;

        // This character's velocity
        Vector3 velocity;

        // Was this character grounded the last frame?
        bool wasGrounded;

        // The amount of time it takes before it starts to regenerate health, if enabled.
        public float timeUntilHealing = 30f;

        // The amount of health to recover every second while healing
        public float healAmount;

        // The timer that keeps track of above's time.
        float healTimer;

        // The start and end point of the character's fall, when falling.
        Vector3 fallStart, fallStop;

        // Character Flags
        [Flags]
        public enum Flags
        {
            /* GODMODE:         If enabled, character will no longer take damage.
             * GRAVITY:         If enabled, character will simulate gravity.
             * DEAD:            If enabled, the character will not move, and other
             *                  scripts will know this character is dead. Gravity
             *                  and Sticking to ground will still apply.
             * FALLDAMAGE:      If enabled, the character will suffer fall damage.
             * CANMOVE:         If enabled, entities will handle movement.
             * CANJUMP:         If enabled, entities will handle jumping.
             * CANRUN:          If enabled, entities will handle running.
             *                  Note that CANJUMP and CANRUN can be flagged, but
             *                  nothing will happen if CANMOVE is not flagged.
             *                  Also, these 3 do nothing here, and are to be used
             *                  by outside classes (entities).
             * PASSIVEHEAL:     Will heal this character over time.
             */

            GODMODE         = 1 << 0,
            GRAVITY         = 1 << 1,
            DEAD            = 1 << 2,
            FALLDAMAGE      = 1 << 3,
            CANMOVE         = 1 << 4,
            CANJUMP         = 1 << 5,
            CANRUN          = 1 << 6,
            PASSIVEHEAL     = 1 << 7
        }

        // Damage Types for damage handling (Changes animations and other effects).
        [HideInInspector]
        public enum DamageType
        {
            /* GENERIC:     Does generic damage. Not many effects, no animation and just a "ouch!"
             * SILENT:      Deals damage without effects, animations or any visual or audio indication
             * FALL:        Falling from heights.
             * BRUTE:       Being punched, CONSECUTIVE C-Q-C.
             * BURN:        Being burned (Fire, Lava, Whatever)
             * BULLET:      Being shot. (Gunfire)
             * CRUSH:       Being crushed (Crushers!)
             * EXPLOSION:   Damage from explosions (Exploding props, grenades, etc)
             */

            GENERIC,
            SILENT,
            FALL,
            BRUTE,
            BURN,
            BULLET,
            CRUSH,
            EXPLOSION
        }

        // See above
        public Flags flags;

        // The last type of damaged this character suffered
        [HideInInspector]
        public DamageType lastDamageType = DamageType.GENERIC;


        // Start Method. Not much to see here.
        void Start()
        {
            // Assign the controller
            controller = GetComponent<CharacterController>();
            // Look for a ruling entity.

            health = maxHealth;
            healTimer = timeUntilHealing;
        }

        // Update - Try to keep it organized.
        void Update()
        {
            if (HasFlag(Flags.GRAVITY))
            {
                HandleGravity();
            }
            HandleVelocity();
        }

        #region Public Methods

        /// <summary>
        /// Checks for a character flag
        /// </summary>
        /// <param name="flag">The flag to be checked for</param>
        /// <returns>True if character has flag</returns>
        public bool HasFlag(Flags flag)
        {
            if (flags.HasFlag(flag))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets a character flag
        /// </summary>
        /// <param name="flag">The flag to modify</param>
        /// <param name="value">The new flag value</param>
        public void SetFlag(Flags flag, bool value)
        {
            if (value)
            {
                if (!HasFlag(flag))
                {
                    flags |= flag;
                }
            }
            else
            {
                if (HasFlag(flag))
                {
                    flags &= ~flag;
                }
            }
        }

        /// <summary>
        /// Deals damage to this character.
        /// </summary>
        /// <param name="amount">(Positive) Amount of damage</param>
        /// <param name="type">The type of damage</param>
        public void Damage(float amount, DamageType type)
        {
            // No damage if godmode is active
            if (!HasFlag(Flags.GODMODE))
            {
                health -= Mathf.Abs(amount);
            }
            // Register the last damage type taken.
            lastDamageType = type;
            UpdateStats();
            Debug.Log($"{name} has sustained {amount} {type.ToString()} damage");

            // Reset the natural heal time
            healTimer = timeUntilHealing;
        }

        /// <summary>
        /// Deals damage to this character.
        /// </summary>
        /// <param name="amount">(Positive) Amount of damage</param>
        public void Damage(float amount)
        {
            Damage(amount, DamageType.GENERIC);
        }

        /// <summary>
        /// Heals this character
        /// </summary>
        /// <param name="amount">(Positive) amount of health to heal.</param>
        public void Heal(float amount)
        {
            health += Mathf.Abs(amount);
            UpdateStats();
        }


        #endregion

        #region Private Methods
        /// <summary>
        /// Update this character's stats (Health, etc)
        /// </summary>
        void UpdateStats()
        {
            health = Mathf.Clamp(health, 0, maxHealth);

            if (HasFlag(Flags.DEAD))
            {
                if (health > 0f)
                {
                    SetFlag(Flags.DEAD, false);
                }
            }
            else
            {
                if (health <= 0f)
                {
                    SetFlag(Flags.DEAD, true);
                }
            }
        }

        /// <summary>
        /// Handles gravity for this character
        /// </summary>
        void HandleGravity()
        {
            // This assumes gravity is in the Y axis.
            // If not, you'll have to set the new axis in the below code where it updates velocity
            velocity += (Physics.gravity * Time.deltaTime);

            // If character was grounded last frame...
            if (wasGrounded)
            {
                // And it remains grounded...
                if (controller.isGrounded)
                {
                    velocity.y = 0f;
                    wasGrounded = true;
                }
                else // And is no longer grounded (Jumping or falling of ledge)
                {
                    fallStart = transform.position;
                    wasGrounded = false;
                }
            }
            else // If character was not grounded last frame...
            {
                // And he hits the ground...
                if (controller.isGrounded)
                {
                    fallStop = transform.position;
                    if (HasFlag(Flags.FALLDAMAGE))
                    {
                        HandleFallDamage();
                    }

                    wasGrounded = true;
                }
                else // And he keeps falling...
                {
                    wasGrounded = false;
                }
            }
        }

        void HandleFallDamage()
        {
            // Get the fall height (Negative values means a... climb?)
            float fallHeight = fallStart.y - fallStop.y;
            
            // If it's bigger than the allowed fall height.
            if (fallHeight > maxFallHeight)
            {
                float fallDamage = (fallHeight - maxFallHeight) * fallDamageMultiplier;
                Damage(fallDamage, DamageType.FALL);
            }
        }

        void HandleVelocity()
        {
            controller.Move(velocity * Time.deltaTime);
        }

        void HandleHealing()
        {
            if (healTimer <= 0)
            {
                Heal(healAmount * Time.deltaTime);
            } else
            {
                healTimer -= Time.deltaTime;
            }
        }
        
        #endregion




    }
}
