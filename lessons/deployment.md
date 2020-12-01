# Deployment

## Goal 🎯

The goal of this lesson is to create the Azure resources which are required to run (and monitor) our Function App.

You'll need an Azure account in order to complete this lesson.

This lesson consists of the following exercises:

|Nr|Exercise
|-|-
|1|[Azure Resources](#1-azure-resources)
|2|[Creating Resources](#2-creating-resources)
|3|[Publishing the Solution](#3-publishing-the-solution)

## 1. Azure Resources

Up to now we've ran our Function App locally using an emulated storage account. In order to run the application in Azure we need the following:

- A resource group, which is a logical container for our Azure resources
- A storage account, which is used to store our Function App files and is also used by Durable Functions.
- A Function App resource
- Optional but highly recommended: Application Insights, which is used for monitoring and diagnosing our application.

## 2. Creating Resources

There are many different ways to create Azure resources; via the portal, the Azure CLI, ARM templates, and many cloud provision tools.

Here we'll show some samples how the Azure CLI can be used. But feel free to use the tools which work best for you.

In a command prompt type: `az login` in order to login with your Azure credentials.

> 🔎 __Observation__ - In the examples below we're using a Powershell specific syntax for variables (e.g. `$location`) which are used in arguments. You need to change the syntax of these variables when we're not using Powershell.

> 📝 __Tip__ - In order to see what options are available for a given CLI command use the `-h` argument, such as `az group -h` to see the available subcommands for resource groups.

Once logged in proceed with the following commands:

### Steps

1. See the available subscriptions

    `az account list`

2. Set the desired subscription

    `az account set -s <id>`

3. Create a resource group

    `$location="westeurope"`

    `$rgname="game-highscore-rg"`

    `az group create --name $rgname --location $location --tags type=labs`

4. Create a Storage Account

    `$stname="gamehighscorest"`

    `az storage account create --name $stname --resource-group $rgname --location $location --sku Standard_LRS --kind StorageV2 --access-tier Hot`

5. Add & Create application insights. Application Insights is not available by default in the Azure CLI and needs to be added first:

    `az extension add --name application-insights`

    `$ainame="game-highscore-ai"`

    `az monitor app-insights component create --app $ainame --location $location --application-type web --kind web --resource-group $rgname`

6. Create the Function App

    `$funcAppName="game-highscore-fa"`

    `az functionapp create --name $funcAppName --resource-group $rgname --consumption-plan-location $location --storage-account $stname --app-insights $ainame --runtime dotnet --os-type Windows`

    > ❔ __Question__ - Inspect the above CLI command. What can you tell about the configuration of the Function App?

## 3. Publishing the Solution

The goal of this exercise is to publish our application to Azure.

As with provisioning, publishing our Function App can be done in different ways. You can do it straight from our IDE if that supports Azure, via the Azure Functions CLI, or via a build & release pipeline in Azure DevOps.

### Option 1: Publish from Visual Studio

Follow these instructions: [docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs#publish-to-azure](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs#publish-to-azure)

Note that we should select to publish to an existing Function App resource if we provisioned one in the previous exercise.

Application settings in the `local.settings.json` file are not published. Follow these instructions to manage our app settings from Visual Studio: [docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs#function-app-settings](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs#function-app-settings)

### Option 2: Publish from VS Code

Follow these instructions: [docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code#publish-to-azure](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code?tabs=nodejs#publish-to-azure)

Application settings in the `local.seetings.json` file are not published. Follow these instructions to manage our app settings from VS Code: [docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code#application-settings-in-azure](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code?tabs=nodejs#application-settings-in-azure)

### Option 3: Publish from Azure Functions CLI

Follow these instructions: [docs.microsoft.com/en-us/azure/azure-functions/functions-run-local#publish](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local#publish)

### Option 4: Continuous delivery with Azure DevOps

Follow these instructions: [https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-azure-devops?tabs=csharp](https://docs.microsoft.com/en-us/azure/azure-functions/functions-how-to-azure-devops?tabs=csharp).

---
[◀ back to README](../README.md)