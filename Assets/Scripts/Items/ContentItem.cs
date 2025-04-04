using System;
using Newtonsoft.Json;
[Serializable]
public class ContentItem
{
    public string id { get; set; }
    public string Text { get; set; }
    public int SortingOrder { get; set; }
}
