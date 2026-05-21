using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS {
    [Serializable]
    public class Language {
        [EnumPaging] [HideLabel]
        public Languages language;
        [TableList(ShowIndexLabels = false, AlwaysExpanded = true)] [HideLabel] 
        public List<LanguageAtlas> words;

        public Sprite GetActionIcon(Actions targetAction) {
            LanguageAtlas entry = words.Find(la => la.word == targetAction);
            if (entry == null) { Debug.LogWarning($"[Language] No icon for {targetAction} in {language}"); return null; }
            return entry.icon;
        }
    }

}