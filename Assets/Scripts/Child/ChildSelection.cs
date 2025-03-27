using System.Collections.Generic;
using System.Threading.Tasks;
using Items;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Child
{
    public class ChildSelection : MonoBehaviour
    {
        public void OnChildCreationButtonClick()
        {
            SceneManager.LoadScene("ChildCreationScene");
        }
        
        public GameObject itemPrefab; // Prefab for the list item
        public Transform contentParent; // Content object of the ScrollView
        private List<ChildItem> _childrenList;
        void Start()
        {
            LoadChildren();
        }

        private async void LoadChildren()
        {
            _childrenList = await GetChildren();
            AddItems(_childrenList);
        }

        private async Task<List<ChildItem>> GetChildren()
        {
            _childrenList = JsonConvert.DeserializeObject<List<ChildItem>>(
                await ApiClient.PerformApiCall(ApiClient.apiurl + "/child", "GET"));
            return _childrenList;
        }

        void AddItems(List<ChildItem> children)
        {
            // Check if prefab and content parent are assigned
            if (itemPrefab == null || contentParent == null)
            {
                Debug.LogError("Prefab or Content Parent is not assigned!");
                return;
            }
        
            // Loop through the list of appointments
            foreach (var child in children)
            {
                // Instantiate the prefab
                GameObject newItem = Instantiate(itemPrefab, contentParent);
        
                // Set the text values
                TMP_Text nameText = newItem.transform.Find("ButtonSelectChild/TMP_TextName")?.GetComponent<TMP_Text>();
                TMP_Text idText = newItem.transform.Find("ButtonSelectChild/TMP_TextId")?.GetComponent<TMP_Text>();
                
                if (nameText != null)
                {
                    nameText.text = child.Name;
                }
                else
                {
                    Debug.LogError("TMP_TextName not found in itemPrefab");
                }
                if (idText != null)
                {
                    idText.text = child.Id;
                }
                else
                {
                    Debug.LogError("TMP_TextId not found in itemPrefab");
                }
        
                // Set up the delete button
                Button deleteButton = newItem.transform.Find("ButtonDeleteChild")?.GetComponent<Button>();
                if (deleteButton != null)
                {
                    deleteButton.onClick.AddListener(() => DeleteItem(newItem));
                }
                else
                {
                    Debug.LogError("DeleteButton not found in itemPrefab");
                }
                
                // Set up the select button
                Button selectButton = newItem.transform.Find("ButtonSelectChild")?.GetComponent<Button>();
                if (selectButton != null)
                {
                    selectButton.onClick.AddListener(() => SelectItem(newItem));
                }
                else
                {
                    Debug.LogError("SelectButton not found in itemPrefab");
                }
        
                // Make the new item active
                newItem.SetActive(true);
            }
        }

        public void SelectItem(GameObject item)
        {
            // Find the text components
            TMP_Text nameText = item.transform.Find("ButtonSelectChild/TMP_TextName")?.GetComponent<TMP_Text>();
            TMP_Text idText = item.transform.Find("ButtonSelectChild/TMP_TextId")?.GetComponent<TMP_Text>();
            
            if (idText != null && !string.IsNullOrEmpty(idText.text) && nameText != null && !string.IsNullOrEmpty(nameText.text))
            {
                // Retrieve the text values
                string childName = nameText.text;
                string childId = idText.text;
            
                // Log the text values (or use them as needed)
                Debug.Log($"Selected child: {childName} with id: {childId}");
                PlayerPrefs.SetString("SelectedChildName", childName);
                PlayerPrefs.SetString("SelectedChildId", childId);
                
                string userType = PlayerPrefs.GetString("UserType");
                if (userType == "Ouder")
                {
                    SceneManager.LoadScene("AfsprakenScene");
                } else if (userType == "Child")
                {
                    // TODO navigate to child app
                } else
                {
                    Debug.LogError("UserType not found in PlayerPrefs");
                }
            } else
            {
                Debug.LogError("Id or Name not found in itemPrefab");
            }
        }
        
        private async void SendDeleteRequest(string ChildId)
        {
            await ApiClient.PerformApiCall(ApiClient.apiurl + "/child/" + ChildId, "DELETE");
        }
        
        public void DeleteItem(GameObject item)
        {
            // Find the text components
            TMP_Text idText = item.transform.Find("ButtonSelectChild/TMP_TextId")?.GetComponent<TMP_Text>();
            
            if (idText != null && !string.IsNullOrEmpty(idText.text))
            {
                // Retrieve the text values
                string appointmentName = idText.text;
            
                // Log the text values (or use them as needed)
                SendDeleteRequest(appointmentName);
                Destroy(item);
            } else
            {
                Debug.LogError("Id not found in itemPrefab");
            }
        }
    }
}