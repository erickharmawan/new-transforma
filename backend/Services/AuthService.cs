using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using backend.Contracts;
using backend.Data;
using backend.DTOs;
using backend.Models;
using BCrypt.Net;

namespace backend.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginDto)
    {
        var user = await _context.Users
            .Include(u => u.BaseRole)
            .Include(u => u.UserPolicies)
                .ThenInclude(up => up.Policy)
            .Include(u => u.ProjectAccesses)
                .ThenInclude(pa => pa.Project)
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user == null || !user.IsActive)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            return null;

        // Get user policies
        var policies = await GetUserPoliciesAsync(user.Id);

        // Get project accesses
        var projectAccesses = user.ProjectAccesses.Select(pa => new ProjectAccessDto
        {
            ProjectId = pa.ProjectId,
            ProjectName = pa.Project.Name,
            AccessLevel = pa.AccessLevel
        }).ToList();

        // Generate JWT token
        var token = GenerateJwtToken(user, policies);

        return new LoginResponseDto
        {
            Token = token,
            User = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                RoleName = user.BaseRole?.Name,
                BaseRoleId = user.BaseRoleId,
                IsActive = user.IsActive
            },
            Policies = policies,
            ProjectAccesses = projectAccesses
        };
    }

    public async Task<UserDto?> RegisterAsync(CreateUserDto createUserDto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == createUserDto.Email))
            return null;

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = createUserDto.Name,
            Email = createUserDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
            BaseRoleId = createUserDto.BaseRoleId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            BaseRoleId = user.BaseRoleId,
            IsActive = user.IsActive
        };
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"] ?? "");

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidAudience = _configuration["JwtSettings:Audience"],
                ClockSkew = TimeSpan.Zero
            }, out _);

            return true;
        }
        catch
        {
            return false;
        }
    }

    private string GenerateJwtToken(User user, List<string> policies)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings.GetValue<string>("SecretKey") ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Email),
            new("role", user.BaseRole?.Name ?? "")
        };

        // Add policies as claims
        foreach (var policy in policies)
        {
            claims.Add(new Claim("policy", policy));
        }

        var token = new JwtSecurityToken(
            issuer: jwtSettings.GetValue<string>("Issuer"),
            audience: jwtSettings.GetValue<string>("Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtSettings.GetValue<int>("ExpirationInMinutes")),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<List<string>> GetUserPoliciesAsync(Guid userId)
    {
        var user = await _context.Users
            .Include(u => u.BaseRole)
                .ThenInclude(r => r!.RolePolicies)
                .ThenInclude(rp => rp.Policy)
            .Include(u => u.UserPolicies)
                .ThenInclude(up => up.Policy)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return new List<string>();

        var policies = new List<string>();

        // Get role policies
        if (user.BaseRole != null)
        {
            policies.AddRange(user.BaseRole.RolePolicies.Select(rp => rp.Policy.Name));
        }

        // Apply user-specific policy overrides
        foreach (var userPolicy in user.UserPolicies)
        {
            if (userPolicy.IsGranted && !policies.Contains(userPolicy.Policy.Name))
            {
                policies.Add(userPolicy.Policy.Name);
            }
            else if (!userPolicy.IsGranted)
            {
                policies.Remove(userPolicy.Policy.Name);
            }
        }

        return policies.Distinct().ToList();
    }
}
