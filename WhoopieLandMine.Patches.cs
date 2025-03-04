﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace WhoopieLandMine
{
    [HarmonyPatch(typeof(GrabbableObject))]
    public class WhoopieLandMinePatches
    {

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void Postfix(GrabbableObject __instance)
        {
            Debug.Log("GrabbableObject Patch Loading...");

            if (__instance is WhoopieCushionItem)
            {

                Transform triggerTransform = __instance.transform.Find("Trigger");
                if (triggerTransform != null)
                {

                    GameObject triggerObject = triggerTransform.gameObject;

                    if (triggerObject.GetComponent<WhoopieScript>() == null)
                    {
                        triggerObject.gameObject.AddComponent<WhoopieScript>();
                        Debug.Log("WhoopieScript Attached to child");
                    }
                }
                else
                {
                    Debug.LogError("Child object named 'Trigger' not found");
                }


            }

        }

                [HarmonyPatch(typeof(WhoopieCushionTrigger))]
        public static class WhoopieCushionTriggerPatch
        {
            public static event EventHandler<WhoopieEventArgs> OnWhoopieTrigger;

            [HarmonyPatch("OnTriggerEnter")]
            [HarmonyPostfix]
            private static void Postfix(WhoopieCushionTrigger __instance, Collider other)
            {
                if (!__instance.itemScript.isHeld && (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy")))
                {
                    OnWhoopieTrigger?.Invoke(__instance, new WhoopieEventArgs { collider = other });
                }

            }


        }

        public class WhoopieEventArgs : EventArgs
        {
            public Collider collider { get; set; }
        }


    }
}
