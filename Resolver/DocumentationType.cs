using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resolver
{
    public enum DocumentationType
    {
        Unknown = 0,
        Application = 1,
        AnnualVerification = 2,
        FullVerification = 3,
        NewEnrollment = 4,
        Escalation = 5,
        RandomSample = 6,
        StreetTeams = 7,
        FastApproval = 8,
        SSNEscalation = 9,
        RTA = 10,
        HHWSDRC = 11,
        HHWPicture = 12,
        NewAnnualVerificationModule = 13,
        Migration = 14
    }
}
