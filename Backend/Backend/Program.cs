using Backend.Extensions;
using Backend.Infrastructure;
using Backend.Services;
using Backend.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Firestore
builder.Services.AddFirestoreDb(builder.Configuration);

// Register services
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddFirebaseAuth(builder.Configuration)
    .AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseFirebaseAuth();

app.Run();