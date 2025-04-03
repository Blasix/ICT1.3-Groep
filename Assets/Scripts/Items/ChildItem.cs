namespace Items
{
    public class ChildItem
    {
        public string id;
        public string name;
        public string userId;
        public string trajectId;
        public string prefabId;
        public string artsName;
        
        public ChildItem(string id, string name, string userId, string trajectId, string prefabId, string artsName)
        {
            this.id = id;
            this.name = name;
            this.userId = userId;
            this.trajectId = trajectId;
            this.prefabId = prefabId;
            this.artsName = artsName;
        }
    }
}