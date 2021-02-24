using System.Collections.Generic;
using Enfo.Domain.Entities;

namespace Enfo.Domain.Data
{
    public static partial class DomainData
    {
        public static IEnumerable<LegalAuthority> GetLegalAuthorities() => new List<LegalAuthority>
        {
            new LegalAuthority {Id = 1, Active = true, AuthorityName = "Air Quality Act",},
            new LegalAuthority {Id = 2, Active = true, AuthorityName = "Asbestos Safety Act",},
            new LegalAuthority
            {
                Id = 3, Active = true, AuthorityName = "Motor Vehicle Inspection and Maintenance Act",
            },
            new LegalAuthority {Id = 4, Active = true, AuthorityName = "Hazardous Waste Management Act",},
            new LegalAuthority {Id = 5, Active = true, AuthorityName = "Hazardous Site Response Act",},
            new LegalAuthority {Id = 6, Active = true, AuthorityName = "Underground Storage Tank Act",},
            new LegalAuthority {Id = 7, Active = true, AuthorityName = "Comprehensive Solid Waste Management Act",},
            new LegalAuthority
            {
                Id = 8,
                Active = true,
                AuthorityName = "Water Quality Control Act (including Surface Water Allocation)",
            },
            new LegalAuthority {Id = 9, Active = true, AuthorityName = "River Basin Management Planning Act",},
            new LegalAuthority {Id = 10, Active = true, AuthorityName = "Erosion and Sedimentation Act",},
            new LegalAuthority {Id = 11, Active = true, AuthorityName = "Surface Mining Act",},
            new LegalAuthority {Id = 12, Active = true, AuthorityName = "Safe Dams Act",},
            new LegalAuthority {Id = 13, Active = true, AuthorityName = "Safe Drinking Water Act",},
            new LegalAuthority {Id = 14, Active = true, AuthorityName = "Groundwater Use Act",},
            new LegalAuthority {Id = 15, Active = true, AuthorityName = "Oil and Gas and Deep Drilling Act",},
            new LegalAuthority {Id = 16, Active = true, AuthorityName = "Radiation Control Act",},
            new LegalAuthority
            {
                Id = 17, Active = true, AuthorityName = "Oil or Hazardous Materials Spills or Releases Act",
            },
            new LegalAuthority {Id = 18, Active = true, AuthorityName = "Georgia Environmental Policy Act",},
            new LegalAuthority {Id = 19, Active = true, AuthorityName = "Lead Poisoning Prevention Act",},
            new LegalAuthority {Id = 20, Active = true, AuthorityName = "Water Well Standards Act",},
            new LegalAuthority {Id = 21, Active = false, AuthorityName = "Year 2000 Readiness Act",},
            new LegalAuthority {Id = 22, Active = true, AuthorityName = "Voluntary Remediation Program Act",}
        };
    }
}
