namespace Items
{
    public class NoteItem
    {
        public string Id;
        public string ChildId;
        public string DateTime;
        public string Title;
        public string Content;
        
        public NoteItem(string id, string childId, string dateTime, string title, string content)
        {
            Id = id;
            ChildId = childId;
            DateTime = dateTime;
            Title = title;
            Content = content;
        }
    }
}