# Prerequisites

## Frameworks & Tooling 🧰

In order to complete the the lessons you need to install the following:

|Prerequisite|Description
|-|-
|[.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core)|The .NET runtime and SDK.
|[VSCode](https://code.visualstudio.com/Download) or [VS2019](https://visualstudio.microsoft.com/vs/)|VSCode is great for cross platform. VS2019 is best for Windows only.
|[VSCode AzureFunctions extension](https://github.com/Microsoft/vscode-azurefunctions)|Extension for VSCode to easily develop and manage Azure Functions.
|[Azure Functions Core Tools](https://github.com/Azure/azure-functions-core-tools)|Azure Functions runtime and CLI for local development.
|[RESTClient for VSCode](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) or [Postman](https://www.postman.com/)|An extension or  application to make HTTP requests.
|[Azure Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer/)|Application to manage Azure Storage resources (both in the cloud and local emulated).
|[Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator) (Windows only) or [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite)|Emulator for using Azure Storage services if you want to develop locally without connecting to a Storage Account in the cloud. If you can't use an emulator you need an [Azure Storage Account](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-create?tabs=azure-portal).

## Creating your local workspace 👩‍💻

We strongly suggest you create a new folder (local git repository) and use this repository for reference only (for when you're stuck).

- Create a new folder to work in:

    ```cmd
    C:\dev\mkdir functionsworkshop
    C:\dev\cd .\functionsworkshop\
    ```

- Turn this into a git repository:

    ```cmd
    C:\dev\functionsworkshop\git init
    ```

- Add subfolders for the source code and test files:

    ```cmd
    C:\dev\functionsworkshop\mkdir src
    C:\dev\functionsworkshop\mkdir tst
    ```

You should be good to go now!

---
[◀ back to README](README.md)
