namespace Company.pro.PL.Services
{
    public interface ISengeltonService
    {
        public Guid Guid { get; set; }
        string GetGuid();
    }
}
