// See https://aka.ms/new-console-template for more information
using SeedGenerator.Lib.Image.Composers;

Console.WriteLine("Hello, World!");

ChequeComposer composer = new ChequeComposer(96);
composer.Compose(new Dictionary<string, string>());