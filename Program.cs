using MyRedditAPI;
using MyRedditAPI.Options;
using MyRedditAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<CredentialsOptions>(
    builder.Configuration.GetSection(CredentialsOptions.Credentials));

builder.Services.Configure<SubRedditOptions>(
    builder.Configuration.GetSection(SubRedditOptions.Credentials));
builder.Services.AddSingleton<RedditHostedService>();
builder.Services.AddHostedService<RedditHostedService>();
builder.Services.AddSingleton<IRedditRepository, RedditRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
