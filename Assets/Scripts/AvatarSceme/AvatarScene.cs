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
        string savedAvatarName = PlayerPrefs.GetString("avatar_ID", string.Empty); // Get the saved avatar name from PlayerPrefs

        if (!string.IsNullOrEmpty(savedAvatarName))
        {
            Debug.Log($"Saved avatar found! {savedAvatarName}");
            HomeButton.gameObject.SetActive(true);
            foreach (Transform child in IconContainer.transform)
            {
                if (child.gameObject.name == savedAvatarName)
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
        string avatarName = avatarImage.name;
        SavePrefabName(avatarName);
        Debug.Log($"Saving avatar {avatarName}");
        PlaceSelectedIconInMiddle(avatarImage);
    }

    public void SavePrefabName(string name)
    {
        PlayerPrefs.SetString("avatar_ID", name);
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
