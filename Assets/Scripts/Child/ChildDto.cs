namespace Patient
{
    public class ChildDto
    {
        public string trajectId;
        public string artsName;
        public string name;
        public int prefabId;
        
        public ChildDto(string trajectId, string artsName, string name, int prefabId)
        {
            this.trajectId = trajectId;
            this.artsName = artsName;
            this.name = name;
            this.prefabId = prefabId;
        }
    }
}