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
 * and NPCs will (or should) be using this class as their parent.
 * 
 * 
 * 
 * Usage: Inherit it on a class to make it a Character entity.
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
        public Vector3 velocity;

        // The last velocity of this Character
        public Vector3 lastVelocity;

        // Was this character grounded the last frame?
        bool wasGrounded;

        // The amount of time it takes before it starts to regenerate health, if enabled.
        public float timeUntilHealing = 30f;

        // The amount of health to recover every second while healing
        public float healAmount;

        // The last damage taken by this Character
        public float lastDamage;

        // The last healing taken by this Character
        public float lastHeal;

        // The last object to deal damage to this character
        public object lastDamageDealer;

        // The last object to heal this character
        public object lastHealer;

        // Is this character moving?
        public bool isMoving;

        // Friction (Deaceleration)
        public float friction = 0.8f;

        // The timer that keeps track of when to heal.
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
             *                  by children classes.
             * PASSIVEHEAL:     Will heal this character over time.
             * FRICTION:        Will deacelerate on X and Z axis over time.
             */

            GODMODE         = 1 << 0,
            GRAVITY         = 1 << 1,
            DEAD            = 1 << 2,
            FALLDAMAGE      = 1 << 3,
            CANMOVE         = 1 << 4,
            CANJUMP         = 1 << 5,
            CANRUN          = 1 << 6,
            PASSIVEHEAL     = 1 << 7,
            FRICTION        = 1 << 8
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
             * ELETRIC:     Damage from electricity (Teslas, power box shocks, whatever)
             * TOXIC:       Damage from toxic things (Gases, hazards, twitter, etc)
             * POISON:      Damage from poison stuff?
             */

            GENERIC,
            SILENT,
            FALL,
            BRUTE,
            BURN,
            BULLET,
            CRUSH,
            EXPLOSION,
            ELETRIC,
            TOXIC,
            POISON
        }

        // See above
        public Flags flags;

        // The last type of damaged this character suffered
        [HideInInspector]
        public DamageType lastDamageType = DamageType.GENERIC;


        // Start Method. Not much to see here.
        public virtual void Start()
        {
            // Assign the controller
            controller = GetComponent<CharacterController>();

            health = maxHealth;
            healTimer = timeUntilHealing;
        }

        // Update - Try to keep it organized.
        public virtual void Update()
        {
            if (HasFlag(Flags.GRAVITY))
            {
                HandleGravity();
            }

            if (HasFlag(Flags.FRICTION))
            {
                HandleFriction();
            }
            
            HandleVelocity();
        }

        #region Public Methods

        /// <summary>
        /// Checks for a character flag
        /// </summary>
        /// <param name="flag">The flag to be checked for</param>
        /// <returns>True if character has flag</returns>
        public virtual bool HasFlag(Flags flag)
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
        public virtual void SetFlag(Flags flag, bool value)
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
            Damage(amount, type, null);
        }

        /// <summary>
        /// Deals damage to this character.
        /// </summary>
        /// <param name="amount">(Positive) Amount of damage</param>
        public void Damage(float amount)
        {
            Damage(amount, DamageType.GENERIC, null);
        }

        /// <summary>
        /// Deals damage to this character.
        /// </summary>
        /// <param name="amount">(Positive) Amount of damage</param>
        /// <param name="type">The type of damage</param>
        /// <param name="dealer">Object that dealt the damage.</param>
        public virtual void Damage(float amount, DamageType type, object dealer)
        {
            // No damage if godmode is active
            if (!HasFlag(Flags.GODMODE))
            {
                health -= Mathf.Abs(amount);
            }

            // Register the last damage type taken & last dealer
            lastDamageDealer = dealer;
            lastDamageType = type;
            UpdateStats();

            // Reset the natural heal time
            healTimer = timeUntilHealing;
            OnDamaged(amount, type, dealer);
        }



        /// <summary>
        /// Heals this character
        /// </summary>
        /// <param name="amount">(Positive) amount of health to heal.</param>
        public void Heal(float amount) => Heal(amount, this, false);
        /// <summary>
        /// Heals this character
        /// </summary>
        /// <param name="amount">(Positive) amount of health to heal.</param>
        /// <param name="passive">Is it passive healing?.</param>
        public void Heal(float amount, bool passive) => Heal(amount, this, passive);

        /// <summary>
        /// Heals this character
        /// </summary>
        /// <param name="amount">(Positive) amount of health to heal.</param>
        /// <param name="passive">Is it passive healing or not?.</param>
        public virtual void Heal(float amount, object healer, bool passive)
        {
            // Register last healer & last heal
            lastHeal = amount;
            lastHealer = healer;
            // Add health
            health += Mathf.Abs(amount);
            // Update stats
            UpdateStats();
            // Call onHeal method
            OnHeal(healAmount * Time.deltaTime, healer, passive);
        }

        public virtual void AddVelocity(Vector3 velocity)
        {
            lastVelocity = this.velocity;
            this.velocity += velocity;
            OnVelocityChange(lastVelocity, velocity);
        }

        public virtual void SetVelocity(Vector3 velocity)
        {
            lastVelocity = this.velocity;
            this.velocity = velocity;
            OnVelocityChange(lastVelocity, velocity);
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
                    OnRevive(health, lastHealer);
                }
            }
            else
            {
                if (HasFlag(Flags.PASSIVEHEAL))
                {
                    if (healTimer <= 0f)
                    {
                        if (health < maxHealth)
                        {
                            Heal(healAmount * Time.deltaTime, true);
                        }
                    }
                    else
                    {
                        healTimer -= Time.deltaTime;
                    }
                }
                if (health <= 0f)
                {
                    SetFlag(Flags.DEAD, true);
                    OnDeath(lastDamage, lastDamageType, lastDamageDealer);
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
            AddVelocity(Physics.gravity * Time.deltaTime);

            // If character was grounded last frame...
            if (wasGrounded)
            {
                // And it remains grounded...
                if (controller.isGrounded)
                {
                    SetVelocity(new Vector3(velocity.x, 0f, velocity.z));
                    wasGrounded = true;
                }
                else // And is no longer grounded (Jumping or falling of ledge)
                {
                    fallStart = transform.position;
                    OnLeaveGround(velocity, fallStart);
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
                    OnLand(velocity, fallStop);
                    wasGrounded = true;
                }
                else // And he keeps falling...
                {
                    wasGrounded = false;
                }
            }
        }

        void HandleFriction()
        {
            if (!isMoving)
            {
                velocity *= friction * Time.deltaTime;
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
        
        #endregion

        public virtual void OnDamaged(float damage, DamageType type, object dealer) { }
        public virtual void OnLeaveGround(Vector3 velocity, Vector3 position) { }
        public virtual void OnLand(Vector3 velocity, Vector3 position) { }
        public virtual void OnHeal(float amount, object healer, bool passive) { }
        public virtual void OnDeath(float finalBlow, DamageType dmgType, object killer) { }
        public virtual void OnRevive(float heal, object healer) { }
        public virtual void OnVelocityChange(Vector3 lastVelocity, Vector3 newVelocity) { }
    }
}
