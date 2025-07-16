using ProgChess.VM.Simulation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policy  =>
        {
            policy.WithOrigins("http://localhost:5290").AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed(host => true);
        });
});


// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddScoped<VmService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");
app.MapControllers();
app.Run();
