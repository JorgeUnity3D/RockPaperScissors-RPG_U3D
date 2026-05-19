using Sirenix.OdinInspector;
using UnityEngine;

namespace Kapibara.RPS
{
    [System.Serializable]
    public class LanguageAtlas
    {
        [HorizontalGroup("LanguageAtlas", MaxWidth = 150)] [EnumPaging] [HideLabel]
        public Actions word;

        [HorizontalGroup("LanguageAtlas", MaxWidth = 150)] [PreviewField] [HideLabel]
        public Sprite icon;
    }
}
