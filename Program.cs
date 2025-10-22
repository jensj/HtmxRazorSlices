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

// Configure database storage
var dbStorageOptions = builder.Configuration.GetSection(DbStorageOptions.SectionName).Get<DbStorageOptions>() ?? new DbStorageOptions();

IToDoDb todoDb = dbStorageOptions.StorageType.ToLowerInvariant() switch
{
    "filesystem" => new FileSystemToDoDb(dbStorageOptions.LocalStoragePath ?? "./data"),
    "azureblob" => new AzureBlobToDoDb(
        dbStorageOptions.AzureBlobConnectionString ?? throw new InvalidOperationException("AzureBlobConnectionString is required for AzureBlob storage type"),
        dbStorageOptions.AzureBlobContainerName ?? "todos"),
    "inmemory" => new ToDoDb(),
    _ => throw new InvalidOperationException($"Unknown storage type: {dbStorageOptions.StorageType}")
};

builder.Services.AddSingleton<IToDoDb>(todoDb);
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