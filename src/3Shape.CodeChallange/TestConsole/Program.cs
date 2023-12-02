// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Ports;

Console.WriteLine("Hello, World!");

var serviceCollection = new ServiceCollection();
serviceCollection.AddServices();
var serviceProvider = serviceCollection.BuildServiceProvider().CreateScope().ServiceProvider;

var dataImporter = serviceProvider.GetRequiredService<IDataImporter>();
var ImportedBooks = dataImporter.ReadBooks("Book:\r\nAuthor: Brian Jensen\r\nTitle: Texts from Denmark\r\nPublisher: Gyldendal\r\nPublished: 2001\r\nNumberOfPages: 253\r\n\u00a0\r\nBook:\r\nAuthor: Peter Jensen\r\nAuthor: Hans Andersen\r\nTitle: Stories from abroad\r\nPublisher: Borgen\r\nPublished: 2012\r\nNumberOfPages: 156");

var dataAccessor = serviceProvider.GetRequiredService<IdataAccessor>();
var SearchedBooks = dataAccessor.FindBooks("*Jensen*");

Console.ReadLine();