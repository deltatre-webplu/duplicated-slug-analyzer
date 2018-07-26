# Duplicated Slug Analyzer

This is a simple console app to get a feedback about duplicated slug inside your system.

Given the guishell base URL, the name of the Forge application registered under guishell and the secret to call the guishell admin API, this application will check your backoffice and distribution databases and will produce a JSON report with the duplicated slug reservation keys found.

**NOTE**: we consider a *SlugReservationKey* to be a key composed by *Slug*, *EntityType*, *EntityCode* and *Culture*. 

## Configuration

There are 3 mandatory configurations (**if you don't provide these settings the application will exit doing nothing**): 
 - *guishellBaseUrl*: the base URL of your Guishell installation
 - *applicationName*: the name of Forge application registered under Guishell
 - *guishellSecret*: the secret to be used to call Guishell admin API
 
You can also specify the following optional configurations:
- *reportDirectoryPath*: the absolute path under which you want to save the JSON file containing the report produced by the application. If not specified the report will be saved under the folder *Reports* located at the same level of the executable file
- *logsDirectoryPath*: the absolute path under which you want to save the application logs. If not specified the logs will be saved under the folder *Logs* located at the same level of the executable file

You can configure the application by using JSON configuration file, environment variables and command line arguments.  

Following rules hold true for the available configuration modes:
- environment variables override JSON configuration file
- command line arguments override environment variables
- JSON configuration file must be called *appsettings.json* and must be placed at the same level of the executable file
- **any configuration mode you decide to use the settings must be named as indicated above**

Here is an example of JSON configuration file containing dummy data: 
```
{
	"guishellBaseUrl": "YOUR GUISHELL BASE URL",
	"applicationName": "NAME OF FORGE APPLICATION REGISTERED UNDER GUISHELL",
	"guishellSecret": "YOUR GUISHELLL ADMIN API SECRET",
	"reportDirectoryPath": "C:\\Temp\\duplicate-slug-analyzer\\Reports",
	"logsDirectoryPath": "C:\\Temp\\duplicate-slug-analyzer\\Logs"
}
```

## Invoke from command line

To invoke the application from command line and pass command line arguments, type the following command in your command line:
```
DuplicatedSlugAnalyzer.exe --guishellBaseUrl="YOUR GUISHELL BASE URL" --applicationName="NAME OF FORGE APPLICATION REGISTERED UNDER GUISHELL" --guishellSecret="YOUR GUISHELLL ADMIN API SECRET"
```
