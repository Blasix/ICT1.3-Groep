using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;

public class AvatarScene : MonoBehaviour
{
    public GameObject IconContainer;
    public GameObject SelectedIconPrefab; // Reference to the SelectedIcon prefab
    private GameObject selectedIconInstance;
    public GameObject Avatar1;
    public GameObject Avatar2;
    public GameObject Avatar3;
    public GameObject Avatar4;

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
                    PlaceSelectedIconInMiddle(avatar);
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
        HomeButton.gameObject.SetActive(true);
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
