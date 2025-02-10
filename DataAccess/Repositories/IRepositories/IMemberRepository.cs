using BusinessObject.Models;

namespace DataAccess.Repositories.IRepositories
{
    public interface IMemberRepository
    {
        void AddMember(Member member);
        Member GetMemberById(int id);
        Member GetMemberByEmail(string email);
        List<Member> GetMembers();
        List<Member> Search(string keyword);
        void UpdateMember(Member Member);
        void DeleteMember(Member Member);
    }
}
