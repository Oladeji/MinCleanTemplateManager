using MinCleanTemplateManager.Contracts.ResponseDTO.V1;

namespace MinCleanTemplateManager.Contracts.ResponseDTO.V1
{
    public record ModelResponseDTO(Guid GuidId, string ModelName, string SampleModelName, ICollection<ModelVersionResponseDTO>? ModelVersions);

}