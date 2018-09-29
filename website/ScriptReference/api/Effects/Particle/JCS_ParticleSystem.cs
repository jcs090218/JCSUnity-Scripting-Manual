﻿/**
 * $File: JCS_ParticleSystem.cs $
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
    /// Simulate the Rain effect
    /// </summary>
    [RequireComponent(typeof(JCS_EnvironmentSoundPlayer))]
    public class JCS_ParticleSystem
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables


        [Header("** Initialize Variables (JCS_2DParticleSystem) **")]

        [Tooltip("Number of particle this particle system hold.")]
        [SerializeField]
        [Range(50, 5000)]
        private int mNumOfParticle = 50;

        private JCS_Vector<JCS_Particle> mParticles = null;
        private int mLastAvaliableIndex = 0;


        [Header("** Runtime Variables (JCS_2DParticleSystem) **")]

        [Tooltip("Particle u want to spawn")]
        [SerializeField]
        private JCS_Particle mParticle = null;

        [Tooltip("Is the particle engine active?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip("Active thread pool?")]
        [SerializeField]
        private bool mActiveThread = true;

        [Tooltip("What layer should this be render?")]
        [SerializeField]
        private int mOrderLayer = 15;

        [Tooltip("How much do it range?")]
        [SerializeField]
        [Range(0, 1000)]
        private int mDensity = 5;

        [Tooltip("How much to tilt the particle?")]
        [SerializeField]
        [Range(-180, 179)]
        private float mWindSpeed = 0;


        [Header("** Position Settings (JCS_2DParticleSystem) **")]

        [Tooltip("Randomize the X position. (Default : 0)")]
        [SerializeField]
        [Range(0, 1000)]
        private float mRandPosX = 0;

        [Tooltip("Randomize the Y position. (Default : 0)")]
        [SerializeField]
        [Range(0, 1000)]
        private float mRandPosY = 0;

        [Tooltip("Randomize the Z position. (Default : 0)")]
        [SerializeField]
        [Range(0, 1000)]
        private float mRandPosZ = 0;


        [Header("** Rotation Settings (JCS_2DParticleSystem) **")]

        [Tooltip("")]
        [SerializeField]
        [Range(0, 359)]
        private float mRandAngleX = 0;

        [Tooltip("")]
        [SerializeField]
        [Range(0, 359)]
        private float mRandAngleY = 0;

        [Tooltip("Wind speed in 2 dimensional.")]
        [SerializeField]
        [Range(0, 359)]
        private float mRandAngleZ = 0;


        [Header("** Scale Settings (JCS_2DParticleSystem) **")]

        [Tooltip(@"Apply the scale always the same. This will 
only take the and mRandScaleX as a standard and ignore 
mRandScaleY and mRandScaleZ variables.")]
        [SerializeField]
        private bool mAlwaysTheSameScale = true;

        [Tooltip("Randomize the X scale. (Default : 0)")]
        [SerializeField]
        [Range(0, 1000)]
        private float mRandScaleX = 0;

        [Tooltip("Randomize the Y scale. (Default : 0)")]
        [SerializeField]
        [Range(0, 1000)]
        private float mRandScaleY = 0;

        [Tooltip("Randomize the Z scale. (Default : 0)")]
        [SerializeField]
        [Range(0, 1000)]
        private float mRandScaleZ = 0;


        private float mSequenceTimer = 0;
        private float mTimeAParticle = 0.5f;


        [Header("- Freeze Effect")]

        [Tooltip("")]
        [SerializeField]
        private bool mFreezeX = false;

        [Tooltip("")]
        [SerializeField]
        private bool mFreezeY = true;

        [Tooltip("")]
        [SerializeField]
        private bool mFreezeZ = true;

        private Vector3 mFreezePos = Vector3.zero;


        [Header("- One shot Setting")]

        [Tooltip("Do not process the particle by thread, by main thread.")]
        [SerializeField]
        private bool mDoShotImmediately = false;

        // check if the particle spawned.
        private bool mParticleSpawned = false;


        [Header("- Other Setting")]

        [Tooltip("Set the particles as child?")]
        [SerializeField]
        private bool mSetChild = true;

        [Tooltip("Everytime active the particle set the position to this transform position.")]
        [SerializeField]
        private bool mSetToSamePositionWhenActive = true;

        //-- Thread
        private JCS_Vector<int> mThread = null;
        private JCS_Vector<float> mTimers = null;           // timer per thread
        private JCS_Vector<int> mParticleCount = null;         // how many shoot should process per thread
        private JCS_Vector<int> mParticleCounter = null;         // counter per thread


        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public JCS_Vector<JCS_Particle> GetParticles() { return this.mParticles; }

        // Binds.
        public bool Active { get { return this.mActive; } set { this.mActive = value; } }
        public bool ActiveThread { get { return this.mActiveThread; } set { this.mActiveThread = value; } }
        public int OrderLayer { get { return this.mOrderLayer; } set { this.mOrderLayer = value; } }
        public int NumOfParticle { get { return this.mNumOfParticle; } set { this.mNumOfParticle = value; } }
        public JCS_Particle Particle { get { return this.mParticle; } set { this.mParticle = value; } }
        public int Density { get { return this.mDensity; } set { this.mDensity = value; } }
        public float WindSpeed { get { return this.mWindSpeed; } set { this.mWindSpeed = value; } }
        public bool AlwaysTheSameScale { get { return this.mAlwaysTheSameScale; } set { this.mAlwaysTheSameScale = value; } }
        public bool SetChild { get { return this.mSetChild; } set { this.mSetChild = value; } }
        public bool SetToSamePositionWhenActive { get { return this.mSetToSamePositionWhenActive; } set { this.mSetToSamePositionWhenActive = value; } }
        public bool FreezeX { get { return this.mFreezeX; } set { this.mFreezeX = value; } }
        public bool FreezeY { get { return this.mFreezeY; } set { this.mFreezeY = value; } }
        public bool FreezeZ { get { return this.mFreezeZ; } set { this.mFreezeZ = value; } }
        public float RandPosX { get { return this.mRandPosX; } set { this.mRandPosX = value; } }
        public float RandPosY { get { return this.mRandPosY; } set { this.mRandPosY = value; } }
        public float RandPosZ { get { return this.mRandPosZ; } set { this.mRandPosZ = value; } }
        public float RandAngleX { get { return this.mRandAngleX; } set { this.mRandAngleX = value; } }
        public float RandAngleY { get { return this.mRandAngleY; } set { this.mRandAngleY = value; } }
        public float RandAngleZ { get { return this.mRandAngleZ; } set { this.mRandAngleZ = value; } }
        public float RandScaleX { get { return this.mRandScaleX; } set { this.mRandScaleX = value; } }
        public float RandScaleY { get { return this.mRandScaleY; } set { this.mRandScaleY = value; } }
        public float RandScaleZ { get { return this.mRandScaleZ; } set { this.mRandScaleZ = value; } }
        public bool DoShotImmediately { get { return this.mDoShotImmediately; } set { this.mDoShotImmediately = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mParticles = new JCS_Vector<JCS_Particle>();

            mThread = new JCS_Vector<int>();
            mTimers = new JCS_Vector<float>();
            mParticleCount = new JCS_Vector<int>();
            mParticleCounter = new JCS_Vector<int>();

            mFreezePos = this.transform.position;
        }

        private void Start()
        {
            SpawnParticles();
        }

        private void Update()
        {
#if (UNITY_EDITOR)
            Test();
#endif

            if (mActiveThread)
                ProccessSequences();

            // check if this particle engine active?
            if (!mActive)
                return;

            DoParticle();

            Freeze();
        }

#if (UNITY_EDITOR)
        private void Test()
        {
            if (JCS_Input.GetKeyDown(KeyCode.C))
            {
                StartActive();
            }

            if (JCS_Input.GetKeyDown(KeyCode.V))
            {
                StopActive();
            }

            if (JCS_Input.GetKeyDown(KeyCode.X))
            {
                PlayOneShot(10);
            }
        }
#endif

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Start the paritcle engine.
        /// </summary>
        public void StartActive()
        {
            mActive = true;
        }

        /// <summary>
        /// Stop the paritcle engine.
        /// </summary>
        public void StopActive()
        {
            mActive = false;
        }

        /// <summary>
        /// Active some particle in certain number current frame.
        /// </summary>
        /// <param name="num"> number to spawn the particle. </param>
        public void PlayOneShot()
        {
            PlayOneShot(mDensity);
        }

        /// <summary>
        /// Active some particle in certain number current frame.
        /// </summary>
        /// <param name="num"> number to spawn the particle. </param>
        public void PlayOneShot(int num)
        {
            if (mParticle == null)
                return;

            if (mDoShotImmediately)
            {
                for (int count = 0;
                    count < num;
                    ++count)
                {
                    JCS_Particle particle = SearchAvaliableParticles();

                    if (particle == null)
                        continue;

                    SetParticleBySetting(particle);
                }
            }
            else
            {
                for (int count = 0;
                    count < num;
                    ++count)
                {
                    DoSequenceParticle(mDensity);
                }
            }
        }

        /// <summary>
        /// Check if no particle running?
        /// </summary>
        /// <returns>
        /// true : no thread running.
        /// false : vice versa.
        /// </returns>
        public bool IsParticleEnd()
        {
            return (this.mThread.length == 0);
        }

        /// <summary>
        /// Spawn all particle base on the count.
        /// </summary>
        public void SpawnParticles()
        {
            // check if particles already spawned?
            if (mParticleSpawned)
                return;

            if (mParticle == null)
            {
                JCS_Debug.Log(
                    "No particle assign!");
                return;
            }

            for (int index = 0;
                index < mNumOfParticle;
                ++index)
            {
                JCS_Particle trans = (JCS_Particle)JCS_Utility.SpawnGameObject(mParticle);
                mParticles.push(trans);

                // disable the object
                trans.gameObject.SetActive(false);

                if (mSetChild)
                {
                    // set parent
                    trans.transform.SetParent(this.transform);
                }
            }

            mParticleSpawned = true;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Freee the paritcle engine position?
        /// </summary>
        private void Freeze()
        {
            Vector3 newPos = this.transform.position;

            if (mFreezeX)
                newPos.x = mFreezePos.x;
            if (mFreezeY)
                newPos.y = mFreezePos.y;
            if (mFreezeZ)
                newPos.z = mFreezePos.z;

            this.transform.position = newPos;
        }

        /// <summary>
        /// Do the particle algorithm here.
        /// </summary>
        private void DoParticle()
        {
            if (mParticle == null)
                return;

            mSequenceTimer += Time.deltaTime;

            // NOTE(jenchieh): magic number...
            float time = JCS_Random.Range(0.5f, 1.0f);

            if (time < mSequenceTimer)
            {
                // reset timer
                mSequenceTimer = 0;

                // do a sequence
                DoSequenceParticle(mDensity);
            }
        }

        /// <summary>
        /// Spawn a thread in order to get process.
        /// </summary>
        /// <param name="density"> density for the particle effect. </param>
        private void DoSequenceParticle(int density)
        {
            // thread itself
            mThread.push(mThread.length);

            // needed data
            mTimers.push(0);
            mParticleCount.push(density);
            mParticleCounter.push(0);
        }

        /// <summary>
        /// Find the avaliable particle in the list.
        /// </summary>
        /// <returns> non-active paritcle. </returns>
        private JCS_Particle SearchAvaliableParticles(bool sec = false)
        {
            if (mNumOfParticle <= 0)
            {
                JCS_Debug.LogError(
                    "Number of particle cannot lower or equal to zero...");
                return null;
            }

            for (int index = mLastAvaliableIndex;
                index < mParticles.length;
                ++index)
            {
                JCS_Particle particle = mParticles.at(index);
                bool isActive = particle.gameObject.activeInHierarchy;
                if (isActive == false)
                {
                    particle.gameObject.SetActive(true);
                    mLastAvaliableIndex = index;
                    return particle;
                }
            }

            // if we get here mean we cycle once but we
            // did not spawn a text!
            // so reset the spawn pos and 
            // try to search agian until we find one!
            mLastAvaliableIndex = 0;

            // if second time still does not find, return null.
            // prevent stack overflow.
            if (sec)
                return null;

            // dangerious, use carefully!
            // make sure u have enough number of handle
            // or else the program might crash? (too many delay?)
            return SearchAvaliableParticles(true);
        }

        /// <summary>
        /// Loop through thread list and process it.
        /// </summary>
        private void ProccessSequences()
        {
            for (int processIndex = 0;
                processIndex < mThread.length;
                ++processIndex)
            {
                // process all the thread
                Sequence(processIndex);
            }
        }

        /// <summary>
        /// Process the thread.
        /// </summary>
        /// <param name="processIndex"> thread id </param>
        private void Sequence(int processIndex)
        {
            // get the timer from the thread
            float newTimer = mTimers.at(processIndex);

            // add time to timer
            newTimer += Time.deltaTime;

            // check if we can do the particle or not
            if (mTimeAParticle < newTimer)
            {
                int totalParticleCount = mParticleCount.at(processIndex);
                int currentParticleCount = mParticleCounter.at(processIndex);
                if (currentParticleCount == totalParticleCount)
                {
                    // Remove Thread.
                    EndProcessSequence(processIndex);
                    return;
                }

                // find a particle
                JCS_Particle particle = SearchAvaliableParticles();

                if (particle == null)
                {
                    // exit function.
                    return;
                }

                SetParticleBySetting(particle);

                ++currentParticleCount;

                // update new count, in order 
                // to spawn next bullet
                mParticleCounter.set(processIndex, currentParticleCount);
                newTimer = 0;
            }

            // update timer
            mTimers.set(processIndex, newTimer);
        }

        /// <summary>
        /// Delete the thead when done processing.
        /// </summary>
        /// <param name="processIndex"> thread id </param>
        private void EndProcessSequence(int processIndex)
        {
            mThread.slice(processIndex);

            mTimers.slice(processIndex);
            mParticleCount.slice(processIndex);
            mParticleCounter.slice(processIndex);
        }

        /// <summary>
        /// Make a particle fit the setting.
        /// </summary>
        /// <param name="particle"> particle to change and fit the setting. </param>
        private void SetParticleBySetting(JCS_Particle particle)
        {
            // get this particle info.
            Vector3 newPos = particle.transform.position;

            if (mSetToSamePositionWhenActive)
            {
                // set position to the same position as particle 
                // system's position.
                particle.transform.position = this.transform.position;
            }

            // after we have set the position to current 
            // particle system's position. Next step is to 
            // apply the random value to the newer position.
            newPos.x += JCS_Random.Range(-mRandPosX, mRandPosX);
            newPos.y += JCS_Random.Range(-mRandPosY, mRandPosY);
            newPos.z += JCS_Random.Range(-mRandPosZ, mRandPosZ);
            particle.transform.position = newPos;

            // set rotation depend on wind speed
            /**
             * NOTE(jenchieh): 
             *                      ------- Set the scale from 
             *                      ↓      the original particle, 
             *                      ↓      so the euler angles won't 
             *                  ↓-------↓  get stack up.              */
            Vector3 newRot = mParticle.transform.localEulerAngles;
            newRot.z = mWindSpeed;

            // apply wind speed.
            newRot.z += JCS_Random.Range(-mRandAngleX, mRandAngleX);

            // apply random rotation.
            newRot.x += JCS_Random.Range(-mRandAngleX, mRandAngleX);
            newRot.y += JCS_Random.Range(-mRandAngleY, mRandAngleY);
            newRot.z += JCS_Random.Range(-mRandAngleZ, mRandAngleZ);
            particle.transform.localEulerAngles = newRot;

            /**
             * NOTE(jenchieh): 
             *                      ------- Set the scale from 
             *                      ↓      the original particle, 
             *                      ↓      so the scale won't 
             *                  ↓-------↓  get stack up.              */
            Vector3 newScale = mParticle.transform.localScale;
            newScale.x += JCS_Random.Range(-mRandScaleX, mRandScaleX);
            newScale.y += JCS_Random.Range(-mRandScaleY, mRandScaleY);
            newScale.z += JCS_Random.Range(-mRandScaleZ, mRandScaleZ);
            if (mAlwaysTheSameScale)
            {
                // set the scale the same.
                newScale.y = newScale.x;
                newScale.z = newScale.x;
            }
            particle.transform.localScale = newScale;
        }

    }
}
