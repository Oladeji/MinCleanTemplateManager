

namespace MinCleanTemplateManager.Contracts.RequestDTO.V1
{


    //public record SampleModelCreateRequestDTO(Guid GuidId, string SampleModelName, string TestingModeGroupName);


    //public record SampleModelUpdateRequestDTO(Guid SampleModelId, string SampleModelName, string TestingModeGroupName);
    public record SampleModelCreateRequestDTO(Guid GuidId, string SampleModelName);


    public record SampleModelUpdateRequestDTO(Guid GuidId, string SampleModelName);
    public record SampleModelGetRequestByGuidDTO(Guid GuidId);
    public record SampleModelGetRequestByIdDTO(string SampleModelId);
    public record SampleModelGetRequestDTO(string SampleModelName);
    public record SampleModelDeleteRequestDTO(Guid guid);
}
