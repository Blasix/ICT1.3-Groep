using UnityEngine;
using UnityEngine.SceneManagement;

public class AvatarScene : MonoBehaviour
{
    public GameObject IconContainer;
    public GameObject SelectedIconPrefab; // Reference to the SelectedIcon prefab
    private GameObject selectedIconInstance;

    public Transform HomeButton;

    void Start()
    {
        int savedAvatarId = PlayerPrefs.GetInt("avatar_ID", -1); // Get the saved avatar ID from PlayerPrefs
        
        if (savedAvatarId != -1)
        {
            Debug.Log($"saved avatar found! {savedAvatarId}");
            HomeButton.gameObject.SetActive(true);
            foreach (Transform child in IconContainer.transform)
            {
                if (child.gameObject.GetInstanceID() == savedAvatarId)
                {
                    PlaceSelectedIconInMiddle(child.gameObject);
                    break;
                }
            }
        }

        else
        {
            HomeButton.gameObject.SetActive(false);
        }
    }


    public void OnAvatarImageClick(GameObject avatarImage)
    {
        int instanceId = avatarImage.GetInstanceID();
        SavePrefabId(instanceId);
        PlaceSelectedIconInMiddle(avatarImage);
    }

    public void SavePrefabId(int id)
    {
        PlayerPrefs.SetInt("avatar_ID", id);
        PlayerPrefs.Save();
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
