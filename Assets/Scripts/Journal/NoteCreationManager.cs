using Items;
using Patient;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Journal
{
    public class NoteCreationManager : MonoBehaviour
    {
        public TMP_InputField TitileInputField;
        public TMP_InputField ContentInputField;
        // public TMP_Text ErrorText;
    
        public Button CreateNoteButton;
        public Button BackButton;
        
        void Start()
        {
            CreateNoteButton.onClick.AddListener(CreateNote);
            BackButton.onClick.AddListener(Back);
        }
        
        public async void CreateNote()
        {
            // if (!Verify()) return;
            NoteDto note = new NoteDto(PlayerPrefs.GetString("SelectedChildId"), TitileInputField.text, ContentInputField.text);
            Debug.Log("Note JSON: " + JsonUtility.ToJson(note));
            await ApiClient.PerformApiCall(ApiClient.apiurl + "/note", "POST", JsonUtility.ToJson(note));
        
            Back();
        }
    
        public void Back()
        {
            SceneManager.LoadScene("JournalScene");
        }
    }
}