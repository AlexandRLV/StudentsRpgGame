﻿using UnityEngine;

namespace Invector.vCharacterController.AI
{
    using System.Collections.Generic;
    using vEventSystems;

    public partial interface vIControlAI : vIHealthController
    {
        /// <summary>
        /// Used just to Create AI Editor
        /// </summary>
        void CreatePrimaryComponents();

        /// <summary>
        /// Used just to Create AI Editor
        /// </summary>
        void CreateSecondaryComponents();

        /// <summary>
        /// Check if <seealso cref="vIControlAI"/> has a <seealso cref=" vIAIComponent"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool HasComponent<T>() where T : vIAIComponent;

        /// <summary>
        /// Get Specific <seealso cref="vIAIComponent"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetAIComponent<T>() where T : vIAIComponent;

        Vector3 selfStartPosition { get; set; }
        Vector3 targetDestination { get; }
        Collider selfCollider { get; }
        Animator animator { get; }
        vAnimatorStateInfos animatorStateInfos { get; }
        vWaypointArea waypointArea { get; set; }
        vAIReceivedDamegeInfo receivedDamage { get; }
        vWaypoint targetWaypoint { get; }
        List<vWaypoint> visitedWaypoints { get; set; }

        Vector3 lastTargetPosition { get; }
        bool ragdolled { get; }
        bool isInDestination { get; }
        bool isMoving { get; }
        bool isStrafing { get; }
        bool isRolling { get; }
        bool isCrouching { get; set; }
        bool targetInLineOfSight { get; }
        vAISightMethod SightMethod { get; set; }
        vAIUpdateQuality UpdatePathQuality { get; set; }
        vAIUpdateQuality FindTargetUpdateQuality { get; set; }
        vAIUpdateQuality CanseeTargetUpdateQuality { get; set; }
        vAIMovementSpeed movementSpeed { get; }
        float targetDistance { get; }
        float changeWaypointDistance { get; }
        Vector3 desiredVelocity { get; }
        float remainingDistance { get; }
        float stopingDistance { get; set; }
        float minDistanceToDetect { get; set; }
        float maxDistanceToDetect { get; set; }
        float fieldOfView { get; set; }
        bool selfStartingPoint { get; }
        bool customStartPoint { get; }

        Vector3 customStartPosition { get; }
        void SetDetectionLayer(LayerMask mask);
        void SetDetectionTags(List<string> tags);
        void SetObstaclesLayer(LayerMask mask);
        void SetLineOfSight(float fov = -1, float minDistToDetect = -1, float maxDistToDetect = -1, float lostTargetDistance = -1);
        void NextWayPoint();

        /// <summary>
        /// Move AI to a position in World Space
        /// </summary>
        /// <param name="destination">world space position</param>
        /// <param name="speed">movement speed</param>
        void MoveTo(Vector3 destination, vAIMovementSpeed speed = vAIMovementSpeed.Walking);
        /// <summary>
        /// Move AI to a position in World Space and rotate to a custom direction
        /// </summary>
        /// <param name="destination">world space position</param>
        /// <param name="forwardDirection">target rotation direction</param>
        /// <param name="speed">>movement speed</param>
        void StrafeMoveTo(Vector3 destination, Vector3 forwardDirection, vAIMovementSpeed speed = vAIMovementSpeed.Walking);

        /// <summary>
        /// Move AI to a position in World Space with out update the target rotation direction of the AI
        /// </summary>
        /// <param name="destination">world space position</param>      
        /// <param name="speed">>movement speed</param>
        void StrafeMoveTo(Vector3 destination, vAIMovementSpeed speed = vAIMovementSpeed.Walking);

        void RotateTo(Vector3 direction);
        void RollTo(Vector3 direction);
        void SetCurrentTarget(Transform target);
        void SetCurrentTarget(Transform target, bool overrideCanseeTarget);
        void RemoveCurrentTarget();
        vAITarget currentTarget { get; }

        /// <summary>
        /// Return a list of targets resulted of <seealso cref="FindTarget"/> method
        /// </summary>
        /// <returns></returns>
        Collider[] GetTargetsInRange();
        /// <summary>
        /// Find target using Detection settings
        /// </summary>
        void FindTarget();
        /// <summary>
        /// Find target with ignoring obstacles option
        /// </summary>
        /// <param name="checkForObstacles"></param>
        void FindTarget(bool checkForObstacles);
        /// <summary>
        /// Try get a target 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        bool TryGetTarget(out vAITarget target);
        /// <summary>
        /// Try get a target
        /// </summary>
        /// <param name="tag">possible target tag</param>
        /// <param name="target"></param>
        /// <returns></returns>
        bool TryGetTarget(string tag, out vAITarget target);
        /// <summary>
        /// Try get a target 
        /// </summary>
        /// <param name="m_detectTags">list of possible target tags</param>
        /// <param name="target"></param>
        /// <returns></returns>
        bool TryGetTarget(List<string> m_detectTags, out vAITarget target);
        /// <summary>
        /// Find target with specific detection settings
        /// </summary>
        /// <param name="m_detectTags">list of possible target tags</param>
        /// <param name="m_detectLayer">layer of possible target</param>
        /// <param name="checkForObstables"></param>
        void FindSpecificTarget(List<string> m_detectTags, LayerMask m_detectLayer, bool checkForObstables = true);
        void LookAround();
        void LookTo(Vector3 point, float stayLookTime = 1f, float offsetLookHeight = -1);
        void LookToTarget(Transform target, float stayLookTime = 1f, float offsetLookHeight = -1);
        void Stop();
        void ForceUpdatePath(float timeInUpdate = 1f);

        /// <summary>
        /// Check if AI is Trigger With some collider with specific tag
        /// </summary>
        /// <param name="targ"></param>
        /// <returns></returns>
        bool IsInTriggerWithTag(string tag);
        /// <summary>
        /// Check if AI is Trigger With some collider with specific name
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        bool IsInTriggerWithName(string name);

        /// <summary>
        /// Check if AI is Trigger With some collider with specific name
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        bool IsInTriggerWithTag(string tag, out Collider result);
        /// <summary>
        /// Check if AI is Trigger With some collider with specific tag
        /// </summary>
        /// <param name="name"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        bool IsInTriggerWithName(string name, out Collider result);
    }

    public partial interface vIControlAICombat : vIControlAI
    {
        int strafeCombatSide { get; }
        float minDistanceOfTheTarget { get; }
        float combatRange { get; }
        bool isInCombat { get; set; }
        bool strafeCombatMovement { get; }

        int attackCount { get; set; }
        float attackDistance { get; }
        bool isAttacking { get; }
        bool canAttack { get; }
        void InitAttackTime();
        void ResetAttackTime();
        void Attack(bool strongAttack = false, int attackID = -1, bool forceCanAttack = false);

        bool isBlocking { get; }
        bool canBlockInCombat { get; }
        void ResetBlockTime();
        void Blocking();

        void AimTo(Vector3 point, float stayLookTime = 1f, object sender = null);
        void AimToTarget(float stayLookTime = 1f, object sender = null);

        bool isAiming { get; }
        bool isArmed { get; }
    }

    public partial interface vIControlAIMelee : vIControlAICombat
    {
        vMelee.vMeleeManager MeleeManager { get; set; }
        void SetMeleeHitTags(List<string> tags);
    }

    public partial interface vIControlAIShooter : vIControlAICombat
    {
        vAIShooterManager shooterManager { get; set; }
        void SetShooterHitLayer(LayerMask mask);
        /// <summary>
        /// Check if Aim is aligned to the target position setted from method <see cref="vIControlAICombat.AimTo(Vector3, float, object)"/>
        /// </summary>
        bool IsInShotAngle { get; }
    }


    [System.Serializable]
    public class vAITarget
    {
        [SerializeField] protected Transform _transform;
        [SerializeField, HideInInspector] protected Collider _collider;
        public Transform transform { get { return _transform; } protected set { _transform = value; } }
        public Collider collider { get { return _collider; } protected set { _collider = value; } }

        public vIHealthController healthController;
        public vIControlAICombat combateController;
        public vIMeleeFighter meleeFighter;
        public vICharacter character;

        public bool isFixedTarget = true;
        [HideInInspector]
        public bool isLost;
        [HideInInspector]
        public bool _hadHealthController;

        public bool hasCollider
        {
            get
            {
                return collider != null;
            }
        }
        public static implicit operator Transform(vAITarget m)
        {
            try
            {
                return m.transform;
            }
            catch { return null; }
        }
        public bool hasHealthController
        {
            get
            {
                if (_hadHealthController && healthController == null)
                    transform = null;
                return healthController != null;
            }
        }

        public bool isDead
        {
            get
            {
                var value = true;
                if (hasHealthController) value = healthController.isDead;
                else if (_hadHealthController) value = true;
                else if (!transform.gameObject.activeInHierarchy) value = true;
                else if (_collider) value = !_collider.enabled;
                return value;
            }
        }

        public bool isArmed
        {
            get
            {
                if (!isFighter) return false;
                return meleeFighter != null ? meleeFighter.isArmed : combateController != null ? combateController.isArmed : false;
            }
        }

        public bool isBlocking
        {
            get
            {
                if (!isFighter) return false;
                return meleeFighter != null ? meleeFighter.isBlocking : combateController != null ? combateController.isBlocking : false;
            }
        }

        public bool isAttacking
        {
            get
            {
                if (!isFighter) return false;
                return meleeFighter != null ? meleeFighter.isAttacking : combateController != null ? combateController.isAttacking : false;
            }
        }

        public bool isFighter
        {
            get
            {
                return meleeFighter != null || combateController != null;
            }
        }

        public bool isCharacter
        {
            get
            {
                return character != null;
            }
        }

        public float currentHealth
        {
            get
            {
                if (hasHealthController) return healthController.currentHealth;
                return 0;
            }
        }

        public void InitTarget(Transform target)
        {
            if (target)
            {
                transform = target;
                collider = transform.GetComponent<Collider>();
                healthController = transform.GetComponent<vIHealthController>();
                _hadHealthController = this.healthController != null;
                meleeFighter = transform.GetComponent<vIMeleeFighter>();
                character = transform.GetComponent<vICharacter>();
                combateController = transform.GetComponent<vIControlAICombat>();
            }
        }

        public void ClearTarget()
        {
            transform = null;
            collider = null;
            healthController = null;
            meleeFighter = null;
            character = null;
            combateController = null;
        }
    }

    [System.Serializable]
    public class vAIReceivedDamegeInfo
    {
        public vAIReceivedDamegeInfo()
        {
            lasType = "unnamed";
        }
        [vReadOnly(false)] public bool isValid;
        [vReadOnly(false)] public float lastValue;
        [vReadOnly(false)] public string lasType = "unnamed";
        [vReadOnly(false)] public Transform lastSender;
        [vReadOnly(false)] public int massiveCount;
        [vReadOnly(false)] public float massiveValue;

        protected float lastValidDamage;
        float _massiveTime;
        public void Update()
        {
            _massiveTime -= Time.deltaTime;
            if (_massiveTime <= 0)
            {
                _massiveTime = 0;
                if (massiveValue > 0) massiveValue -= 1;
                if (massiveCount > 0) massiveCount -= 1;
            }
            isValid = lastValidDamage > Time.time;
        }

        public void UpdateDamage(vDamage damage, float validDamageTime = 2f)
        {
            if (damage == null) return;
            lastValidDamage = Time.time + validDamageTime;
            _massiveTime += Time.deltaTime;
            massiveCount++;
            lastValue = damage.damageValue;
            massiveValue += lastValue;
            lastSender = damage.sender;
            lasType = string.IsNullOrEmpty(damage.damageType) ? "unnamed" : damage.damageType;
        }
    }

    public interface vIStateAttackListener
    {
        void OnReceiveAttack(vIControlAICombat combatController, ref vDamage damage, vIMeleeFighter attacker, ref bool canBlock);
    }
}