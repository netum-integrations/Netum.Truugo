# Netum.Truugo.EDIFACT

Task for calling Truugo EDIFACT API endpoints. You can refer to Truugo API Swagger here: https://api.truugo.com/reference/. Please note that in order to use this task, you need Truugo API Credentials.

[![EDIFACT_build](https://github.com/FrendsPlatform/Netum.Truugo/actions/workflows/EDIFACT_test_on_main.yml/badge.svg)](https://github.com/FrendsPlatform/Netum.Truugo/actions/workflows/EDIFACT_test_on_main.yml)
![Coverage](https://app-github-custom-badges.azurewebsites.net/Badge?key=FrendsPlatform/Netum.Truugo/Netum.Truugo.EDIFACT|main)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

## Installing

You can install the Task via Frends UI Task View.

## Building

### Clone a copy of the repository

`git clone https://github.com/FrendsPlatform/Netum.Truugo.git`

### Build the project

`dotnet build`

### Run tests

Before running tests, you must add a valid EDIFACT file to TestFiles-folder and specify the file in .env. Add your Truugo API credentials to the .env file.

Run the tests

`dotnet test`

Get unit test coverage

`dotnet test --collect:"XPlat Code Coverage"`

### Create a NuGet package

`dotnet pack --configuration Release`

### StyleCop.Analyzers Version
This project uses StyleCop.Analyzers 1.2.0-beta.556, as recommended by the author, to get the latest fixes and improvements not available in the last stable release.
