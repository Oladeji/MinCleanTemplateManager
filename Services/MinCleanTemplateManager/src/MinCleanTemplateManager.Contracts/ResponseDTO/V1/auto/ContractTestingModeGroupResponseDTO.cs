namespace MinCleanTemplateManager.Contracts.ResponseDTO.V1
{
  //  public  record TestingModeGroupResponseDTO(Object Value);
    public record TestingModeGroupResponseDTO(string TestingModeGroupName, string testingMode, string description, Guid guidId);
}