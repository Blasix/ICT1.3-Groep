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

    void Start()
    {
        if (PlayerPrefs.HasKey("avatar_ID"))
        {
            int avatarId = PlayerPrefs.GetInt("avatar_ID");
            Debug.Log("Retrieved avatar_ID: " + avatarId);

            if (avatarId >= 1 && avatarId <= 4)
            {
                GameObject avatar = GetAvatarById(avatarId);
                if (avatar != null)
                {
                    PlaceSelectedIconInMiddle(avatar);
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
        int avatarId = GetAvatarId(avatarImage);
        if (avatarId != -1)
        {
            SaveAvatarId(avatarId);
            LogChildUpdateWithAvatarId(avatarId);
            PlaceSelectedIconInMiddle(avatarImage);
        }
    }

    private int GetAvatarId(GameObject avatarImage)
    {
        if (avatarImage == Avatar1) return 1;
        if (avatarImage == Avatar2) return 2;
        if (avatarImage == Avatar3) return 3;
        if (avatarImage == Avatar4) return 4;
        return -1;
    }

    private GameObject GetAvatarById(int id)
    {
        switch (id)
        {
            case 1: return Avatar1;
            case 2: return Avatar2;
            case 3: return Avatar3;
            case 4: return Avatar4;
            default: return null;
        }
    }

    public void SaveAvatarId(int id)
    {
        ApiClient api = new ApiClient();
        string childId = PlayerPrefs.GetString("SelectedChildId");
        api.UpdateChild(childId, id);
        PlayerPrefs.SetInt("avatar_ID", id);
        PlayerPrefs.Save();
    }

    private void LogChildUpdateWithAvatarId(int id)
    {
        Debug.Log("Child updated with avatar ID: " + id);
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
