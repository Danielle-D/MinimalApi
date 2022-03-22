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

app.Run();
