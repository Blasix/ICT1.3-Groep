namespace Journal
{
    public class NoteDto
    {
        public string childId;
        public string title;
        public string content;
        
        public NoteDto(string childId, string title, string content)
        {
            this.childId = childId;
            this.title = title;
            this.content = content;
        }
    }
}