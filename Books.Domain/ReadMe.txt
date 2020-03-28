It can be noted that this project contains repositories.
At first this does not seem valid as this project is not intended for data access, however when considering that the repositories
should be based on aggregate root domain models, achieving that without creating the repositories in this project would not be possible.
That is because the data access project would have to reference this project to create the repositories, and this project would then 
have to use the data access project in order to provide its functionality. That would create a circular project reference.

With the design as it is the domain project requires knowledge of how to map to and from domain models and database models, 
as well as how to interact with the database store. This means that if the database provider changes the model will have to change.