using Backend.Core;
using Backend.Infrastructure.Identity.Constants;
using FastEndpoints;

namespace Backend.Features;

public class GetDomainPropertiesEndpoint : EndpointWithoutRequest<ApiResponse<DomainPropertiesDto>>
{
    public override void Configure()
    {
        Get("/api/domain-properties");
        AllowAnonymous();
        Description(b => b
            .WithName("GetDomainProperties")
            .WithTags("DomainProperties"));
    }

    public override async Task HandleAsync(CancellationToken ct)
        => await SendAsync(ApiResponse<DomainPropertiesDto>.Success(DomainPropertiesDto.GetAll), cancellation: ct);
}

public class DomainPropertiesDto
{
    public string ApiVersion { get; set; } = "1.0";
    public RegistrationDetails RegistrationDetails { get; set; }

    public static DomainPropertiesDto GetAll => new()
    {
        RegistrationDetails = new RegistrationDetails
        {
            SupportedRoles = ApplicationRole.GetPublicRoles,
            PasswordPolicy = new PasswordPolicy
            {
                MinLength = 8,
                MaxLength = 100,
                RequiresUppercase = true,
                RequiresLowercase = true,
                RequiresDigit = true,
                RequiresSpecialCharacter = false
            }
        }
    };
}

public class RegistrationDetails
{
    public List<RoleDto> SupportedRoles { get; set; }
    public PasswordPolicy PasswordPolicy { get; set; }
}

public class PasswordPolicy
{
    public int MinLength { get; set; }
    public int MaxLength { get; set; }
    public bool RequiresUppercase { get; set; }
    public bool RequiresLowercase { get; set; }
    public bool RequiresDigit { get; set; }
    public bool RequiresSpecialCharacter { get; set; }
}


