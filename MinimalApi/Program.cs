using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/fileName", (IFormFile file, CancellationToken cancellationToken) =>
{
    return file.FileName;
})
.WithName("GetFileName")
.WithDescription("Gets the file name");

app.MapPost("/fileExtension", (IFormFile file, CancellationToken cancellationToken) =>
{
    return Path.GetExtension(file.FileName);
})
.WithName("GetFileExtension")
.WithDescription("Gets the file extension");

app.MapPost("/uploadContent", async (IFormFile file) =>
{
    using var stream = System.IO.File.OpenWrite("upload.txt");
    await file.CopyToAsync(stream);
})
.WithName("UploadContent")
.WithDescription("Uploads the content of the file to a text file");

app.MapPost("/uploadFiles", [EndpointSummary("Uploads multiple files")] async (IFormFileCollection files) =>
    {
        foreach (var file in files)
        {
            var fileName = file.FileName;

            using var stream = System.IO.File.OpenWrite(fileName);
            await file.CopyToAsync(stream);
        }
    })
.WithName("UploadFiles");

app.MapGet("/numberOfGuestsPerTable", (int[] t) => $"table1: {t[0]}, table2: {t[1]}, table3: {t[2]}")
.WithName("Bind query string values to a primitive type array example");

app.MapGet("/fullName", (string[] names) => $"firstName: {names[0]}, lastName: {names[1]}")
.WithName("Bind to a string array example");

app.MapGet("/book", (StringValues names)
        => $"author: {names[0]} , title: {names[1]}")
.WithName("Bind to StringValues example");

app.Run();
