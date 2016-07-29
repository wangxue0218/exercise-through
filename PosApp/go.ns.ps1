param(
    $taskList
)

import-module ".\build\psake-master\psake.psm1"

if ($taskList -eq "help") {
    invoke-psake -docs
} else {
    invoke-psake -taskList $taskList
}