# Prerequisites

## Frameworks & Tooling 🧰

In order to complete the the lessons you need to install the following:

|Prerequisite|Description
|-|-
|[.NET Core 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)|The .NET runtime and SDK. 
|[VSCode](https://code.visualstudio.com/Download) or [VS2022](https://visualstudio.microsoft.com/vs/)|VSCode is great for cross platform. VS2022 is best for Windows only.
|[AZ CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)|The Azure CLI is a command-line tool that you can use to create and manage Azure resources.|
|[VSCode AzureFunctions extension](https://github.com/Microsoft/vscode-azurefunctions)|Extension for VSCode to easily develop and manage Azure Functions.
|[Azure Functions Core Tools](https://github.com/Azure/azure-functions-core-tools)|Azure Functions runtime and CLI for local development.
|[RESTClient for VSCode](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) or [Postman](https://www.postman.com/)|An extension or  application to make HTTP requests.
|[Azure Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer/)|Application to manage Azure Storage resources (both in the cloud and local emulated).
|[Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator) (Windows only) or [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite)|Emulator for using Azure Storage services if you want to develop locally without connecting to a Storage Account in the cloud. If you can't use an emulator you need an [Azure Storage Account](https://docs.microsoft.com/en-us/azure/storage/common/storage-account-create?tabs=azure-portal).

## Codespaces

Alternatively; you can use GitHub CodeSpaces, a feature from GitHub, which provides you with a cloud based development environment. This is a great way to get started with Azure Functions, without having to install anything on your local machine. You do need a GitHub account for this, but you can use a free account. 

From within the repository, click on the `Code` button and select `Open with Codespaces`. This will spin up a new environment for you, and you can start working on the workshop when it has finished provisioning.

Be sure to stop the Codespace when you are done, to avoid unnecessary costs.

> Note: when you are using a codespace, Azurite is running along the dotnet image. You will need to use a different connection string in this case: `DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://azurite:10000/devstoreaccount1;QueueEndpoint=http://azurite:10001/devstoreaccount1;TableEndpoint=http://azurite:10002/devstoreaccount1;`

## Azure 

For the second part of the workshop, you will need an Azure subscription. If you don't have an Azure Subscription, install [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio#install-and-run-azurite).
Get your free Azure subscription [here](https://azure.microsoft.com/en-us/free). You will need a credit card, but it won't be charged, provided you clean up all resources after completing this lab. If you have a MSDN account, you most likely have Azure Credits as well. 


## Cloning the workshop lab files

The files inside this repository contain everything you need to continue with the workshop at a later time. It also contains the completed solutions for each of the labs. You can use this to check how things could be at the end, or to take a peek when you are stuck and need a hint. The lab files should function, apart from some light tweaking of connection strings to actual Azure resources. For obvious reasons, these are not included in the code and configuration of this repository.

- Open a terminal window or command prompt and change your working directory to your usual place where you clone Git repositories, e.g. `C:\Sources` on Windows or `~/sources` on Linux. 

    ```cmd
    cd \Sources     (Windows)
    cd ~/Sources    (Linux)
    ```
- Clone this Git repository:

    ```cmd
    git clone https://github.com/XpiritBV/AzureFunctionsWorkshop.git
    ```

## Creating your local workspace 👩‍💻

We suggest you create a new folder dedicated to your own work, at a convenient location such as `workshop`.

- Inside your sources folder from the previous step, create a new folder to add your workshop files to:

    ```cmd
    mkdir workshop
    cd .\workshop
    ```

- Turn the `workshop` folder into a git repository:

    ```cmd
    git init
    ```

- Add subfolders for the source code and test files:

    ```cmd
    mkdir src
    mkdir tst
    ```

You should be good to go now!

---
[◀ back to README](../README.md)
