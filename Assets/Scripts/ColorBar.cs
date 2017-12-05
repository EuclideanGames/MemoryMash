using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ColorBar : MonoBehaviour
{
    public Image ColorImagePrefab;
    public List<Image> ColorImages;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

	private void Start()
	{
		
	}
	
	private void Update()
	{
		
	}

    public void DisplayColorList(List<Color> colors)
    {
        foreach (RectTransform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Color color in colors)
        {
            Image colorImage = Instantiate(ColorImagePrefab);
            colorImage.transform.SetParent(transform, false);
            ColorImages.Add(colorImage);

            colorImage.color = color;
        }
    }

    public void Show()
    {
        canvasGroup.Show();
    }

    public void Hide()
    {
        StartCoroutine(canvasGroup.FadeOut(2.0f));
    }
}
