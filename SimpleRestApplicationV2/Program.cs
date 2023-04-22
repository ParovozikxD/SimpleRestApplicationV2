using Microsoft.EntityFrameworkCore;
using SimpleRestApplicationV2;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("default") ?? throw new ArgumentNullException("Database string is not founded");

builder.Services.AddDbContext<ApplicationContext>((options) => 
    options.UseSqlite(connection));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/users", (ApplicationContext db) =>
    {
        var users = db.Users.ToList();

        if(users == null)
        {
            return Results.NotFound("Cannot find the Users resource");
        }

        return Results.Json(users);
    }
);

app.MapPost("/users", async (HttpContext context, ApplicationContext db) =>
    {
        User requestedUser;

        try
        {
            requestedUser = await context.Request.ReadFromJsonAsync<User>();

            if (string.IsNullOrEmpty(requestedUser.Name) || string.IsNullOrEmpty(requestedUser.Name))
            {
                throw new ArgumentNullException("One of params is Empty");
            }
        }
        catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);        
        }

        var newUser = new User(requestedUser.Name, requestedUser.Age);

        db.Users.Add(newUser);
        db.SaveChanges();

        return Results.Json(newUser);
    }
);

app.MapDelete("/users/{userID:guid}", async (ApplicationContext db, string userID) =>
    {
        var userForRemove = db.Users.Find(userID);

        if (userForRemove == null)
        {
            return Results.NotFound("Cannot found the User");
        }
        
        db.Users.Remove(userForRemove);
        db.SaveChanges();

        return Results.Json(userForRemove);
    }
);

app.MapPut("/users/{userID:guid}", async (HttpContext context, ApplicationContext db, string userID) =>
{
    var userForUpdate = db.Users.Find(userID);

    if (userForUpdate == null)
    {
        return Results.NotFound("Cannot found the User");
    }

    User requestedUser;

    try
    {
        requestedUser = await context.Request.ReadFromJsonAsync<User>();


        if (string.IsNullOrEmpty(requestedUser.Name) || string.IsNullOrEmpty(requestedUser.Name))
        {
            throw new ArgumentNullException("One of params is Empty");
        }
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
    

    userForUpdate.Name = requestedUser.Name;
    userForUpdate.Age = requestedUser.Age;

    db.SaveChanges();
  
    return Results.Json(userForUpdate);
}
);

app.Run();
