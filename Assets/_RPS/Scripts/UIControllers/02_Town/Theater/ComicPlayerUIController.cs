using UnityEngine;
using UnityEngine.UI;

public class ComicPlayerUIController : MonoBehaviour
{
	//[SerializeField] private List<UIContainer> _vignettesCont;
	//[SerializeField] private List<UIContainerUIAnimator> _vignettes;
	[SerializeField] private Button _resetButton;
	[SerializeField] private Button _nextButton;
	private int _currentVignette;


	private void Awake()
	{
		//_currentVignette = -1;
		//_vignettesCont.ForEach(vignette => { vignette.InstantHide(); });
		//_nextButton.AddListener(NextVignette);
		//_resetButton.AddListener(ResetComic);

	}

	private void ResetComic()
	{
		//foreach (UIContainer vignette in _vignettesCont)
		//{
		//	vignette.Hide();
		//}
		
		//_currentVignette = -1;
	}

	private void NextVignette()
	{
		//_currentVignette++;
		//if (_currentVignette >= _vignettesCont.Count)
		//{
		//	return;
		//}
		//_vignettesCont[_currentVignette].Show();		
	}
}
