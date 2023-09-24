REGION_NAME=eastus
POST_FIX=12345
RESOURCE_GROUP=SP-PoC_Zone
FUNCTION_NAME=oalogsfunc-$POST_FIX
STORAGE_NAME=oalasf$POST_FIX
GIT_REPO=https://github.com/oaviles/hello_loganalytics.git

az login --service-principal -u $1 -p $2 --tenant $3

az group create --name $RESOURCE_GROUP --location $REGION_NAME

az storage account create --name $STORAGE_NAME --resource-group $RESOURCE_GROUP --location $REGION_NAME --sku Standard_LRS

az functionapp create --resource-group $RESOURCE_GROUP --name $FUNCTION_NAME --os-type Windows --runtime dotnet --runtime-version 6 --functions-version 4 --storage-account $STORAGE_NAME --consumption-plan-location $REGION_NAME --deployment-source-url $GIT_REPO
