namespace MinCleanTemplateManager.Contracts.RequestDTO.V1
{
    public record ModelGetRequestByGuidDTO(Guid guid);
    public record ModelGetRequestByIdDTO(string ModelName);
    public record ModelGetRequestDTO(string ModelName);
    public record ModelCreateRequestDTO(string ModelName, string SampleModelsName, Guid GuidId);
    public record ModelUpdateRequestDTO(string ModelName, string SampleModelsName, Guid GuidId);
    public record ModelDeleteRequestDTO(Guid guid);
}


