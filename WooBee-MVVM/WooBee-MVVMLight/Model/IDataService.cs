using System.Threading.Tasks;

namespace WooBee_MVVMLight.Model
{
    public interface IDataService
    {
        Task<DataItem> GetData();
    }
}