using AopWebAdmin;
using AopWebAdmin.CloudStorage;
using AopWebAdmin.Pages;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

AopsImporter.Init();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<ICloudStorage, GoogleCloudStorage>();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddSingleton(new MongoDbProvider()
    .GetDatabase(builder.Configuration.GetValue<string>("DatabaseConnectionString")!, builder.Configuration.GetValue<string>("DatabaseName")!));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.AccessDeniedPath = "/";
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
    });

// Add a global authorization policy
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.AddPolicy("ViewUsers", policy =>
        policy.RequireClaim("ViewUsers", "ViewUsers"));
    
    options.AddPolicy("EditUsers", policy =>
        policy.RequireClaim("EditUsers", "EditUsers"));

    options.AddPolicy("DeleteUsers", policy =>
        policy.RequireClaim("DeleteUsers", "DeleteUsers"));

    options.AddPolicy("AddUsers", policy =>
        policy.RequireClaim("AddUsers", "AddUsers"));

    options.AddPolicy("ViewProblems", policy =>
        policy.RequireClaim("ViewProblems", "ViewProblems"));
    
    options.AddPolicy("EditProblems", policy =>
        policy.RequireClaim("EditProblems", "EditProblems"));

    options.AddPolicy("DeleteProblems", policy =>
        policy.RequireClaim("DeleteProblems", "DeleteProblems"));

    options.AddPolicy("AddProblems", policy =>
        policy.RequireClaim("AddProblems", "AddProblems"));

    options.AddPolicy("ViewQuizzes", policy =>
        policy.RequireClaim("ViewQuizzes", "ViewQuizzes"));
    
    options.AddPolicy("EditQuizzes", policy =>
        policy.RequireClaim("EditQuizzes", "EditQuizzes"));

    options.AddPolicy("DeleteQuizzes", policy =>
        policy.RequireClaim("DeleteQuizzes", "DeleteQuizzes"));

    options.AddPolicy("AddQuizzes", policy =>
        policy.RequireClaim("AddQuizzes", "AddQuizzes"));

    options.AddPolicy("ViewProblemsets", policy =>
        policy.RequireClaim("ViewProblemsets", "ViewProblemsets"));
    
    options.AddPolicy("EditProblemsets", policy =>
        policy.RequireClaim("EditProblemsets", "EditProblemsets"));

    options.AddPolicy("DeleteProblemsets", policy =>
        policy.RequireClaim("DeleteProblemsets", "DeleteProblemsets"));

    options.AddPolicy("AddProblemsets", policy =>
        policy.RequireClaim("AddProblemsets", "AddProblemsets"));

    options.AddPolicy("ViewStorage", policy =>
        policy.RequireClaim("ViewStorage", "ViewStorage"));
    
    options.AddPolicy("EditStorage", policy =>
        policy.RequireClaim("EditStorage", "EditStorage"));

    options.AddPolicy("DeleteStorage", policy =>
        policy.RequireClaim("DeleteStorage", "DeleteStorage"));

    options.AddPolicy("AddStorage", policy =>
        policy.RequireClaim("AddStorage", "AddStorage"));

    options.AddPolicy("ViewLessons", policy =>
        policy.RequireClaim("ViewLessons", "ViewLessons"));
    
    options.AddPolicy("EditLessons", policy =>
        policy.RequireClaim("EditLessons", "EditLessons"));

    options.AddPolicy("DeleteLessons", policy =>
        policy.RequireClaim("DeleteLessons", "DeleteLessons"));

    options.AddPolicy("AddLessons", policy =>
        policy.RequireClaim("AddLessons", "AddLessons"));

    options.AddPolicy("ViewUnits", policy =>
        policy.RequireClaim("ViewUnits", "ViewUnits"));
    
    options.AddPolicy("EditUnits", policy =>
        policy.RequireClaim("EditUnits", "EditUnits"));

    options.AddPolicy("DeleteUnits", policy =>
        policy.RequireClaim("DeleteUnits", "DeleteUnits"));

    options.AddPolicy("AddUnits", policy =>
        policy.RequireClaim("AddUnits", "AddUnits"));

    options.AddPolicy("ViewGroups", policy =>
        policy.RequireClaim("ViewGroups", "ViewGroups"));
    
    options.AddPolicy("EditGroups", policy =>
        policy.RequireClaim("EditGroups", "EditGroups"));

    options.AddPolicy("DeleteGroups", policy =>
        policy.RequireClaim("DeleteGroups", "DeleteGroups"));

    options.AddPolicy("AddGroups", policy =>
        policy.RequireClaim("AddGroups", "AddGroups"));

    options.AddPolicy("ViewSubmissions", policy =>
        policy.RequireClaim("ViewSubmissions", "ViewSubmissions"));
    
    options.AddPolicy("EditSubmissions", policy =>
        policy.RequireClaim("EditSubmissions", "EditSubmissions"));

    options.AddPolicy("DeleteSubmissions", policy =>
        policy.RequireClaim("DeleteSubmissions", "DeleteSubmissions"));

    options.AddPolicy("AddSubmissions", policy =>
        policy.RequireClaim("AddSubmissions", "AddSubmissions"));

    options.AddPolicy("ViewEmails", policy =>
        policy.RequireClaim("ViewEmails", "ViewEmails"));
    
    options.AddPolicy("EditEmails", policy =>
        policy.RequireClaim("EditEmails", "EditEmails"));

    options.AddPolicy("DeleteEmails", policy =>
        policy.RequireClaim("DeleteEmails", "DeleteEmails"));

    options.AddPolicy("AddEmails", policy =>
        policy.RequireClaim("AddEmails", "AddEmails"));

    options.AddPolicy("ViewExams", policy =>
        policy.RequireClaim("ViewExams", "ViewExams"));
    
    options.AddPolicy("EditExams", policy =>
        policy.RequireClaim("EditExams", "EditExams"));

    options.AddPolicy("DeleteExams", policy =>
        policy.RequireClaim("DeleteExams", "DeleteExams"));

    options.AddPolicy("AddExams", policy =>
        policy.RequireClaim("AddExams", "AddExams"));
 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();