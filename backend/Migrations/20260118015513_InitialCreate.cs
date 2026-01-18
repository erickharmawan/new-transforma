using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "policies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Resource = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Action = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_policies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "role_policies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    PolicyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_policies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_role_policies_policies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_policies_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    BaseRoleId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_roles_BaseRoleId",
                        column: x => x.BaseRoleId,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "project_accesses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccessLevel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_accesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_project_accesses_projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_project_accesses_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_policies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PolicyId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsGranted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_policies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_policies_policies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_policies_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workstreams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    LeaderId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workstreams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workstreams_projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_workstreams_users_LeaderId",
                        column: x => x.LeaderId,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "outputs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    WorkstreamId = table.Column<Guid>(type: "uuid", nullable: false),
                    PicLeaderId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_outputs_users_PicLeaderId",
                        column: x => x.PicLeaderId,
                        principalTable: "users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_outputs_workstreams_WorkstreamId",
                        column: x => x.WorkstreamId,
                        principalTable: "workstreams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "milestones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    OutputId = table.Column<Guid>(type: "uuid", nullable: false),
                    PicUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PlannedStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PlannedEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Progress = table.Column<int>(type: "integer", nullable: false),
                    IsCritical = table.Column<bool>(type: "boolean", nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ApprovedById = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RejectionReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_milestones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_milestones_outputs_OutputId",
                        column: x => x.OutputId,
                        principalTable: "outputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_milestones_users_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_milestones_users_PicUserId",
                        column: x => x.PicUserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "milestone_documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MilestoneId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FileType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadedById = table.Column<Guid>(type: "uuid", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_milestone_documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_milestone_documents_milestones_MilestoneId",
                        column: x => x.MilestoneId,
                        principalTable: "milestones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_milestone_documents_users_UploadedById",
                        column: x => x.UploadedById,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "milestone_histories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MilestoneId = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OldStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    NewStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ChangedById = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_milestone_histories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_milestone_histories_milestones_MilestoneId",
                        column: x => x.MilestoneId,
                        principalTable: "milestones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_milestone_histories_users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "policies",
                columns: new[] { "Id", "Action", "CreatedAt", "Description", "Name", "Resource" },
                values: new object[,]
                {
                    { new Guid("a0000000-0000-0000-0000-000000000001"), "view", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "View milestone details", "milestone.view", "milestone" },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), "edit", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Edit milestone", "milestone.edit", "milestone" },
                    { new Guid("a0000000-0000-0000-0000-000000000003"), "approve", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Approve milestone", "milestone.approve", "milestone" },
                    { new Guid("a0000000-0000-0000-0000-000000000004"), "view", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "View project", "project.view", "project" },
                    { new Guid("a0000000-0000-0000-0000-000000000005"), "manage", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manage project", "project.manage", "project" },
                    { new Guid("a0000000-0000-0000-0000-000000000006"), "view", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "View reports", "report.view", "report" },
                    { new Guid("a0000000-0000-0000-0000-000000000007"), "generate", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Generate reports", "report.generate", "report" },
                    { new Guid("a0000000-0000-0000-0000-000000000008"), "manage", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manage users", "user.manage", "user" },
                    { new Guid("a0000000-0000-0000-0000-000000000009"), "view", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "View workstream", "workstream.view", "workstream" },
                    { new Guid("a0000000-0000-0000-0000-00000000000a"), "manage", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manage workstream", "workstream.manage", "workstream" }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "TDU Team - Full access", "SuperAdmin" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "PIC Workstream/Co-Leader - Approval access", "Supervisor" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "PIC Milestone - Input access", "WorkingLevel" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "BoD & BoC - Internal reports", "InternalViewer" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Danantara - External reports", "ExternalViewer" }
                });

            migrationBuilder.InsertData(
                table: "role_policies",
                columns: new[] { "Id", "CreatedAt", "PolicyId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("b0000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a0000000-0000-0000-0000-000000000001"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("b0000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a0000000-0000-0000-0000-000000000002"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("b0000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a0000000-0000-0000-0000-000000000003"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("b0000000-0000-0000-0000-000000000004"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a0000000-0000-0000-0000-000000000004"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("b0000000-0000-0000-0000-000000000005"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a0000000-0000-0000-0000-000000000005"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("b0000000-0000-0000-0000-000000000006"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a0000000-0000-0000-0000-000000000006"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("b0000000-0000-0000-0000-000000000007"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a0000000-0000-0000-0000-000000000007"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("b0000000-0000-0000-0000-000000000008"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a0000000-0000-0000-0000-000000000008"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("b0000000-0000-0000-0000-000000000009"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a0000000-0000-0000-0000-000000000009"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("b0000000-0000-0000-0000-00000000000a"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a0000000-0000-0000-0000-00000000000a"), new Guid("11111111-1111-1111-1111-111111111111") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_milestone_documents_MilestoneId",
                table: "milestone_documents",
                column: "MilestoneId");

            migrationBuilder.CreateIndex(
                name: "IX_milestone_documents_UploadedById",
                table: "milestone_documents",
                column: "UploadedById");

            migrationBuilder.CreateIndex(
                name: "IX_milestone_histories_ChangedById",
                table: "milestone_histories",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_milestone_histories_MilestoneId",
                table: "milestone_histories",
                column: "MilestoneId");

            migrationBuilder.CreateIndex(
                name: "IX_milestones_ApprovedById",
                table: "milestones",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_milestones_OutputId",
                table: "milestones",
                column: "OutputId");

            migrationBuilder.CreateIndex(
                name: "IX_milestones_PicUserId",
                table: "milestones",
                column: "PicUserId");

            migrationBuilder.CreateIndex(
                name: "IX_outputs_PicLeaderId",
                table: "outputs",
                column: "PicLeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_outputs_WorkstreamId",
                table: "outputs",
                column: "WorkstreamId");

            migrationBuilder.CreateIndex(
                name: "IX_policies_Resource_Action",
                table: "policies",
                columns: new[] { "Resource", "Action" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_project_accesses_ProjectId",
                table: "project_accesses",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_project_accesses_UserId_ProjectId",
                table: "project_accesses",
                columns: new[] { "UserId", "ProjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_policies_PolicyId",
                table: "role_policies",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_role_policies_RoleId_PolicyId",
                table: "role_policies",
                columns: new[] { "RoleId", "PolicyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_policies_PolicyId",
                table: "user_policies",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_user_policies_UserId_PolicyId",
                table: "user_policies",
                columns: new[] { "UserId", "PolicyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_BaseRoleId",
                table: "users",
                column: "BaseRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_workstreams_LeaderId",
                table: "workstreams",
                column: "LeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_workstreams_ProjectId",
                table: "workstreams",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "milestone_documents");

            migrationBuilder.DropTable(
                name: "milestone_histories");

            migrationBuilder.DropTable(
                name: "project_accesses");

            migrationBuilder.DropTable(
                name: "role_policies");

            migrationBuilder.DropTable(
                name: "user_policies");

            migrationBuilder.DropTable(
                name: "milestones");

            migrationBuilder.DropTable(
                name: "policies");

            migrationBuilder.DropTable(
                name: "outputs");

            migrationBuilder.DropTable(
                name: "workstreams");

            migrationBuilder.DropTable(
                name: "projects");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
