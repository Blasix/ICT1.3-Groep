namespace Patient
{
    public class ChildDto
    {
        public string trajectName;
        public string artsName;
        public string name;
        public int prefabId;
        
        public ChildDto(string trajectName, string artsName, string name, int prefabId)
        {
            this.trajectName = trajectName;
            this.artsName = artsName;
            this.name = name;
            this.prefabId = prefabId;
        }
    }
}