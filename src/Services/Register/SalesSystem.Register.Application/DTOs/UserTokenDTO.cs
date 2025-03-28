namespace SalesSystem.Register.Application.DTOs
{
    public record UserTokenDTO(string Id, string Email, IEnumerable<UserClaimDTO> Claims);
}
