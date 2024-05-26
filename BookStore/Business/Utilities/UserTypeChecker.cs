using Persistence.DAL;

namespace Business.Utilities;

internal static class UserTypeChecker
{
    public static bool CheckIfAdmin(string username)
    {
        var gdprUsername = GdprUtility.Encrypt(username);
        var userType = PersistenceFacade.Instance.UserRepository.GetUserType(gdprUsername);
        if (!userType.IsSuccess)
            return false;

        return userType.SuccessValue == GdprUtility.Encrypt("ADMIN");
    }
    
    public static bool CheckIfProvider(string username)
    {
        var gdprUsername = GdprUtility.Encrypt(username);
        var userType = PersistenceFacade.Instance.UserRepository.GetUserType(gdprUsername);
        if (!userType.IsSuccess)
            return false;

        return userType.SuccessValue == GdprUtility.Encrypt("PROVIDER");
    }
}