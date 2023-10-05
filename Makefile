
test.integration.run:
	export $(cat .env.test | xargs)
	dotnet test --filter "OrderingApi.IntegrationTest"

test.unit:
	dotnet test --filter "OrderingApi.UnitTest"

migrations.testing.run:
	export $(cat .env.test | xargs)
	dotnet ef database update

migrations.testing.show:
	export $(cat .env.test | xargs)
	dotnet ef migrations list 

run:
	export $(cat .env | xargs)
	dotnet run
	
