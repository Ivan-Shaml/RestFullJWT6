using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using RESTJwt.Data;
using RESTJwt.Data.Contracts;
using RESTJwt.Data.Repositories;
using RESTJwt.Models;
using RESTJwt.Models.DTOs;
using RESTJwt.Services;
using RESTJwt.Services.Contracts;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using RESTJwt.Providers;
using RESTJwt.Providers.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    };
});

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

// Register repositories in the dependency injection container
builder.Services.AddTransient<IMovieRepository, MovieRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddSingleton<IPasswordProvider, SHA256WithoutSaltPasswordProvider>();

builder.Services.AddTransient<IMovieService, MovieService>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Bearer Authenication with JWT Token",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Id = "Bearer",
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                }
            },
            new List<string>()
        }
    });
});


var app = builder.Build();

app.UseSwagger();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!").ExcludeFromDescription();

app.MapPost("/create",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
    (MovieDTO movie, IMovieService service) => Create(movie, service))
        .Accepts<MovieDTO>("application/json")
        .Produces<Movie>(statusCode: 200, contentType: "application/json");

app.MapGet("/get",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Standard, Administrator")]
    (int id, IMovieService service) => Get(id, service))
        .Produces<Movie>(statusCode: 200, contentType: "application/json");

app.MapGet("/list",
    (IMovieService service) => List(service))
        .Produces<List<Movie>>(statusCode: 200, contentType: "application/json");

app.MapPut("/update",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
    (Movie newMovie, IMovieService service) => Update(newMovie, service))
        .Accepts<Movie>("application/json")
        .Produces<Movie>(statusCode: 200, contentType: "application/json");

app.MapDelete("/delete",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
    (int id, IMovieService service) => Delete(id, service))
        .Accepts<int>("application/json")
        .Produces<Boolean>(statusCode: 200, contentType: "application/json");

app.MapPost("/login",
    (UserLoginDTO user, IUserService service) => UserLogin(user, service))
        .Accepts<UserLoginDTO>("application/json")
        .Produces<string>();

app.MapPost("/register",
    (UserRegisterDTO userRegister, IUserService service) => UserRegister(userRegister, service))
        .Accepts<UserRegisterDTO>("application/json")
        .Produces<string[]>();
app.MapPut("/changeRole",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(string username, IUserService service) => ChangeRole(username, service))
        .Accepts<string>("application/json")
        .Produces<string>(statusCode: 200, contentType: "application/json");

IResult Create(MovieDTO movie, IMovieService service)
{
    var result = service.Create(movie);
    return Results.Ok(result);
}

IResult Get(int id, IMovieService service)
{
    var movie = service.Get(id);
    
    if (movie is null)
    {
        return Results.NotFound("Movie not found");
    }

    return Results.Ok(movie);
}

IResult List(IMovieService service)
{
    var result = service.List();
    return Results.Ok(result);
}

IResult Update(Movie newMovie, IMovieService service)
{
    var result = service.Update(newMovie);
    if (result is null)
    {
        return Results.NotFound("Movie not found.");
    }
    return Results.Ok(result);
}

IResult Delete(int id, IMovieService service)
{
    var isSucessfull = service.Delete(id);
    if (!isSucessfull)
    {
        return Results.BadRequest("Something went wrong.");
    }
    return Results.Ok(isSucessfull);
}

string GenerateJwtToken(User loggedInUser)
{
    var claims = new[]
       {
            new Claim(ClaimTypes.NameIdentifier, loggedInUser.Username),
            new Claim(ClaimTypes.Email, loggedInUser.EmailAddress),
            new Claim(ClaimTypes.GivenName, loggedInUser.FirstName),
            new Claim(ClaimTypes.Surname, loggedInUser.LastName),
            new Claim(ClaimTypes.Role, loggedInUser.Role),
        };

    var token = new JwtSecurityToken
    (
        issuer: builder.Configuration["Jwt:Issuer"],
        audience: builder.Configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddDays(30),
        notBefore: DateTime.UtcNow,
        signingCredentials: new SigningCredentials(
            new SymmetricSecurityKey
            (
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                SecurityAlgorithms.HmacSha256
            )
    );

    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

    return tokenString;

}

IResult UserLogin(UserLoginDTO user, IUserService service)
{
    if (!string.IsNullOrWhiteSpace(user.Username) && 
        !string.IsNullOrWhiteSpace(user.Password))
    {
        var loggedInUser = service.Get(user);
        
        if (loggedInUser is null)
        {
            return Results.BadRequest("Invalid username or password.");
        }

        var tokenString = GenerateJwtToken(loggedInUser);

        return Results.Ok(tokenString);
    }

    return Results.BadRequest("Invalid username or password.");
}

IResult UserRegister(UserRegisterDTO userRegister, IUserService service)
{
    if (userRegister.Password != userRegister.RepeatPassword)
    {
        return Results.BadRequest(new[] { "Passwords don't match." });
    }

    IEnumerable<string> resultsList = service.DoesUserExists(userRegister);
    
    if (resultsList is null)
    {
        User newUser = service.Create(userRegister);
        string jwt = GenerateJwtToken(newUser);

        return Results.Ok(jwt);
    }

    return Results.BadRequest(resultsList.ToArray());
}

IResult ChangeRole(string username, IUserService service)
{
    var result = service.ChangeRole(username);

    if (result is null)
    {
        return Results.BadRequest("User not found.");
    }

    return Results.Ok(result);
}

app.UseSwaggerUI();

app.Run();
