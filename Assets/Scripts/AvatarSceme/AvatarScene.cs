using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;

public class AvatarScene : MonoBehaviour
{
    public GameObject IconContainer;
    public GameObject SelectedIconPrefab; // Reference to the SelectedIcon prefab
    private GameObject selectedIconInstance;

    void Start()
    {

        if (PlayerPrefs.HasKey("avatar_ID"))
        {
            int avatarId = PlayerPrefs.GetInt("avatar_ID");
            Debug.Log("Retrieved avatar_ID: " + avatarId);

            if (avatarId != 0)
            {
                GameObject found = FindInstanceInContainer(IconContainer.transform, avatarId);

                if (found != null)
                {
                    PlaceSelectedIconInMiddle(found);
                }
            }
        }
        else
        {
            Debug.LogWarning("avatar_ID key not found in PlayerPrefs.");
        }
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
        ApiClient api = new ApiClient();
        string childId = PlayerPrefs.GetString("SelectedChildId");
        api.UpdateChild(childId, id);
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

    private GameObject FindInstanceInContainer(Transform container, int instanceId)
    {
        foreach (Transform child in container)
        {
            if (child.gameObject.GetInstanceID() == instanceId)
            {
                return child.gameObject;
            }
        }
        return null;
    }

    public void OnHomeBtn()
    {
       SceneManager.LoadScene("RoadMapScene");
    }
}
