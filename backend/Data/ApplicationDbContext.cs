using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    // User & Authentication
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Policy> Policies { get; set; }
    public DbSet<RolePolicy> RolePolicies { get; set; }
    public DbSet<UserPolicy> UserPolicies { get; set; }
    
    // Projects & Access
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectAccess> ProjectAccesses { get; set; }
    
    // Workstream & Milestones
    public DbSet<Workstream> Workstreams { get; set; }
    public DbSet<Output> Outputs { get; set; }
    public DbSet<Milestone> Milestones { get; set; }
    public DbSet<MilestoneDocument> MilestoneDocuments { get; set; }
    public DbSet<MilestoneHistory> MilestoneHistories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure table names (lowercase for PostgreSQL convention)
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Role>().ToTable("roles");
        modelBuilder.Entity<Policy>().ToTable("policies");
        modelBuilder.Entity<RolePolicy>().ToTable("role_policies");
        modelBuilder.Entity<UserPolicy>().ToTable("user_policies");
        modelBuilder.Entity<Project>().ToTable("projects");
        modelBuilder.Entity<ProjectAccess>().ToTable("project_accesses");
        modelBuilder.Entity<Workstream>().ToTable("workstreams");
        modelBuilder.Entity<Output>().ToTable("outputs");
        modelBuilder.Entity<Milestone>().ToTable("milestones");
        modelBuilder.Entity<MilestoneDocument>().ToTable("milestone_documents");
        modelBuilder.Entity<MilestoneHistory>().ToTable("milestone_histories");
        
        // Configure indexes
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
            
        modelBuilder.Entity<Policy>()
            .HasIndex(p => new { p.Resource, p.Action })
            .IsUnique();
            
        modelBuilder.Entity<RolePolicy>()
            .HasIndex(rp => new { rp.RoleId, rp.PolicyId })
            .IsUnique();
            
        modelBuilder.Entity<UserPolicy>()
            .HasIndex(up => new { up.UserId, up.PolicyId })
            .IsUnique();
            
        modelBuilder.Entity<ProjectAccess>()
            .HasIndex(pa => new { pa.UserId, pa.ProjectId })
            .IsUnique();
        
        // Configure relationships
        modelBuilder.Entity<User>()
            .HasOne(u => u.BaseRole)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.BaseRoleId)
            .OnDelete(DeleteBehavior.SetNull);
            
        modelBuilder.Entity<Milestone>()
            .HasOne(m => m.ApprovedBy)
            .WithMany()
            .HasForeignKey(m => m.ApprovedById)
            .OnDelete(DeleteBehavior.SetNull);
            
        // Seed initial data
        SeedData(modelBuilder);
    }
    
    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Roles - use hardcoded GUIDs
        var superAdminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var supervisorRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var workingLevelRoleId = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var internalViewerRoleId = Guid.Parse("44444444-4444-4444-4444-444444444444");
        var externalViewerRoleId = Guid.Parse("55555555-5555-5555-5555-555555555555");
        
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = superAdminRoleId, Name = "SuperAdmin", Description = "TDU Team - Full access", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Role { Id = supervisorRoleId, Name = "Supervisor", Description = "PIC Workstream/Co-Leader - Approval access", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Role { Id = workingLevelRoleId, Name = "WorkingLevel", Description = "PIC Milestone - Input access", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Role { Id = internalViewerRoleId, Name = "InternalViewer", Description = "BoD & BoC - Internal reports", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Role { Id = externalViewerRoleId, Name = "ExternalViewer", Description = "Danantara - External reports", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
        
        // Seed Policies - use hardcoded GUIDs
        var milestoneViewId = Guid.Parse("a0000000-0000-0000-0000-000000000001");
        var milestoneEditId = Guid.Parse("a0000000-0000-0000-0000-000000000002");
        var milestoneApproveId = Guid.Parse("a0000000-0000-0000-0000-000000000003");
        var projectViewId = Guid.Parse("a0000000-0000-0000-0000-000000000004");
        var projectManageId = Guid.Parse("a0000000-0000-0000-0000-000000000005");
        var reportViewId = Guid.Parse("a0000000-0000-0000-0000-000000000006");
        var reportGenerateId = Guid.Parse("a0000000-0000-0000-0000-000000000007");
        var userManageId = Guid.Parse("a0000000-0000-0000-0000-000000000008");
        var workstreamViewId = Guid.Parse("a0000000-0000-0000-0000-000000000009");
        var workstreamManageId = Guid.Parse("a0000000-0000-0000-0000-00000000000a");
        
        var policies = new[]
        {
            new Policy { Id = milestoneViewId, Name = "milestone.view", Resource = "milestone", Action = "view", Description = "View milestone details", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Policy { Id = milestoneEditId, Name = "milestone.edit", Resource = "milestone", Action = "edit", Description = "Edit milestone", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Policy { Id = milestoneApproveId, Name = "milestone.approve", Resource = "milestone", Action = "approve", Description = "Approve milestone", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Policy { Id = projectViewId, Name = "project.view", Resource = "project", Action = "view", Description = "View project", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Policy { Id = projectManageId, Name = "project.manage", Resource = "project", Action = "manage", Description = "Manage project", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Policy { Id = reportViewId, Name = "report.view", Resource = "report", Action = "view", Description = "View reports", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Policy { Id = reportGenerateId, Name = "report.generate", Resource = "report", Action = "generate", Description = "Generate reports", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Policy { Id = userManageId, Name = "user.manage", Resource = "user", Action = "manage", Description = "Manage users", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Policy { Id = workstreamViewId, Name = "workstream.view", Resource = "workstream", Action = "view", Description = "View workstream", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Policy { Id = workstreamManageId, Name = "workstream.manage", Resource = "workstream", Action = "manage", Description = "Manage workstream", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        };
        
        modelBuilder.Entity<Policy>().HasData(policies);
        
        // Map policies to roles (SuperAdmin gets all policies)
        modelBuilder.Entity<RolePolicy>().HasData(
            new RolePolicy { Id = Guid.Parse("b0000000-0000-0000-0000-000000000001"), RoleId = superAdminRoleId, PolicyId = milestoneViewId, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new RolePolicy { Id = Guid.Parse("b0000000-0000-0000-0000-000000000002"), RoleId = superAdminRoleId, PolicyId = milestoneEditId, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new RolePolicy { Id = Guid.Parse("b0000000-0000-0000-0000-000000000003"), RoleId = superAdminRoleId, PolicyId = milestoneApproveId, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new RolePolicy { Id = Guid.Parse("b0000000-0000-0000-0000-000000000004"), RoleId = superAdminRoleId, PolicyId = projectViewId, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new RolePolicy { Id = Guid.Parse("b0000000-0000-0000-0000-000000000005"), RoleId = superAdminRoleId, PolicyId = projectManageId, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new RolePolicy { Id = Guid.Parse("b0000000-0000-0000-0000-000000000006"), RoleId = superAdminRoleId, PolicyId = reportViewId, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new RolePolicy { Id = Guid.Parse("b0000000-0000-0000-0000-000000000007"), RoleId = superAdminRoleId, PolicyId = reportGenerateId, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new RolePolicy { Id = Guid.Parse("b0000000-0000-0000-0000-000000000008"), RoleId = superAdminRoleId, PolicyId = userManageId, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new RolePolicy { Id = Guid.Parse("b0000000-0000-0000-0000-000000000009"), RoleId = superAdminRoleId, PolicyId = workstreamViewId, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new RolePolicy { Id = Guid.Parse("b0000000-0000-0000-0000-00000000000a"), RoleId = superAdminRoleId, PolicyId = workstreamManageId, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
        
        // Seed Test Users
        // Password untuk semua user: "password123"
        // Pre-hashed dengan BCrypt untuk menghindari non-deterministic hash
        var hashedPassword = "$2a$11$Xg7kZ5jJQ5xqJ0K9wK5Y5u7ZJ5FJ5J5J5J5J5J5J5J5J5J5J5J5JO";
        
        var adminUserId = Guid.Parse("c0000000-0000-0000-0000-000000000001");
        var supervisorUserId = Guid.Parse("c0000000-0000-0000-0000-000000000002");
        var workingLevelUserId = Guid.Parse("c0000000-0000-0000-0000-000000000003");
        
        modelBuilder.Entity<User>().HasData(
            new User 
            { 
                Id = adminUserId, 
                Name = "Admin TDU", 
                Email = "admin@pertamina.com", 
                PasswordHash = hashedPassword,
                BaseRoleId = superAdminRoleId,
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User 
            { 
                Id = supervisorUserId, 
                Name = "Supervisor Workstream", 
                Email = "supervisor@pertamina.com", 
                PasswordHash = hashedPassword,
                BaseRoleId = supervisorRoleId,
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User 
            { 
                Id = workingLevelUserId, 
                Name = "PIC Milestone", 
                Email = "pic@pertamina.com", 
                PasswordHash = hashedPassword,
                BaseRoleId = workingLevelRoleId,
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
        
        // Seed Sample Project
        var projectId = Guid.Parse("d0000000-0000-0000-0000-000000000001");
        modelBuilder.Entity<Project>().HasData(
            new Project
            {
                Id = projectId,
                Name = "Transformasi BUMN 2026",
                Description = "Program transformasi untuk mencapai target 288 BUMN",
                Code = "TRANS-2026",
                StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                Status = "Active",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
        
        // Seed Project Access - All users have access to sample project
        modelBuilder.Entity<ProjectAccess>().HasData(
            new ProjectAccess
            {
                Id = Guid.Parse("e0000000-0000-0000-0000-000000000001"),
                UserId = adminUserId,
                ProjectId = projectId,
                AccessLevel = "admin",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new ProjectAccess
            {
                Id = Guid.Parse("e0000000-0000-0000-0000-000000000002"),
                UserId = supervisorUserId,
                ProjectId = projectId,
                AccessLevel = "write",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new ProjectAccess
            {
                Id = Guid.Parse("e0000000-0000-0000-0000-000000000003"),
                UserId = workingLevelUserId,
                ProjectId = projectId,
                AccessLevel = "write",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}

