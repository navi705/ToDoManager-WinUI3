# Introduction
**TodoManager**-is a WinUI 3 ***training project*** is designed to manage, record your routine tasks.

Brief description of the process:
* When starting up for the first time, register and log in. (More screenshots in [Pages](/Source/Docs))
* Add a task and set up a task
* Execute the task by clicking on the ticked button.(Notifications arrive half an hour before the appointed time)
* Go to the "TimeManager" page your task will automatically be added to the schedule if you need to correct the time.

Features:

* The theme of the application depends on the selected accent colour.
* Russian and English localisations available
* Toast notification available
* NotifyIcon and minimise to system tray are available
## Windows 11
![](/Docs/LoginPage.png)
## Windows 10
![](/Docs/ScreenshotWindows10.png)
# Architectures and patterns
* MVVM
* Inversion of Control / Dependency injection
* Service
* Observer pattern
# Techical Stack
* ASP.NET .Net 7
* MongoDB
* Swagger UI
* WinUI 3 App SDK 1.2.221109.1 .Net 7
* H.NotifyIcon.WinUI
* Moq
* xUnit
# How to run
## App
Check that you have the APK SDK and .Net 7 installed. Download zip or clone this repository.

Install [MongoDB](https://www.mongodb.com/try/download/community) and create a database named
"TaskManager" with the "User" and "TestUser" collections for tests. And in each of these collections, create unique 'email' indexes. (Please advise if there is another solution for deployment other than the Docker)

Open the downloaded solution in two Visual Studios. In the first one, select ToDoManager.Asp.Net.Core and run it until Swagger opens. After Swagger is activated, open the second Visual Studio and run the ToDoManager.

Unfortunately, I have not found how to start up projects sequentially (multiple startup projects are not suitable).
## Tests
If you have installed and created a "TestUser" collection open the top panel and press Run All Test in Visual Studio.
![](/Docs/TestIsValid.png)
# References
* https://github.com/microsoft/WinUI-3-Demos/tree/master/src/ContosoAirlinePOS
