using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ThirdPartyClientMVC.AuthPolicy
{
    public class SmithInSomeWhereRequirement:IAuthorizationRequirement
    {
        public SmithInSomeWhereRequirement()
        {
                
        }
    }
}
