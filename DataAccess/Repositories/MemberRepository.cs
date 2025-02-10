using BusinessObject.Models;
using DataAccess.DAO;
using DataAccess.Repositories.IRepositories;

namespace DataAccess.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly MemberDAO _memberDAO;

        public MemberRepository(MemberDAO memberDAO)
        {
            _memberDAO = memberDAO;
        }

        public void AddMember(Member member) => _memberDAO.AddMember(member);

        public Member GetMemberById(int id) => _memberDAO.FindMemberById(id);

        public Member GetMemberByEmail(string email) => _memberDAO.FindMemberByEmail(email);

        public List<Member> GetMembers() => _memberDAO.GetMembers();

        public List<Member> Search(string keyword) => _memberDAO.Search(keyword);

        public void UpdateMember(Member member) => _memberDAO.UpdateMember(member);

        public void DeleteMember(Member member) => _memberDAO.DeleteMember(member);
    }
}
