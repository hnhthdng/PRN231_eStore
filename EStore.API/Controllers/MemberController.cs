using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO.Member;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace eStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;
        public MemberController(IMemberRepository memberRepository, IMapper mapper)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var members = _memberRepository.GetMembers();
            var membersResponse = _mapper.Map<List<MemberResponseDTO>>(members);
            return Ok(membersResponse);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var member = _memberRepository.GetMemberById(id);
            if (member == null)
            {
                return NotFound();
            }
            var memberResponse = _mapper.Map<MemberResponseDTO>(member);
            return Ok(memberResponse);
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string keyword)
        {
            var members = _memberRepository.Search(keyword);
            var membersResponse = _mapper.Map<List<MemberResponseDTO>>(members);
            return Ok(membersResponse);
        }

        [HttpGet("email/{email}")]
        public IActionResult GetMemberByEmail(string email)
        {
            var member = _memberRepository.GetMemberByEmail(email);
            if (member == null)
            {
                return NotFound();
            }
            var memberResponse = _mapper.Map<MemberResponseDTO>(member);
            return Ok(memberResponse);
        }

        [HttpPost]
        public IActionResult Post([FromBody] MemberRequestDTO memberRequest)
        {
            var member = _mapper.Map<Member>(memberRequest);
            _memberRepository.AddMember(member);
            return Ok(memberRequest);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] MemberRequestDTO memberRequest)
        {
            var member = _memberRepository.GetMemberById(id);
            if (member == null)
            {
                return NotFound();
            }
            _mapper.Map(memberRequest, member);
            _memberRepository.UpdateMember(member);
            return Ok(member);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var member = _memberRepository.GetMemberById(id);
            if (member == null)
            {
                return NotFound();
            }
            _memberRepository.DeleteMember(member);
            return Ok();
        }
    }
}
