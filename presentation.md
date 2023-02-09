---
marp: true
title: 'Azure Functions on Azure workshop'
theme: gaia
class: 
  - lead
  - invert
paginate: true
keywords: Azure Functions, Azure, Workshop
description: Azure Functions workshop
author: Xpirit
---

# <!--fit--> Workshop: Serverless .NET Core Solutions using Azure Functions


---

## Hands-on labs for Azure Functions

 * Lab 1 - Azure Functions 101​
 * Lab 2 – Build an end-to-end solution​
 * Lab 3 – Unit testing your functions ​
 * Lab 4 – Deploying to Azure

---

Browse to: https://github.com/XpiritBV/AzureFunctionsWorkshop​
​
git clone https://github.com/XpiritBV/AzureFunctionsWorkshop.git ​

or use CodeSpaces  

---

## Transitioning to server-less

### There are no servers…​ When you do not need them

___

## Functions as a Service (FaaS)​

### Small pieces of self-contained server-side logic​

- **Event-driven**​  Responds to external triggers​
- **Instant scaling** ​ Abstraction of server infrastructure​, Scales when needed
- **Pay by consumption**​ Charged by GB-s and # of executions​

---
![bg right 100%](assets/functionapp.png)

## Anatonomy of an Azure Function App

- Hosted as Azure App Service​
- JSON based configuration​
- Running (multiple) functions​

Trigger starts execution​

Bindings for ​input and output​


___
## Programming model: It’s a function​

![width:75% height:500](assets/functionmodel.png)

---