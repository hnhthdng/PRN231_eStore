using BusinessObject.Models;
using DataAccess.Data;

namespace DataAccess.DAO
{
    public class MemberDAO
    {
        private readonly StoreDbContext _context;

        // Constructor nhận StoreDbContext từ DI
        public MemberDAO(StoreDbContext context)
        {
            _context = context;
        }

        public List<Member> GetMembers()
        {
            var listMembers = new List<Member>();
            try
            {
                listMembers = _context.Members.ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listMembers;
        }

        public List<Member> Search(string keyword)
        {
            var listMembers = new List<Member>();
            try
            {
                listMembers = _context.Members
                    .Where(c => c.CompanyName.Contains(keyword) || c.Email.Contains(keyword))
                    .ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listMembers;
        }

        public Member FindMemberById(int memberId)
        {
            Member member;
            try
            {
                member = _context.Members.SingleOrDefault(c => c.MemberId == memberId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }

        public Member FindMemberByEmail(string email)
        {
            Member member;
            try
            {
                member = _context.Members.FirstOrDefault(c => c.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }

        public void AddMember(Member member)
        {
            var memberExist = FindMemberByEmail(member.Email);
            if (memberExist != null)
            {
                throw new Exception("Member already exists");
            }
            try
            {
                _context.Members.Add(member);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateMember(Member member)
        {
            try
            {
                var memberToUpdate = _context.Members.SingleOrDefault(c => c.MemberId == member.MemberId);
                if (memberToUpdate != null)
                {
                    memberToUpdate.Email = member.Email;
                    memberToUpdate.CompanyName = member.CompanyName;
                    memberToUpdate.City = member.City;
                    memberToUpdate.Country = member.Country;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteMember(Member member)
        {
            try
            {
                var memberToDelete = _context.Members.SingleOrDefault(c => c.MemberId == member.MemberId);
                if (memberToDelete != null)
                {
                    _context.Members.Remove(memberToDelete);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
