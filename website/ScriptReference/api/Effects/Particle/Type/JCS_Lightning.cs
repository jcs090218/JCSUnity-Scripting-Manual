﻿/**
 * $File: JCS_Lightning.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{
    /// <summary>
    /// Lightning particle.
    /// </summary>
    [RequireComponent(typeof(JCS_SoundPlayer))]
    [RequireComponent(typeof(JCS_SoundPool))]
    [RequireComponent(typeof(JCS_AnimPool))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(JCS_DisableWithAnimEndEvent))]
    public class JCS_Lightning
        : JCS_WeatherParticle
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private JCS_AnimPool mAnimPool = null;
        private Animator mAnimator = null;

        // Sound settings
        private JCS_SoundPool mSoundPool = null;
        private JCS_SoundPlayer mSoundPlayer = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();
            this.mSoundPool = this.GetComponent<JCS_SoundPool>();
            this.mAnimPool = this.GetComponent<JCS_AnimPool>();

            this.mAnimator = this.GetComponent<Animator>();
        }
        
        private void OnEnable()
        {
            // if app still initializing then return.
            if (JCS_ApplicationManager.APP_INITIALIZING)
                return;

            // when enabled play the sound and animation.
            PlayLightningSound();
            PlayLightningAnim();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// 
        /// </summary>
        private void PlayLightningSound()
        {
            AudioClip ac = this.mSoundPool.GetRandomSound();

            mSoundPlayer.PlayOneShotWhileNotPlaying(ac);
        }

        /// <summary>
        /// 
        /// </summary>
        private void PlayLightningAnim()
        {
            RuntimeAnimatorController con = this.mAnimPool.GetRandomAnim();
            mAnimator.runtimeAnimatorController = con;
        }

    }
}
