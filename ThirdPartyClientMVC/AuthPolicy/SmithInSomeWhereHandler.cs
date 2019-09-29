using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace ThirdPartyClientMVC.AuthPolicy
{
    public class SmithInSomeWhereHandler:AuthorizationHandler<SmithInSomeWhereRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            SmithInSomeWhereRequirement requirement)
        {
            var familyName = context.User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.FamilyName)?.Value;
            var location = context.User.Claims.FirstOrDefault(c => c.Type == "location")?.Value;
            if (familyName == "Smith" && location == "somewhere" && context.User.Identity.IsAuthenticated)
            {
                context.Succeed(requirement);
                return  Task.CompletedTask;
            }
            context.Fail();
            return  Task.CompletedTask;

            // 一个Requirement可以有多个Handler
            // 一个Handler成功，其他Handler没有返回失败 => 则Requirement被满足了
            // 某个Handler返回失败=>则 Requirement 无法被满足
            // 没有成功或失败=>则 Requirement 无法被满足
        }
    }
}
