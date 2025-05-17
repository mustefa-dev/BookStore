using BookStore.DATA.DTOs.User;

namespace BookStore.Interface
{
    public interface ITokenService
    {
        string CreateToken(TokenDTO user);
    }
}