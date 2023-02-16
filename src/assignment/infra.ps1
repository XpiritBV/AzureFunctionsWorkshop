$location="westeurope"
$rgname="game-highscore-rg"

az group create --name $rgname --location $location --tags type=labs

$stname="gamehighscorest"

az storage account create --name $stname --resource-group $rgname --location $location --sku Standard_LRS --kind StorageV2 --access-tier Hot

az extension add --name application-insights

$ainame="game-highscore-ai"

az monitor app-insights component create --app $ainame --location $location --application-type web --kind web --resource-group $rgname

$funcAppName="game-highscore-fa"

az functionapp create --name $funcAppName --resource-group $rgname --consumption-plan-location $location --storage-account $stname --app-insights $ainame --runtime dotnet --functions-version 4 --os-type Windows

# to remove, run the clean.ps1 file
