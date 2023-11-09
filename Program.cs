using Hangfire;
using Hangfire.SqlServer;
using HangfirePracticeAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(x => x
       .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), 
       new SqlServerStorageOptions
       {
           SlidingInvisibilityTimeout = TimeSpan.FromMinutes(30),
           QueuePollInterval = TimeSpan.Zero,
           UseRecommendedIsolationLevel = true,
           DisableGlobalLocks = true,
           CommandBatchMaxTimeout = TimeSpan.FromMinutes(30)
       }));

builder.Services.AddHangfireServer();
builder.Services.AddScoped<IJobTestService, JobTestService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHangfireDashboard("/dashboard");

app.MapControllers();

app.Run();
