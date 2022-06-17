using UnityEngine;
using UnityEngine.UI;

public class LUI_UIAnimManager : MonoBehaviour {

	public bool isToggle = false;

	[Header("ANIMATORS")]
	public Animator oldAnimator;
	public Animator newAnimator;

	[Header("OBJECTS")]
	public Button animButton;
	public Toggle animToggle;
	public GameObject newPanel;

	[Header("ANIM NAMES")]
	public string oldAnimText;
	public string newAnimText;

    private void Awake()
    {
		oldAnimator.GetComponent<Animator>();
		newAnimator.GetComponent<Animator>();
	}

    private void OnEnable()
    {
		if (!isToggle)
			animButton.onClick.AddListener(TaskOnClick);
		else
			animToggle.onValueChanged.AddListener(CheckOnClick);
	}

    private void OnDisable()
    {
		if (!isToggle)
			animButton.onClick.RemoveListener(TaskOnClick);
		else
			animToggle.onValueChanged.RemoveListener(CheckOnClick);
	}

    private void CheckOnClick(bool arg0)
    {
		if (arg0)
			TaskOnClick();
    }

    void TaskOnClick()
	{
		newPanel.SetActive(true);
		oldAnimator.Play(oldAnimText);
		newAnimator.Play(newAnimText);
	}
}