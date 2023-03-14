using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ToDoManager.Asp.Net.Core.Models;
using ToDoManager.Asp.Net.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//swagger
builder.Services.AddMvc();
builder.Services.AddMvc(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title="Api",Description="Asp.Net" }) ; });


builder.Services.Configure < MongoDbConnection>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<MongoDbService>();


builder.Services.AddAuthorization();


builder.Services.Configure<JwtOption>(builder.Configuration.GetSection("JWT"));
builder.Services.AddSingleton<JwtService>();
var authOption = builder.Configuration.GetSection("JWT").Get<JwtOption>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            ValidIssuer = authOption.Issuer,
            ValidateAudience = true,
            ValidAudience = authOption.Audience,
            ValidateLifetime = false,

            // установка ключа безопасности
            IssuerSigningKey = authOption.GetSymmetricSecurityKey(),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    });
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
//debug
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/", () => "Hello World!");



app.Run();

