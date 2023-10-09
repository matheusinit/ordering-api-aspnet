
test.integration.run:
	TESTING=true dotnet test --filter "OrderingApi.IntegrationTest"

test.unit.run:
	dotnet test --filter "OrderingApi.UnitTest"

migrations.testing.run:
	TESTING=true dotnet ef database update --context ApplicationContext

migrations.testing.show:
	TESTING=true dotnet ef migrations list --context ApplicationContext

run:
	dotnet run
