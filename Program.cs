using FluentValidation;
using HtmxRazorSlices.Data;
using HtmxRazorSlices.Features.Home;
using HtmxRazorSlices.Features.ToDoFeature;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddSingleton<IToDoDb>(new ToDoDb());
builder.Services.AddScoped<IUserIdentifierService, UserIdentifierService>();

builder.Services.AddAntiforgery();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.RegisterHomeFeature();
app.RegisterToDoFeature();


app.Run();