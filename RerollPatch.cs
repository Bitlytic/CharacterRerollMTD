using HarmonyLib;
using flanne;
using flanne.Core;
using UnityEngine;
using System.Collections.Generic;

namespace CharacterRerollPlugin.Patch 
{
    internal static class RerollPatch
    {
        static AccessTools.FieldRef<ChestState, Powerup> powerupRef = AccessTools.FieldRefAccess<ChestState, Powerup>("_currPowerup");
        static AccessTools.FieldRef<PowerupGenerator, List<PowerupPoolItem>> characterPoolRef = AccessTools.FieldRefAccess<PowerupGenerator, List<PowerupPoolItem>>("characterPool");

        static GameController controllerInstance;

        static List<PowerupPoolItem> characterPool = new List<PowerupPoolItem>();

        [HarmonyPatch(typeof(PlayerController), "Start")]
        [HarmonyPrefix]
        static void Start(PlayerController __instance)
        {
            controllerInstance = Object.FindObjectOfType<GameController>();
            characterPool = characterPoolRef(controllerInstance.powerupGenerator);
        }

        [HarmonyPatch(typeof(PlayerController), "Update")]
        [HarmonyPrefix]
        static void Update(PlayerController __instance)
        {
            if (controllerInstance.CurrentState is ChestState)
            {
                if (__instance.playerInput.currentActionMap.FindAction("Pause").triggered)
                {
                    RerollUpgrade();
                }
            }
        }

        static void RerollUpgrade()
        {
            List<Powerup> randomCharacterProfile = controllerInstance.powerupGenerator.GetRandomCharacterProfile(1);
            var r = powerupRef((ChestState)controllerInstance.CurrentState);

            int currentIndex = 0;
            for(int i = 0; i < characterPool.Count; i++)
            {
                if (characterPool[i].powerup.nameString.Equals(r.nameString))
                {
                    currentIndex = (i + 1) % characterPool.Count;
                    break;
                }
            }

            r = powerupRef((ChestState)controllerInstance.CurrentState) = characterPool[currentIndex].powerup;
            controllerInstance.chestUIController.SetToPowerup(r);
        }
    }
}