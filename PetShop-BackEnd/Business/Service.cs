using Persistence.DAL;

namespace Business;

public class Service1
{
    public void Test()
    {
        var userRepo = new PersistenceAccess().UserRepository;
    }
}