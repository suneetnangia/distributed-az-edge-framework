# ------------------------------------------------------------
#  Copyright (c) Microsoft Corporation.  All rights reserved.
#  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
# ------------------------------------------------------------

Param(
    [string]
    [Parameter(mandatory=$true)]
    $ApplicationName,
    [string]
    $Location = 'westeurope'
)

# to get access to function Write-Title
. .\functions.ps1

Write-Title("Start Deploying Core Platform")
$startTime = Get-Date

# ----- Dapr
Write-Title("Install Dapr")
helm repo add dapr https://dapr.github.io/helm-charts/
helm repo update
helm upgrade --install dapr dapr/dapr `
    --version=1.5 `
    --namespace edge-core `
    --create-namespace `
    --wait

# ----- Redis
Write-Title("Install Redis")
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update
helm install redis bitnami/redis `
    --namespace edge-core `
    --wait

$runningTime = New-TimeSpan -Start $startTime
Write-Host "Running time:" $runningTime.ToString() -ForegroundColor Yellow