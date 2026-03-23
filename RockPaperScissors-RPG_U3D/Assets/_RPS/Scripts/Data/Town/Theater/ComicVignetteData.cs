using System;
using UnityEngine;

namespace Kapibara.RPS
{
	/// <summary>
	/// Datos de una viñeta individual dentro de una página de comic.
	/// Serializado inline en ComicPageSO — no es ScriptableObject.
	/// </summary>
	[Serializable]
	public class ComicVignetteData
	{
		[SerializeField] private Sprite _sprite;
		[SerializeField] private string _dialogText;
		[SerializeField] private VignetteAnimation _animation;

		public Sprite Sprite => _sprite;
		public string DialogText => _dialogText;
		public VignetteAnimation Animation => _animation;
	}
}
