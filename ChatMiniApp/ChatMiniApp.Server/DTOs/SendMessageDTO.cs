namespace ChatMiniApp.Server.DTOs
{
    public sealed record SendMessageDTO(Guid UserId, Guid ToUserId, string Message);

}
