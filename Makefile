
integration-test: 
	dotnet test --filter "OrderingApi.IntegrationTest"

unit-test:
	dotnet test --filter "OrderingApi.UnitTest"

run:
	dotnet run
	
