using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage2.Models
{
    public class MembersOverViewViewModel
    {
        public Member Member { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool HasAddedMember { get; set; }
        public List<Member> Members { get; set; }
        public string Sort { get; set; }
        public MemberSearchParams SearchParams { get; set; }
    }
}