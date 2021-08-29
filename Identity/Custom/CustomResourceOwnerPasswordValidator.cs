using IdentityServer4.Validation;
using IdentityModel;
using System.Threading.Tasks;
using Identity.Data;

namespace Identity.Custom
{
    public class CustomResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserRepository _userRepository;

        public CustomResourceOwnerPasswordValidator(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (_userRepository.ValidateCredentials(context.UserName, context.Password))
            {
                var user = _userRepository.FindByUsername(context.UserName);
                context.Result = new GrantValidationResult(user.Id, OidcConstants.AuthenticationMethods.Password);
            }

            return Task.FromResult(0);
        }
    }
}