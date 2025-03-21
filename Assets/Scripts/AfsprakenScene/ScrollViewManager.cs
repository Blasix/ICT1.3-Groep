using UnityEngine;
using UnityEngine.UI;

public class ScrollViewManager : MonoBehaviour
{
    public GameObject itemPrefab; // Assign your prefab in the Inspector
    public Transform contentParent; // Assign the Content object in the Inspector
    public int numberOfItems = 10; // Number of items to add
    public float spacing = 10f; // Spacing between items

    void Start()
    {
        float itemHeight = itemPrefab.GetComponent<RectTransform>().rect.height;
        for (int i = 0; i < numberOfItems; i++)
        {
            GameObject newItem = Instantiate(itemPrefab, contentParent);
            RectTransform rectTransform = newItem.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, -i * (itemHeight + spacing));
        }

        // Adjust the Content size
        RectTransform contentRect = contentParent.GetComponent<RectTransform>();
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, numberOfItems * (itemHeight + spacing));
    }
}