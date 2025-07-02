namespace MinCleanTemplateManager.Contracts.RequestDTO.V1
{
    public record TestingModeGroupGetRequestByGuidDTO(Guid guid);
    public record TestingModeGroupGetRequestByIdDTO(string EntityNameId);
    public record TestingModeGroupGetRequestDTO(string TestingModeGroupName);
    public record TestingModeGroupCreateRequestDTO(string testingModeGroupName, string defaultTestingMode, string description, Guid guidId);
    public record TestingModeGroupUpdateRequestDTO(string testingModeGroupName, string defaultTestingMode, string description, Guid guidId);
    public record TestingModeGroupDeleteRequestDTO(Guid guid);



}