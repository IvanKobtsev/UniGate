using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UniGate.Common.Utilities;

public static class ExtraTask
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var appSettings = serviceProvider.GetRequiredService<IConfiguration>();

        var roleNames = appSettings.GetSection("Roles").Get<string[]>()?.ToList();

        if (roleNames == null) throw new InvalidOperationException("Roles list is missing in configuration");

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
                await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
        }
    }
}