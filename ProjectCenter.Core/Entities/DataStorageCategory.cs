namespace ProjectCenter.Core.Entities
{
    public class DataStorageCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Purpose { get; set; }
        public string? RetentionPeriod { get; set; }
    }
}
