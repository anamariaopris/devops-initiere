using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnlineSchoolApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "app_user",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    first_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    last_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone_number = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password_hash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    last_login_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_user", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "course",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    course_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    department = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "administrator",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    employee_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    department = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    job_title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_administrator", x => x.id);
                    table.ForeignKey(
                        name: "FK_administrator_app_user_id",
                        column: x => x.id,
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "student",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    student_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    enrollment_date = table.Column<DateOnly>(type: "date", nullable: true),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    guardian_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    guardian_email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student", x => x.id);
                    table.ForeignKey(
                        name: "FK_student_app_user_id",
                        column: x => x.id,
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "teacher",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    employee_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    specialization = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bio = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher", x => x.id);
                    table.ForeignKey(
                        name: "FK_teacher_app_user_id",
                        column: x => x.id,
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_permissions",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    permission = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_permissions", x => new { x.user_id, x.permission });
                    table.ForeignKey(
                        name: "fk_user_permissions_user",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "book",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    book_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    student_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book", x => x.id);
                    table.ForeignKey(
                        name: "fk_book_student",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "enrolment",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    student_id = table.Column<int>(type: "int", nullable: true),
                    course_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enrolment", x => x.id);
                    table.ForeignKey(
                        name: "fk_enrolment_course",
                        column: x => x.course_id,
                        principalTable: "course",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_enrolment_student",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "app_user",
                columns: new[] { "id", "created_at", "email", "first_name", "is_active", "last_login_at", "last_name", "password_hash", "phone_number", "updated_at" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), "admin@online-school.ro", "System", true, null, "Administrator", "$2a$10$jg3d2Ltol2A8tyaVg9VoNeG47.fj1iIqGv0ndQpmSxc96JfpBMTLi", "+40740000001", new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), "andrei.ionescu@online-school.ro", "Andrei", true, null, "Ionescu", "$2a$10$jg3d2Ltol2A8tyaVg9VoNeG47.fj1iIqGv0ndQpmSxc96JfpBMTLi", "+40740000010", new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), "maria.popescu@online-school.ro", "Maria", true, null, "Popescu", "$2a$10$jg3d2Ltol2A8tyaVg9VoNeG47.fj1iIqGv0ndQpmSxc96JfpBMTLi", "+40740000011", new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), "alexandru.stan@online-school.ro", "Alexandru", true, null, "Stan", "$2a$10$jg3d2Ltol2A8tyaVg9VoNeG47.fj1iIqGv0ndQpmSxc96JfpBMTLi", "+40740000012", new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), "ioana.dumitrescu@online-school.ro", "Ioana", true, null, "Dumitrescu", "$2a$10$jg3d2Ltol2A8tyaVg9VoNeG47.fj1iIqGv0ndQpmSxc96JfpBMTLi", "+40740000013", new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), "radu.georgescu@online-school.ro", "Radu", true, null, "Georgescu", "$2a$10$jg3d2Ltol2A8tyaVg9VoNeG47.fj1iIqGv0ndQpmSxc96JfpBMTLi", "+40740000014", new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), "ana.marinescu@student.online-school.ro", "Ana", true, null, "Marinescu", "$2a$10$jg3d2Ltol2A8tyaVg9VoNeG47.fj1iIqGv0ndQpmSxc96JfpBMTLi", "+40740000020", new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), "vlad.petrescu@student.online-school.ro", "Vlad", true, null, "Petrescu", "$2a$10$jg3d2Ltol2A8tyaVg9VoNeG47.fj1iIqGv0ndQpmSxc96JfpBMTLi", "+40740000021", new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 22, new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), "elena.rusu@student.online-school.ro", "Elena", true, null, "Rusu", "$2a$10$jg3d2Ltol2A8tyaVg9VoNeG47.fj1iIqGv0ndQpmSxc96JfpBMTLi", "+40740000022", new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 23, new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), "matei.enache@student.online-school.ro", "Matei", true, null, "Enache", "$2a$10$jg3d2Ltol2A8tyaVg9VoNeG47.fj1iIqGv0ndQpmSxc96JfpBMTLi", "+40740000023", new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 24, new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), "sofia.tudor@student.online-school.ro", "Sofia", true, null, "Tudor", "$2a$10$jg3d2Ltol2A8tyaVg9VoNeG47.fj1iIqGv0ndQpmSxc96JfpBMTLi", "+40740000024", new DateTime(2026, 4, 3, 9, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "course",
                columns: new[] { "id", "course_name", "department" },
                values: new object[,]
                {
                    { 100, "Algebra Fundamentals", "Mathematics" },
                    { 101, "Java Programming Basics", "Computer Science" },
                    { 102, "Physics Lab Essentials", "Science" },
                    { 103, "Academic Writing in English", "Languages" },
                    { 104, "Modern European History", "Humanities" },
                    { 105, "Data Structures", "Computer Science" }
                });

            migrationBuilder.InsertData(
                table: "administrator",
                columns: new[] { "id", "department", "employee_code", "job_title" },
                values: new object[] { 1, "Operations", "ADM-001", "Platform Administrator" });

            migrationBuilder.InsertData(
                table: "student",
                columns: new[] { "id", "date_of_birth", "enrollment_date", "guardian_email", "guardian_name", "student_code" },
                values: new object[,]
                {
                    { 20, new DateOnly(2008, 3, 12), new DateOnly(2025, 9, 15), "mihai.marinescu@example.com", "Mihai Marinescu", "STD-001" },
                    { 21, new DateOnly(2007, 11, 2), new DateOnly(2025, 9, 15), "alina.petrescu@example.com", "Alina Petrescu", "STD-002" },
                    { 22, new DateOnly(2008, 7, 24), new DateOnly(2025, 9, 15), "cristian.rusu@example.com", "Cristian Rusu", "STD-003" },
                    { 23, new DateOnly(2007, 5, 19), new DateOnly(2025, 9, 15), "gabriela.enache@example.com", "Gabriela Enache", "STD-004" },
                    { 24, new DateOnly(2008, 1, 30), new DateOnly(2025, 9, 15), "daniel.tudor@example.com", "Daniel Tudor", "STD-005" }
                });

            migrationBuilder.InsertData(
                table: "teacher",
                columns: new[] { "id", "bio", "employee_code", "specialization", "title" },
                values: new object[,]
                {
                    { 10, "Coordoneaza curricula de matematica pentru gimnaziu si liceu.", "TCH-001", "Mathematics", "Senior Mathematics Teacher" },
                    { 11, "Preda baze de programare, algoritmi si proiectare software.", "TCH-002", "Computer Science", "Software Engineering Teacher" },
                    { 12, "Acopera fizica experimentala si aplicatii practice in laborator.", "TCH-003", "Physics", "Applied Physics Teacher" },
                    { 13, "Lucreaza pe comunicare academica si pregatire pentru examene internationale.", "TCH-004", "English", "Academic English Teacher" },
                    { 14, "Preda istorie moderna si gandire critica bazata pe surse.", "TCH-005", "History", "Modern History Teacher" }
                });

            migrationBuilder.InsertData(
                table: "user_permissions",
                columns: new[] { "permission", "user_id" },
                values: new object[,]
                {
                    { "ADMINISTRATOR_READ", 1 },
                    { "ADMINISTRATOR_WRITE", 1 },
                    { "AUTH_READ", 1 },
                    { "AUTH_WRITE", 1 },
                    { "BOOK_READ", 1 },
                    { "BOOK_WRITE", 1 },
                    { "COURSE_READ", 1 },
                    { "COURSE_WRITE", 1 },
                    { "DASHBOARD_READ", 1 },
                    { "DASHBOARD_WRITE", 1 },
                    { "ENROLMENT_READ", 1 },
                    { "ENROLMENT_WRITE", 1 },
                    { "PASSWORD_READ", 1 },
                    { "PASSWORD_WRITE", 1 },
                    { "PERMISSION_READ", 1 },
                    { "PERMISSION_WRITE", 1 },
                    { "PROFILE_READ", 1 },
                    { "PROFILE_WRITE", 1 },
                    { "REPORT_READ", 1 },
                    { "REPORT_WRITE", 1 },
                    { "SETTINGS_READ", 1 },
                    { "SETTINGS_WRITE", 1 },
                    { "STUDENT_READ", 1 },
                    { "STUDENT_WRITE", 1 },
                    { "TEACHER_READ", 1 },
                    { "TEACHER_WRITE", 1 },
                    { "USER_READ", 1 },
                    { "USER_WRITE", 1 },
                    { "AUTH_READ", 10 },
                    { "AUTH_WRITE", 10 },
                    { "BOOK_READ", 10 },
                    { "COURSE_READ", 10 },
                    { "COURSE_WRITE", 10 },
                    { "DASHBOARD_READ", 10 },
                    { "DASHBOARD_WRITE", 10 },
                    { "ENROLMENT_READ", 10 },
                    { "ENROLMENT_WRITE", 10 },
                    { "PASSWORD_READ", 10 },
                    { "PASSWORD_WRITE", 10 },
                    { "PROFILE_READ", 10 },
                    { "PROFILE_WRITE", 10 },
                    { "REPORT_READ", 10 },
                    { "STUDENT_READ", 10 },
                    { "TEACHER_READ", 10 },
                    { "TEACHER_WRITE", 10 },
                    { "AUTH_READ", 11 },
                    { "AUTH_WRITE", 11 },
                    { "BOOK_READ", 11 },
                    { "COURSE_READ", 11 },
                    { "COURSE_WRITE", 11 },
                    { "DASHBOARD_READ", 11 },
                    { "DASHBOARD_WRITE", 11 },
                    { "ENROLMENT_READ", 11 },
                    { "ENROLMENT_WRITE", 11 },
                    { "PASSWORD_READ", 11 },
                    { "PASSWORD_WRITE", 11 },
                    { "PROFILE_READ", 11 },
                    { "PROFILE_WRITE", 11 },
                    { "REPORT_READ", 11 },
                    { "STUDENT_READ", 11 },
                    { "TEACHER_READ", 11 },
                    { "TEACHER_WRITE", 11 },
                    { "AUTH_READ", 12 },
                    { "AUTH_WRITE", 12 },
                    { "BOOK_READ", 12 },
                    { "COURSE_READ", 12 },
                    { "COURSE_WRITE", 12 },
                    { "DASHBOARD_READ", 12 },
                    { "DASHBOARD_WRITE", 12 },
                    { "ENROLMENT_READ", 12 },
                    { "ENROLMENT_WRITE", 12 },
                    { "PASSWORD_READ", 12 },
                    { "PASSWORD_WRITE", 12 },
                    { "PROFILE_READ", 12 },
                    { "PROFILE_WRITE", 12 },
                    { "REPORT_READ", 12 },
                    { "STUDENT_READ", 12 },
                    { "TEACHER_READ", 12 },
                    { "TEACHER_WRITE", 12 },
                    { "AUTH_READ", 13 },
                    { "AUTH_WRITE", 13 },
                    { "BOOK_READ", 13 },
                    { "COURSE_READ", 13 },
                    { "COURSE_WRITE", 13 },
                    { "DASHBOARD_READ", 13 },
                    { "DASHBOARD_WRITE", 13 },
                    { "ENROLMENT_READ", 13 },
                    { "ENROLMENT_WRITE", 13 },
                    { "PASSWORD_READ", 13 },
                    { "PASSWORD_WRITE", 13 },
                    { "PROFILE_READ", 13 },
                    { "PROFILE_WRITE", 13 },
                    { "REPORT_READ", 13 },
                    { "STUDENT_READ", 13 },
                    { "TEACHER_READ", 13 },
                    { "TEACHER_WRITE", 13 },
                    { "AUTH_READ", 14 },
                    { "AUTH_WRITE", 14 },
                    { "BOOK_READ", 14 },
                    { "COURSE_READ", 14 },
                    { "COURSE_WRITE", 14 },
                    { "DASHBOARD_READ", 14 },
                    { "DASHBOARD_WRITE", 14 },
                    { "ENROLMENT_READ", 14 },
                    { "ENROLMENT_WRITE", 14 },
                    { "PASSWORD_READ", 14 },
                    { "PASSWORD_WRITE", 14 },
                    { "PROFILE_READ", 14 },
                    { "PROFILE_WRITE", 14 },
                    { "REPORT_READ", 14 },
                    { "STUDENT_READ", 14 },
                    { "TEACHER_READ", 14 },
                    { "TEACHER_WRITE", 14 },
                    { "AUTH_READ", 20 },
                    { "AUTH_WRITE", 20 },
                    { "BOOK_READ", 20 },
                    { "BOOK_WRITE", 20 },
                    { "COURSE_READ", 20 },
                    { "DASHBOARD_READ", 20 },
                    { "ENROLMENT_READ", 20 },
                    { "PASSWORD_READ", 20 },
                    { "PASSWORD_WRITE", 20 },
                    { "PROFILE_READ", 20 },
                    { "PROFILE_WRITE", 20 },
                    { "STUDENT_READ", 20 },
                    { "AUTH_READ", 21 },
                    { "AUTH_WRITE", 21 },
                    { "BOOK_READ", 21 },
                    { "BOOK_WRITE", 21 },
                    { "COURSE_READ", 21 },
                    { "DASHBOARD_READ", 21 },
                    { "ENROLMENT_READ", 21 },
                    { "PASSWORD_READ", 21 },
                    { "PASSWORD_WRITE", 21 },
                    { "PROFILE_READ", 21 },
                    { "PROFILE_WRITE", 21 },
                    { "STUDENT_READ", 21 },
                    { "AUTH_READ", 22 },
                    { "AUTH_WRITE", 22 },
                    { "BOOK_READ", 22 },
                    { "BOOK_WRITE", 22 },
                    { "COURSE_READ", 22 },
                    { "DASHBOARD_READ", 22 },
                    { "ENROLMENT_READ", 22 },
                    { "PASSWORD_READ", 22 },
                    { "PASSWORD_WRITE", 22 },
                    { "PROFILE_READ", 22 },
                    { "PROFILE_WRITE", 22 },
                    { "STUDENT_READ", 22 },
                    { "AUTH_READ", 23 },
                    { "AUTH_WRITE", 23 },
                    { "BOOK_READ", 23 },
                    { "BOOK_WRITE", 23 },
                    { "COURSE_READ", 23 },
                    { "DASHBOARD_READ", 23 },
                    { "ENROLMENT_READ", 23 },
                    { "PASSWORD_READ", 23 },
                    { "PASSWORD_WRITE", 23 },
                    { "PROFILE_READ", 23 },
                    { "PROFILE_WRITE", 23 },
                    { "STUDENT_READ", 23 },
                    { "AUTH_READ", 24 },
                    { "AUTH_WRITE", 24 },
                    { "BOOK_READ", 24 },
                    { "BOOK_WRITE", 24 },
                    { "COURSE_READ", 24 },
                    { "DASHBOARD_READ", 24 },
                    { "ENROLMENT_READ", 24 },
                    { "PASSWORD_READ", 24 },
                    { "PASSWORD_WRITE", 24 },
                    { "PROFILE_READ", 24 },
                    { "PROFILE_WRITE", 24 },
                    { "STUDENT_READ", 24 }
                });

            migrationBuilder.InsertData(
                table: "book",
                columns: new[] { "id", "book_name", "student_id" },
                values: new object[,]
                {
                    { 200, "Algebra Practice Workbook", 20 },
                    { 201, "Java for Beginners", 21 },
                    { 202, "Physics Experiments Handbook", 22 },
                    { 203, "Academic English Companion", 23 },
                    { 204, "History Sourcebook", 24 },
                    { 205, "Advanced Data Structures Notes", 21 },
                    { 206, "Problem Solving in Mathematics", 20 }
                });

            migrationBuilder.InsertData(
                table: "enrolment",
                columns: new[] { "id", "course_id", "student_id" },
                values: new object[,]
                {
                    { 300L, 100, 20 },
                    { 301L, 101, 20 },
                    { 302L, 101, 21 },
                    { 303L, 105, 21 },
                    { 304L, 102, 22 },
                    { 305L, 103, 23 },
                    { 306L, 104, 24 },
                    { 307L, 100, 24 }
                });

            migrationBuilder.CreateIndex(
                name: "uk_administrator_employee_code",
                table: "administrator",
                column: "employee_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uk_app_user_email",
                table: "app_user",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_book_student_id",
                table: "book",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_enrolment_course_id",
                table: "enrolment",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_enrolment_student_id",
                table: "enrolment",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "uk_student_student_code",
                table: "student",
                column: "student_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uk_teacher_employee_code",
                table: "teacher",
                column: "employee_code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "administrator");

            migrationBuilder.DropTable(
                name: "book");

            migrationBuilder.DropTable(
                name: "enrolment");

            migrationBuilder.DropTable(
                name: "teacher");

            migrationBuilder.DropTable(
                name: "user_permissions");

            migrationBuilder.DropTable(
                name: "course");

            migrationBuilder.DropTable(
                name: "student");

            migrationBuilder.DropTable(
                name: "app_user");
        }
    }
}
