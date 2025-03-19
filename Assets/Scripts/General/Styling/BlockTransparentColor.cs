using UnityEngine;
using UnityEngine.UI;

public class BlockTransparentColor : MonoBehaviour
{
    public Image image;


    void Start()
    {
        image.color = new Color(0.45f, 0.45f, 0.45f, 0.8f);
    }
}
