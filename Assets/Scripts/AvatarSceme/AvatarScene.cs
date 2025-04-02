using UnityEngine;
using UnityEngine.SceneManagement;

public class AvatarScene : MonoBehaviour
{
    public GameObject IconContainer;
    public GameObject SelectedIconPrefab; // Reference to the SelectedIcon prefab
    private GameObject selectedIconInstance;

    void Start()
    {
        PlayerPrefs.SetInt("avatar_ID", -3080);

        int savedAvatarId = PlayerPrefs.GetInt("avatar_ID", -1); // Get the saved avatar ID from PlayerPrefs

        if (savedAvatarId != -1)
        {
            foreach (Transform child in IconContainer.transform)
            {
                if (child.gameObject.GetInstanceID() == savedAvatarId)
                {
                    PlaceSelectedIconInMiddle(child.gameObject);
                    break;
                }
            }
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
        // Save the prefab ID (this could be to PlayerPrefs, a file, etc.)
        PlayerPrefs.SetInt("avatar_ID", id);
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

    public void OnHomeBtn()
    {
       SceneManager.LoadScene("RoadMapScene");
    }
}
