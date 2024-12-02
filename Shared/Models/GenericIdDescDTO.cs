namespace Shared.Models
{
    public class GenericIdDescDTO
    {
        public int id { get; set; }
        public string name { get; set; }

        public GenericIdDescDTO() { }
        public GenericIdDescDTO(int id, string description)
        {
            this.id = id;
            this.name = description;
        }
    }
}
