using MyWebApp.DTO;

namespace MyWebApp.Interfaces;

public interface IAuthenticationService
{
    Task<AuthModel> RegisterUserAsync(SignUpModel model);

    Task<AuthModel> LoginUserAsync(SignInModel model);
}