# Azure Functions Workshop 

Welcome to the Azure Functions Workshop! This repo contains some lessons and source code which helps you learn the basics of .NET Core based Azure Functions and related Azure services.

You will build a modern web application, using Azure Functions and .NET 6. Once completed, you will create Azure infrastructure by using the `az` cli, and run the app in the Cloud.

## Required knowledge

Even though this is an introductory workshop, you will need to bring some experience using C# and a basic understanding of the resource model of Azure.

## What you will learm

In the workshop, you will learn how to create an Azure Function app, that uses a number of Azure components to calculate score. You get introduced to queues, table storage, signalR and appservices inside Azure. We will also show you how to provision this application in the cloud as well by creating the needed infrastructure and deploying it.

## Lessons

- [Lesson 1 - Prerequisites](/lessons/prerequisites.md) - Prepare for the workshop by installing some tools onto your system ahead of time.
- [Lesson 2 - Creating a HTTP trigger](/lessons/http.md) - Create a simple HTTP trigger function. 
- [Lesson 3 - Queue Trigger & Binding](/lessons/queue.md) - Setup a queue trigger and binding to a function. 
- [Lesson 4 - Table Bindings](/lessons/table.md) - React on table changes by using table bindings and store results in a table. 
- [Lesson 5 - Assignment](/lessons/assignment.md) - Combine all the lessons into a single application.
- [Lesson 6 - Unit testing](/lessons/unittesting.md) - Add unit tests to your functions.
- [Lesson 7 - Run in the Cloud](/lessons/deployment.md) - Deploy your application to Azure.

## Get started

Clone this repository to your local machine:
    ```cmd
    git clone https://github.com/XpiritBV/AzureFunctionsWorkshop.git
    ```

Or click on the below button:

[![](https://img.shields.io/badge/-Use%20this%20template-brightgreen?style=for-the-badge&logo=github)](https://github.com/xpiritbv/azurefunctionsworkshop/generate)

To start the presentation; run `npm i` and `npm run start` and open the `presentation.md` file or visit the deployed presentation at [GitHub Pages](https://xpiritbv.github.io/AzureFunctionsWorkshop/)