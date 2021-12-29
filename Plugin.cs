using BepInEx;
using BepInEx.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using APIPlugin;

namespace FuckSacrificeRestrictions
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "splashyflame.inscryption.fucksacrificerestrictions";
        private const string PluginName = "Fuck Sacrifice Restrictions";
        private const string PluginVersion = "1.0.0.0";

        private void Awake()
        {
            Logger.LogInfo($"Loaded {PluginName}!");
            Logger.LogInfo($"[FSR]: Applying Harmony Patches...");
            var harmony = new Harmony("SplashyFlame.Inscryption.FuckSacrificeRestrictions");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(CardMergeSequencer), "GetValidCardsForSacrifice")]
        public class Patch1
        {
            [HarmonyPrefix]
            public static bool Prefix(ref List<CardInfo> __result, CardMergeSequencer __instance, CardInfo host = null)
            {
                List<CardInfo> list = new List<CardInfo>(RunState.DeckList);
	            list.RemoveAll((CardInfo x) => x.NumAbilities == 0);
	            if (host != null)
	            {
		            list.RemoveAll((CardInfo s) => !__instance.SacrificeOffersNewAbility(host, s));
	            }
	            __result = list;
                return false;
            }

        }

        [HarmonyPatch(typeof(CardMergeSequencer), "GetValidCardsForHost")]
        public class Patch2
        {
            [HarmonyPrefix]
            public static bool Prefix(ref List<CardInfo> __result, CardMergeSequencer __instance, CardInfo sacrifice = null)
            {
                List<CardInfo> list = new List<CardInfo>(RunState.DeckList);
	            if (sacrifice != null)
	            {
		            list.RemoveAll((CardInfo h) => !__instance.SacrificeOffersNewAbility(h, sacrifice));
	            }
	            __result = list;
                return false;
            }
        }
    }
}
