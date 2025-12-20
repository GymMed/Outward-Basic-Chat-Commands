using OutwardBasicChatCommands.Managers;
using OutwardBasicChatCommands.Utility.Enums;
using OutwardBasicChatCommands.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace OutwardBasicChatCommands.Utility.Data
{
    public class FollowerData
    {
        public Character Follower { get; }
        public Character CharacterToFollow { get; set; }

        public NavMeshPath NavMeshPath { get; }
        public bool IsFollowing { get => _isFollowing; set => _isFollowing = value; }

        private int _cornerIndex;
        private float _lastPathTime;

        private LocalCharacterControl _localControl;
        private bool _isFollowing;

        public static float InitialMaxDistance = 20f;
        private const float StopFollowingDistance = 5f;
        private const float PathRecalcInterval = 0.5f;
        private const float CornerReachDistance = 0.6f;
        private const float RotationSpeed = 7f;

        public FollowerData(Character follower, Character characterToFollow)
        {
            Follower = follower;
            CharacterToFollow = characterToFollow;
            NavMeshPath = new NavMeshPath();

            _localControl = follower.CharacterControl as LocalCharacterControl;
        }

        public bool StartFollow()
        {
            if (Follower == null || CharacterToFollow == null)
            {
                OBCC.LogMessage("Follower is null or you are trying to follow null character.");
                return false;
            }

            // One-time anti-cheese distance check
            if (!CharacterHelpers.IsCharacterInDistance(
                    Follower,
                    CharacterToFollow,
                    InitialMaxDistance))
            {
                ChatHelpers.SendChatLog(
                    Follower,
                    "Target is too far away to start following.",
                    ChatLogStatus.Warning);
                return false;
            }

            // We only support local-controlled characters
            if (Follower.CharacterControl is not LocalCharacterControl lcc)
            {
                ChatHelpers.SendChatLog(
                    Follower,
                    "Auto-follow only works on local characters.",
                    ChatLogStatus.Warning);
                return false;
            }

            _localControl = lcc;
            IsFollowing = true;
            _cornerIndex = 0;

            // Reset any manual locomotion state
            //_localControl.StopAutoRun();
            //_localControl.m_autoRun = true;

            // Calculate initial path
            RecalculatePath();

            ChatHelpers.SendChatLog(
                Follower,
                $"Now following {CharacterToFollow.Name}.",
                ChatLogStatus.Info);

            return true;
        }

        public void Update()
        {
            if (!IsFollowing || _localControl == null || CharacterToFollow == null)
                return;

            // Cancel ONLY on real player input
            if (Follower == null || Follower.IsDead || PlayerProvidedMovementInput())
            {
                StopFollow();
                return;
            }

            if (_localControl.InputLocked)
            {
                StopFollow();
                return;
            }

            ApplyFollowMovement();
        }

        private void RecalculatePath()
        {
            NavMesh.CalculatePath(
                Follower.transform.position,
                CharacterToFollow.transform.position,
                NavMesh.AllAreas,
                NavMeshPath);

            _cornerIndex = 0;
        }

        private void ApplyFollowMovement()
        {
            if (NavMeshPath.corners == null || NavMeshPath.corners.Length == 0)
            {
                ChatHelpers.SendChatLog(
                    Follower,
                    "Couldn't find navigation path! Recalculating.",
                    ChatLogStatus.Warning);

                RecalculatePath();
                return;
            }

            float distanceToTarget = Vector3.Distance(
                Follower.transform.position,
                CharacterToFollow.transform.position);

            // Stop moving if close enough
            if (distanceToTarget <= StopFollowingDistance)
            {
                _localControl.m_moveInput = Vector2.zero;
                _localControl.m_modifMoveInput = Vector2.zero;
                
                // Stop sprinting when we stop moving
                if (_localControl.m_character.Sprinting)
                {
                    _localControl.m_character.SprintInput(false);
                }
                return;
            }

            // Recalculate path periodically
            if (Time.time - _lastPathTime > PathRecalcInterval)
            {
                RecalculatePath();
                _lastPathTime = Time.time;
            }

            RotateTowardsPath();
            MoveTowardsPath();
        }

        private void RotateTowardsPath()
        {
            if (NavMeshPath.corners == null || NavMeshPath.corners.Length == 0)
                return;

            // Advance corner if close
            while (_cornerIndex < NavMeshPath.corners.Length - 1 &&
                   Vector3.Distance(
                       Follower.transform.position,
                       NavMeshPath.corners[_cornerIndex]) < CornerReachDistance)
            {
                _cornerIndex++;
            }

            Vector3 corner = NavMeshPath.corners[_cornerIndex];
            Vector3 direction = corner - Follower.transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.001f)
                return;

            Quaternion targetRotation =
                Quaternion.LookRotation(direction.normalized, Vector3.up);

            Follower.transform.rotation = Quaternion.Slerp(
                Follower.transform.rotation,
                targetRotation,
                RotationSpeed * Time.deltaTime);
        }

        private void MoveTowardsPath()
        {
            if (NavMeshPath.corners == null || _cornerIndex >= NavMeshPath.corners.Length)
                return;

            Vector3 pathDirection = (NavMeshPath.corners[_cornerIndex] - _localControl.m_character.transform.position).normalized;
            pathDirection.y = 0f;

            Vector3 camForward = _localControl.m_character.CharacterCamera.transform.forward;
            Vector3 camRight   = _localControl.m_character.CharacterCamera.transform.right;
            camForward.y = camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            float y = Vector3.Dot(pathDirection, camForward);
            float x = Vector3.Dot(pathDirection, camRight);

            Vector2 moveInputVec = new Vector2(x, y);
            if (moveInputVec.magnitude > 1f)
                moveInputVec.Normalize();

            // Calculate sprint speed like the game does
            float speed = _localControl.m_character.Speed * 1.75f;
            if (_localControl.m_character.IsSuperSpeedActive)
                speed *= 4f;
            
            // Apply speed multipliers
            speed *= _localControl.MovementMultiplier * _localControl.m_character.Stats.MovementSpeed;
            
            // Set m_modifMoveInput with proper sprint speed
            _localControl.m_modifMoveInput = moveInputVec * speed;
            
            // Also set m_moveInput for consistency
            _localControl.m_moveInput = moveInputVec;
            
            // Enable sprinting animation/state
            if (!_localControl.m_character.Sprinting && _localControl.m_character.Stats.CanSprint())
            {
                _localControl.m_character.SprintInput(true);
            }
            
            // Disable autorun to prevent interference
            _localControl.m_autoRun = false;
        }

        private void StopFollow()
        {
            IsFollowing = false;

            if (_localControl != null)
            {
                _localControl.m_moveInput = Vector2.zero;
                _localControl.m_modifMoveInput = Vector2.zero;
                
                // Stop sprinting
                if (_localControl.m_character != null && _localControl.m_character.Sprinting)
                {
                    _localControl.m_character.SprintInput(false);
                }
            }

            FollowerDataManager.Instance.RemoveFollower(Follower.UID);

            ChatHelpers.SendChatLog(
                Follower,
                "Auto-follow stopped (movement input detected).",
                ChatLogStatus.Info);
        }

        private bool PlayerProvidedMovementInput()
        {
            int playerID = Follower.OwnerPlayerSys.PlayerID;

            float h = ControlsInput.MoveHorizontal(playerID);
            float v = ControlsInput.MoveVertical(playerID);

            return Mathf.Abs(h) > 0.01f || Mathf.Abs(v) > 0.01f;
        }
    }
}
