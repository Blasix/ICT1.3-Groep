using UnityEngine;
using UnityEngine.UI;

public class SelectOneAvatar : MonoBehaviour
{
    public GameObject SelectedIconPrefab; // Reference to the SelectedIcon prefab
    private GameObject selectedIconInstance;

    void Start()
    {

    }

    void Update()
    {

    }

    public void OnAvatarImageClick(GameObject avatarImage)
    {
        int instanceId = avatarImage.GetInstanceID();
        SavePrefabId(instanceId);
        LogChildUpdateWithPrefabId(instanceId);
        PlaceSelectedIconInMiddle(avatarImage);
    }

    public void SavePrefabId(int id)
    {
        // Save the prefab ID (this could be to PlayerPrefs, a file, etc.)
        PlayerPrefs.SetInt("SelectedPrefabId", id);
        PlayerPrefs.Save();
    }

    private void LogChildUpdateWithPrefabId(int id)
    {
        Debug.Log("Child updated with prefab ID: " + id);
    }

    public void PlaceSelectedIconInMiddle(GameObject avatarImage)
    {
        if (selectedIconInstance == null)
        {
            selectedIconInstance = Instantiate(SelectedIconPrefab, avatarImage.transform);
        }

        // Set the SelectedIcon's position to the avatarImage's center
        selectedIconInstance.transform.SetParent(avatarImage.transform, false);
        selectedIconInstance.transform.localPosition = Vector3.zero;
    }
}
