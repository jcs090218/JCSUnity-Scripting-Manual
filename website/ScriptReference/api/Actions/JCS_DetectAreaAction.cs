﻿/**
 * $File: JCS_DetectAreaAction.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace JCSUnity
{
    /// <summary>
    /// Anything with can dectect area must be living thing.
    /// </summary>
    [RequireComponent(typeof(JCS_2DLiveObject))]
    public class JCS_DetectAreaAction
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Check Variables **")]
        [SerializeField] private JCS_2DLiveObject mLiveObject = null;

        [Header("** Initialize Variables **")]
        [SerializeField] private Collider[] mDetectCollider = null;
        private JCS_Vector<JCS_DetectAreaObject> mDetectedObjects = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public JCS_2DLiveObject GetLiveObject() { return this.mLiveObject; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mLiveObject = this.GetComponent<JCS_2DLiveObject>();

            if (mDetectCollider.Length == 0)
            {
                JCS_Debug.LogReminders(
                    "No Collider assing for dectetion...");
            }


            for (int index = 0;
                index < mDetectCollider.Length;
                ++index)
            {

                if (mDetectCollider[index] == null)
                {
                    JCS_Debug.LogReminders(
                        "No Collider assing for dectetion...");

                    continue;
                }

                // let the detect area know his parent class (this)
                JCS_DetectArea da = mDetectCollider[index].gameObject.AddComponent<JCS_DetectArea>();
                da.SetAction(this);

                // force the collider equals to true!!
                mDetectCollider[index].isTrigger = true;
            }

            // create list to manage all detected object
            mDetectedObjects = new JCS_Vector<JCS_DetectAreaObject>();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// If detect a Target add to list
        /// </summary>
        /// <param name="jcsDo"></param>
        public void AddDetectedObject(JCS_DetectAreaObject jcsDo)
        {
            if (jcsDo == null)
                return;

            mDetectedObjects.push(jcsDo);
        }
        /// <summary>
        /// If detected Target leave the area,
        /// remove from list!
        /// </summary>
        /// <param name="jcsDo"></param>
        public void RemoveDetectedObject(JCS_DetectAreaObject jcsDo)
        {
            if (jcsDo == null)
                return;

            mDetectedObjects.slice(jcsDo);
        }

        /// <summary>
        /// Return the furthest object in the array.
        /// </summary>
        /// <returns></returns>
        public JCS_DetectAreaObject FindTheFurthest()
        {
            int furthestIndex = -1;

            float furthestDistance = -1;

            bool theFirstAssign = true;

            for (int index = 0;
                index < mDetectedObjects.length;
                ++index)
            {
                JCS_DetectAreaObject obj = mDetectedObjects.at(index);
                if (mDetectedObjects.at(index) == null)
                {
                    // remove from the list, 
                    // the object could be dead for some reason.
                    mDetectedObjects.slice(index);
                    return null;
                }

                // check to see if the object is live object.
                JCS_2DLiveObject liveObj = obj.GetComponent<JCS_2DLiveObject>();
                if (liveObj != null)
                {
                    // cannot target object that cannot be damage.
                    if (!liveObj.CanDamage)
                        continue;
                }

                Vector3 objectPos = mDetectedObjects.at(index).transform.position;
                Vector3 areaPos = this.transform.position;

                float distance = Vector3.Distance(objectPos, areaPos);

                // assign the first value!
                if (theFirstAssign)
                {
                    furthestDistance = distance;
                    furthestIndex = index;
                    theFirstAssign = false;
                    continue;
                }

                if (distance > furthestDistance)
                {
                    furthestDistance = distance;
                    furthestIndex = index;
                }

            }

            // nothing found or nothing in the list(nothing detected)!
            if (theFirstAssign)
                return null;

            // return result
            return mDetectedObjects.at(furthestIndex);
        }
        /// <summary>
        /// Return the closest object in the array.
        /// </summary>
        /// <returns></returns>
        public JCS_DetectAreaObject FindTheClosest()
        {
            int closestIndex = -1;

            float closestDistance = -1;

            bool theFirstAssign = true;

            for (int index = 0;
                index < mDetectedObjects.length;
                ++index)
            {
                JCS_DetectAreaObject obj = mDetectedObjects.at(index);
                if (mDetectedObjects.at(index) == null)
                {
                    // remove from the list, 
                    // the object could be dead for some reason.
                    mDetectedObjects.slice(index);
                    return null;
                }

                // check to see if the object is live object.
                JCS_2DLiveObject liveObj = obj.GetComponent<JCS_2DLiveObject>();
                if (liveObj != null)
                {
                    // cannot target object that cannot be damage.
                    if (!liveObj.CanDamage)
                        continue;
                }

                Vector3 objectPos = obj.transform.position;
                Vector3 areaPos = this.transform.position;

                float distance = Vector3.Distance(objectPos, areaPos);

                // assign the first value!
                if (theFirstAssign)
                {
                    closestDistance = distance;
                    closestIndex = index;
                    theFirstAssign = false;
                    continue;
                }

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = index;
                }

            }

            // nothing found or nothing in the list(nothing detected)!
            if (theFirstAssign)
                return null;

            // return result
            return mDetectedObjects.at(closestIndex);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
