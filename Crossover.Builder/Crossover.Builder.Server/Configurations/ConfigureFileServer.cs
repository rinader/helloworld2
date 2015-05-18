using System;
using System.IO;
using System.Reflection;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

// ReSharper disable once CheckNamespace

namespace Crossover.Builder.Server
{
    public partial class Startup
    {
        public void ConfigureFileServer(IAppBuilder app)
        {
            var rootUriString = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().GetName().CodeBase) ?? string.Empty;
            if (rootUriString.Length > 0 && rootUriString[rootUriString.Length - 1] != '/') rootUriString += "//";
            var root = new Uri(new Uri(rootUriString), "Site").LocalPath;

            var physicalFileSystem = new PhysicalFileSystem(root);
            var options = new FileServerOptions
            {
                EnableDefaultFiles = true,
                FileSystem = physicalFileSystem
            };
            options.StaticFileOptions.FileSystem = physicalFileSystem;
            options.StaticFileOptions.ServeUnknownFileTypes = true;
            options.DefaultFilesOptions.DefaultFileNames = new[] {"index.html"};
            app.UseFileServer(options);
        }
    }
}