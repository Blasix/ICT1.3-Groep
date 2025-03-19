namespace Patient
{
    public class PatientDto
    {
        public string id;
        public string userID;
        // public string trajectID;
        // public string artsID;
        public string vooraam;
        public string achternaam;
        
        public PatientDto(string id, string userID, string vooraam, string achternaam)
        {
            this.id = id;
            this.userID = userID;
            this.vooraam = vooraam;
            this.achternaam = achternaam;
        }
    }
}