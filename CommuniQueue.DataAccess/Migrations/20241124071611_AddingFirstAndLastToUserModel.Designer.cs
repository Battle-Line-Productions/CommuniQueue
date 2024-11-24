﻿// <auto-generated />
using System;
using CommuniQueue.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CommuniQueue.DataAccess.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241124071611_AddingFirstAndLastToUserModel")]
    partial class AddingFirstAndLastToUserModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CommuniQueue.Contracts.Models.ApiKey", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date_time");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end_date");

                    b.Property<bool>("IsExpired")
                        .HasColumnType("boolean")
                        .HasColumnName("is_expired");

                    b.Property<string>("KeyHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("key_hash");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid")
                        .HasColumnName("project_id");

                    b.Property<string>("Scopes")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("scopes");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_date");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date_time");

                    b.HasKey("Id")
                        .HasName("pk_api_keys");

                    b.HasIndex("ProjectId")
                        .HasDatabaseName("ix_api_keys_project_id");

                    b.ToTable("api_keys", (string)null);
                });

            modelBuilder.Entity("CommuniQueue.Contracts.Models.Container", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date_time");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<bool>("IsRoot")
                        .HasColumnType("boolean")
                        .HasColumnName("is_root");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uuid")
                        .HasColumnName("parent_id");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid")
                        .HasColumnName("project_id");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date_time");

                    b.HasKey("Id")
                        .HasName("pk_containers");

                    b.HasIndex("ParentId")
                        .HasDatabaseName("ix_containers_parent_id");

                    b.HasIndex("ProjectId")
                        .HasDatabaseName("ix_containers_project_id");

                    b.ToTable("containers", (string)null);
                });

            modelBuilder.Entity("CommuniQueue.Contracts.Models.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date_time");

                    b.Property<Guid>("EntityId")
                        .HasColumnType("uuid")
                        .HasColumnName("entity_id");

                    b.Property<int>("EntityType")
                        .HasColumnType("integer")
                        .HasColumnName("entity_type");

                    b.Property<int>("PermissionLevel")
                        .HasColumnType("integer")
                        .HasColumnName("permission_level");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("uuid")
                        .HasColumnName("project_id");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date_time");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_permissions");

                    b.HasIndex("EntityId")
                        .HasDatabaseName("ix_permissions_entity_id");

                    b.HasIndex("ProjectId")
                        .HasDatabaseName("ix_permissions_project_id");

                    b.HasIndex("UserId", "EntityId", "EntityType")
                        .IsUnique()
                        .HasDatabaseName("ix_permissions_user_id_entity_id_entity_type");

                    b.ToTable("permissions", (string)null);
                });

            modelBuilder.Entity("CommuniQueue.Contracts.Models.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date_time");

                    b.Property<string>("CustomerId")
                        .HasColumnType("text")
                        .HasColumnName("customer_id");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date_time");

                    b.HasKey("Id")
                        .HasName("pk_projects");

                    b.ToTable("projects", (string)null);
                });

            modelBuilder.Entity("CommuniQueue.Contracts.Models.Stage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date_time");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("Order")
                        .HasColumnType("integer")
                        .HasColumnName("order");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid")
                        .HasColumnName("project_id");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date_time");

                    b.HasKey("Id")
                        .HasName("pk_stages");

                    b.HasIndex("ProjectId", "Name")
                        .IsUnique()
                        .HasDatabaseName("ix_stages_project_id_name");

                    b.ToTable("stages", (string)null);
                });

            modelBuilder.Entity("CommuniQueue.Contracts.Models.Template", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("ContainerId")
                        .HasColumnType("uuid")
                        .HasColumnName("container_id");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date_time");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid")
                        .HasColumnName("project_id");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date_time");

                    b.HasKey("Id")
                        .HasName("pk_templates");

                    b.HasIndex("ContainerId")
                        .HasDatabaseName("ix_templates_container_id");

                    b.HasIndex("ProjectId")
                        .HasDatabaseName("ix_templates_project_id");

                    b.ToTable("templates", (string)null);
                });

            modelBuilder.Entity("CommuniQueue.Contracts.Models.TemplateStageAssignment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date_time");

                    b.Property<Guid>("StageId")
                        .HasColumnType("uuid")
                        .HasColumnName("stage_id");

                    b.Property<Guid>("TemplateVersionId")
                        .HasColumnType("uuid")
                        .HasColumnName("template_version_id");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date_time");

                    b.HasKey("Id")
                        .HasName("pk_template_stage_assignments");

                    b.HasIndex("TemplateVersionId")
                        .HasDatabaseName("ix_template_stage_assignments_template_version_id");

                    b.HasIndex("StageId", "TemplateVersionId")
                        .IsUnique()
                        .HasDatabaseName("ix_template_stage_assignments_stage_id_template_version_id");

                    b.ToTable("template_stage_assignments", (string)null);
                });

            modelBuilder.Entity("CommuniQueue.Contracts.Models.TemplateVersion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("body");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date_time");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("subject");

                    b.Property<Guid>("TemplateId")
                        .HasColumnType("uuid")
                        .HasColumnName("template_id");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date_time");

                    b.Property<int>("VersionNumber")
                        .HasColumnType("integer")
                        .HasColumnName("version_number");

                    b.HasKey("Id")
                        .HasName("pk_template_versions");

                    b.HasIndex("TemplateId")
                        .HasDatabaseName("ix_template_versions_template_id");

                    b.ToTable("template_versions", (string)null);
                });

            modelBuilder.Entity("CommuniQueue.Contracts.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date_time");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("SsoId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("sso_id");

                    b.Property<DateTime>("UpdatedDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date_time");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_users_email");

                    b.HasIndex("SsoId")
                        .IsUnique()
                        .HasDatabaseName("ix_users_sso_id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("CommuniQueue.Contracts.Models.ApiKey", b =>
                {
                    b.HasOne("CommuniQueue.Contracts.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_api_keys_projects_project_id");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("CommuniQueue.Contracts.Models.Container", b =>
                {
                    b.HasOne("CommuniQueue.Contracts.Models.Container", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("fk_containers_containers_parent_id");

                    b.HasOne("CommuniQueue.Contracts.Models.Project", "Project")
                        .WithMany("Containers")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_containers_projects_project_id");

                    b.Navigation("Parent");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("CommuniQueue.Contracts.Models.Permission", b =>
                {
                    b.HasOne("CommuniQueue.Contracts.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_permissions_projects_entity_id");

                    b.HasOne("CommuniQueue.Contracts.Models.Project", null)
                        .WithMany("Permissions")
                        .HasForeignKey("ProjectId")
                        .HasConstraintName("fk_permissions_projects_project_id");

                    b.HasOne("CommuniQueue.Contracts.Models.User", "User")
                        .WithMany("Permissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_permissions_users_user_id");

                    b.Navigation("Project");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CommuniQueue.Contracts.Models.Stage", b =>
                {
                    b.HasOne("CommuniQueue.Contracts.Models.Project", "Project")
                        .WithMany("Stages")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_stages_projects_project_id");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("CommuniQueue.Contracts.Models.Template", b =>
                {
                    b.HasOne("CommuniQueue.Contracts.Models.Container", "Container")
                        .WithMany("Templates")
                        .HasForeignKey("ContainerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_templates_containers_container_id");

                    b.HasOne("CommuniQueue.Contracts.Models.Project", "Project")
                        .WithMany("Templates")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_templates_projects_project_id");

                    b.Navigation("Container");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("CommuniQueue.Contracts.Models.TemplateStageAssignment", b =>
                {
                    b.HasOne("CommuniQueue.Contracts.Models.Stage", "Stage")
                        .WithMany()
                        .HasForeignKey("StageId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_template_stage_assignments_stages_stage_id");

                    b.HasOne("CommuniQueue.Contracts.Models.TemplateVersion", "TemplateVersion")
                        .WithMany("StageAssignments")
                        .HasForeignKey("TemplateVersionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_template_stage_assignments_template_versions_template_versi");

                    b.Navigation("Stage");

                    b.Navigation("TemplateVersion");
                });

            modelBuilder.Entity("CommuniQueue.Contracts.Models.TemplateVersion", b =>
                {
                    b.HasOne("CommuniQueue.Contracts.Models.Template", "Template")
                        .WithMany("Versions")
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_template_versions_templates_template_id");

                    b.Navigation("Template");
                });

            modelBuilder.Entity("CommuniQueue.Contracts.Models.Container", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("Templates");
                });

            modelBuilder.Entity("CommuniQueue.Contracts.Models.Project", b =>
                {
                    b.Navigation("Containers");

                    b.Navigation("Permissions");

                    b.Navigation("Stages");

                    b.Navigation("Templates");
                });

            modelBuilder.Entity("CommuniQueue.Contracts.Models.Template", b =>
                {
                    b.Navigation("Versions");
                });

            modelBuilder.Entity("CommuniQueue.Contracts.Models.TemplateVersion", b =>
                {
                    b.Navigation("StageAssignments");
                });

            modelBuilder.Entity("CommuniQueue.Contracts.Models.User", b =>
                {
                    b.Navigation("Permissions");
                });
#pragma warning restore 612, 618
        }
    }
}
