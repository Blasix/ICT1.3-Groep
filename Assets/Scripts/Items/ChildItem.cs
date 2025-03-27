namespace Items
{
    public class ChildItem
    {
        public string Id;
        public string Name;
        public string UserId;
        public string TrajectId;
        public string PrefabId;
        public string ArtsName;
        
        public ChildItem(string id, string name, string userId, string trajectId, string prefabId, string artsName)
        {
            Id = id;
            Name = name;
            UserId = userId;
            TrajectId = trajectId;
            PrefabId = prefabId;
            ArtsName = artsName;
        }
    }
}