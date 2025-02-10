using eStoreClient.DTO.Member;
using Refit;

namespace eStoreClient.Services
{
    public interface IMemberService
    {
        [Get("/api/member")]
        Task<List<MemberResponseDTO>> GetAllMembersAsync();

        [Get("/api/member/{id}")]
        Task<MemberResponseDTO> GetMemberByIdAsync(int id);

        [Get("/api/member/search")]
        Task<List<MemberResponseDTO>> SearchMembersAsync([AliasAs("keyword")] string keyword);

        [Get("/api/member/email/{email}")]
        Task<MemberResponseDTO> GetMemberByEmailAsync(string email);

        [Post("/api/member")]
        Task<MemberResponseDTO> CreateMemberAsync([Body] MemberRequestDTO memberRequest);

        [Put("/api/member/{id}")]
        Task<MemberResponseDTO> UpdateMemberAsync(int id, [Body] MemberRequestDTO memberRequest);

        [Delete("/api/member/{id}")]
        Task DeleteMemberAsync(int id);
    }
}
