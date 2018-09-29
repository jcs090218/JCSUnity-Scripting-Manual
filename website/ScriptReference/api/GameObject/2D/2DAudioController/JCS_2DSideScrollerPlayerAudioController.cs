﻿/**
 * $File: JCS_2DSideScrollerPlayerAudioController.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen <lkk440456@gmail.com> $
 * $Notice: $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{
    /// <summary>
    /// Audio controller for player
    /// </summary>
    public class JCS_2DSideScrollerPlayerAudioController 
        : JCS_2DPlayerAudioController
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Runtime Variables (JCS_2DSideScrollerPlayerAudioController) **")]
        [SerializeField] private bool mOverrideSound = true;

        [Header("** Sound Settings (JCS_2DSideScrollerPlayerAudioController) **")]
        [SerializeField] private AudioClip mStandSound = null;
        [SerializeField] private AudioClip mWalkSound = null;
        // only attack could have multiple sound
        [SerializeField] private AudioClip[] mAttackSounds = null;
        [SerializeField] private AudioClip[] mJumpSound = null;
        [SerializeField] private AudioClip mProneSound = null;
        [SerializeField] private AudioClip mAlertSound = null;
        [SerializeField] private AudioClip mFlySound = null;
        [SerializeField] private AudioClip mLadderSound = null;
        [SerializeField] private AudioClip mRopeSound = null;
        [SerializeField] private AudioClip mSitSound = null;
        [SerializeField] private AudioClip mHitSound = null;
        [SerializeField] private AudioClip mDanceSound = null;
        [SerializeField] private AudioClip mSwimSound = null;
        [SerializeField] private AudioClip mDeadSound = null;
        [SerializeField] private AudioClip mGhostSound = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// 
        /// </summary>
        public void BasicJumpSound()
        {
            if (mJumpSound.Length < 1)
                return;

            if (mJumpSound[0] == null)
            {
                JCS_Debug.LogError(
                    "Play sound with null references...");
                return;
            }
            mJCSSoundPlayer.PlayOneShot(mJumpSound[0], JCS_SoundSettings.instance.GetSkillsSound_Volume());
        }
        /// <summary>
        /// 
        /// </summary>
        public void DoubleJumpSound()
        {
            if (mJumpSound.Length < 2)
                return;

            if (mJumpSound[1] == null)
            {
                JCS_Debug.LogError( "Play sound with null references...");
                return;
            }
            mJCSSoundPlayer.PlayOneShot(mJumpSound[1], JCS_SoundSettings.instance.GetSkillsSound_Volume());
        }
        /// <summary>
        /// 
        /// </summary>
        public void TripleJumpSound()
        {
            if (mJumpSound.Length < 3)
                return;

            if (mJumpSound[2] == null)
            {
                JCS_Debug.LogError( "Play sound with null references...");
                return;
            }
            mJCSSoundPlayer.PlayOneShot(mJumpSound[2], JCS_SoundSettings.instance.GetSkillsSound_Volume());
        }
        /// <summary>
        /// 
        /// </summary>
        public void AttackSound()
        {
            if (!mOverrideSound)
            {
                // do not override the sound
                if (mJCSSoundPlayer.GetAudioSource().isPlaying)
                    return;
            }

            int rand = Random.Range(0, mAttackSounds.Length);

            if (mAttackSounds[rand] == null)
            {
                JCS_Debug.LogError(
                    "Play sound with null references...");

                return;
            }
            mJCSSoundPlayer.PlayOneShot(mAttackSounds[rand], JCS_SoundSettings.instance.GetSkillsSound_Volume());
        }
        /// <summary>
        /// 
        /// </summary>
        public void WalkSound()
        {
            if (mWalkSound == null)
            {
                JCS_Debug.LogError(
                    "Play sound with null references...");

                return;
            }
            mJCSSoundPlayer.PlayOneShot(mWalkSound, JCS_SoundSettings.instance.GetSkillsSound_Volume());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public void PlaySoundByPlayerState(JCS_LiveObjectState state)
        {
            AudioClip clip = GetSoundByPlayerState(state);

            mJCSSoundPlayer.PlayOneShot(clip);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private AudioClip GetSoundByPlayerState(JCS_LiveObjectState state)
        {
            switch (state)
            {
                case JCS_LiveObjectState.STAND:
                    return mStandSound;
                case JCS_LiveObjectState.WALK:
                    return mWalkSound;
                case JCS_LiveObjectState.RAND_ATTACK:
                    {
                        //int index = JCS_Random.Range(0, mAttackSounds.Length);
                        //return mAttackSounds[index];
                        return null;
                    }
                case JCS_LiveObjectState.JUMP:
                    {
                        //int index = JCS_Random.Range(0, mJumpSound.Length);
                        //return mJumpSound[index];
                        return null;
                    }
                case JCS_LiveObjectState.PRONE:
                    return mProneSound;
                case JCS_LiveObjectState.ALERT:
                    return mAlertSound;
                case JCS_LiveObjectState.FLY:
                    return mFlySound;
                case JCS_LiveObjectState.LADDER:
                    return mLadderSound;
                case JCS_LiveObjectState.ROPE:
                    return mRopeSound;
                case JCS_LiveObjectState.SIT:
                    return mSitSound;
                case JCS_LiveObjectState.HIT:
                    return mHitSound;
                case JCS_LiveObjectState.DANCE:
                    return mDanceSound;
                case JCS_LiveObjectState.SWIM:
                    return mSwimSound;
                case JCS_LiveObjectState.DIE:
                    return mDeadSound;
                case JCS_LiveObjectState.GHOST:
                    return mGhostSound;
            }

            JCS_Debug.LogError(
                "JCS_2DSideScrollerPlayerAudioController",
                 
                "Return sound that aren't in the player state...");

            return null;
        }

    }
}
