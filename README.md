# AspNetCore6.Microservices.BookCatalog

This is a microservice-based application that follows the CQRS architectural pattern in the BookCommand and BookQuery microservices. The application separates the models for reading and writing data. It uses a view database, which is a read-only replica that is designed to support queries. The application keeps the replica up to date by subscribing to domain events published by the service that owns the data.

## Architecture

The application is divided into two microservices, BookCommand and BookQuery. The BookCommand microservice handles all the write operations, while the BookQuery microservice handles all the read operations.

The BookCommand microservice exposes REST API endpoints that allow clients to interact with the book catalog. This microservice uses a dedicated database for writing data, which we refer to as the write database. It also publishes domain events whenever any write operation is performed. These events are subscribed by the BookQuery microservice, which uses them to update the view database.

The BookQuery microservice is responsible for handling read operations such as retrieving books. It exposes REST API endpoints that allow clients to query the book catalog. This microservice uses a dedicated database for reading data, which we refer to as the read database. It also subscribes to domain events published by the BookCommand microservice. When a book is created, updated, or deleted, the BookCommand microservice publishes a domain event, which the BookQuery microservice consumes to keep the read database up to date.

The application uses the CQRS pattern to separate the models for reading and writing data. The write model is designed for high throughput and consistency, while the read model is designed for fast and scalable queries.

## Technologies

The application is built using AspNetCore6, which is a modern and lightweight web framework for building microservices. It uses a SQL database for storing write data, and another SQL database for storing read data. It also uses a message queue (Rabbit MQ) for handling domain events.

### Class Diagram

The BookCommand and BookQuery microservices have their own controllers and data access layers, but they both use the same domain models.

Here is a class diagram that shows the main components of the application:

```lua
+-----------------------+            +----------------------+             +--------------------+
|                       |            |                      |             |                    |
|      BookCommand      + ---------> +     Message Queue    + ----------> +     Book Query     |
|                       |  Publish   |                      |  Subscribe  |                    |
+-----------------------+            +----------------------+             +--------------------+
        |                                        |                                   ^
        | Write                                  |                                   | Read
        v                                        |                                   |
+-----------------------+                        |                        +---------------------+
|                       |                        |                        |                     |
|     Write Database    |                        +----------------------> +    View Database    |
|                       |                             Publish/Update      |                     |
+-----------------------+                                                 +---------------------+
```

## Getting Started
To get started with the application, follow these steps:

- Clone the repository.

- Open the solution file in Visual Studio.

- Build the solution.

- Run the BookCommand microservice.

- Run the BookQuery microservice.

- Use the API endpoints to perform read and write operations.

## API Endpoints
Here are the API endpoints that can be used to perform read and write operations:

### BookCommand Microservice
- POST /api/books - Add a new book.
- PUT /api/books/{id} - Update an existing book.
- DELETE /api/books/{id} - Delete an existing book.

### BookQuery Microservice
- GET /api/books - Get a list of all books.
- GET /api/books/{id} - Get a single book.
