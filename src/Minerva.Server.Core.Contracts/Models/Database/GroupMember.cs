namespace Minerva.Server.Core.Contracts.Models.Database
{
    public class GroupMember
    {
        public int CharacterId { get; set; }
        public Character Character { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public uint Level { get; set; }
    }
}