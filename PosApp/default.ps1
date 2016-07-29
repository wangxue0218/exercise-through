framework "4.6"

function do-migration ($databaseName) {
    $connectionString = "Data Source=.\SQLExpress;Initial Catalog=$databaseName;Integrated Security=True"
    $migrationTarget = ".\src\Pos.Migration\bin\Debug\Pos.Migration.dll"
    iex ".\packages\FluentMigrator.1.6.2\tools\Migrate.exe /connection '$connectionString' /db SqlServer2014 /target '$migrationTarget'"
}

function create-database ($databaseName) {
    iex "sqlcmd -S '.\SQLExpress' -Q 'create database [$databaseName]'"
}

task build {
    exec { msbuild ".\posApp.sln" }
    do-migration 'pos'
}

task migration {
    do-migration 'pos'
}

task migration-ut {
    do-migration 'pos-ut'
}

task create-database {
    create-database "pos"
}

task create-database-ut {
    create-database "pos-ut"
    do-migration 'pos-ut'
}