namespace Minerva.Server.DataAccessLayer.Models
{
    public class ItemData
    {
        public int ItemId { get; set; }
        public Item Item { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}