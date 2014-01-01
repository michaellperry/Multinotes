# Multinotes
An example application of the [Correspondence collaboration framework](http://correspondencecloud.com).

Correspondence caches data on mobile devices, and queues changes to submit when a network connection becomes available. This example application is a simple discussion forum through which users can collaborate across devices.

## Target platforms
- Windows Store
- Windows Phone
- ASP.NET MVC
- WPF
- Silverlight (coming soon)

## Prerequisites
- Visual Studio 2013
- Windows Phone 8.0 SDK
- Silverlight 5 SDK

Fork the repository, pull the source code, and restore the NuGet packages. One of these packages includes a tool that compiles a Factual model into portable C#. Transform the T4 templates to run this tool (**Build: Transform All T4 Templates**).