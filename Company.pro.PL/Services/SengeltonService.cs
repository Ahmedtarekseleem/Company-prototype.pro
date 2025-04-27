
namespace Company.pro.PL.Services
{
    public class SengeltonService : ISengeltonService
    {
        public SengeltonService()
        {
            Guid = Guid.NewGuid();
        }
        public Guid Guid { get; set; }
        public string GetGuid()
        {
            return Guid.ToString();
        }
    }
}
