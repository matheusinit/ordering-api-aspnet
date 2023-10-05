
integration-test: 
	dotnet test --filter "OrderingApi.IntegrationTest"

unit-test:
	dotnet test --filter "OrderingApi.UnitTest"

migrations.testing.run:
	BASH_ENV=./.env.test dotnet ef database update

migrations.testing.show:
	BASH_ENV=./.env.test dotnet ef migrations list 

run:
	dotnet run
	
