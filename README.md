# Downloader

Downloader is a project that allows you to manage and monitor download tasks. It is built with .NET 7 and Angular.

## Project Structure

The project is divided into two main parts:

1. The server-side application, which is written in .NET 7 and is located in the [`Downloader/`](Downloader/) directory.
2. The client-side application, which is written in Angular and is located in the [`Downloader/ClientApp/`](Downloader/ClientApp/) directory.

The server-side application manages download tasks using the [`DownloadQueueManager`](Downloader/Workers/DownloadQueueManager.cs) class. It also provides a REST API for the client-side application to interact with.

The client-side application provides a user interface for managing download tasks. It communicates with the server-side application through the REST API.

## How to Run the Project

### Server-Side Application

To run the server-side application, navigate to the [`Downloader/`](Downloader/) directory and run the following command:

```sh
dotnet run
```

### Client-Side Application

To run the client-side application, navigate to the [`Downloader/ClientApp/`](Downloader/ClientApp/) directory and run the following commands:

```sh
npm install
ng serve
```

Then, open your web browser and navigate to `https://localhost:7289/`.

## How to Build the Project

### Server-Side Application

To build the server-side application, navigate to the [`Downloader/`](Downloader/) directory and run the following command:

```sh
dotnet build
```

### Client-Side Application

To build the client-side application, navigate to the [`Downloader/ClientApp/`](Downloader/ClientApp/) directory and run the following command:

```sh
ng build
```

The build artifacts will be stored in the `dist/` directory.

## How to Test the Project

### Server-Side Application

To test the server-side application, navigate to the [`Downloader/`](Downloader/) directory and run the following command:

```sh
dotnet test
```

### Client-Side Application

To test the client-side application, navigate to the [`Downloader/ClientApp/`](Downloader/ClientApp/) directory and run the following command:

```sh
ng test
```

The unit tests will be executed via [Karma](https://karma-runner.github.io).

## Further Help

For more help on the Angular CLI, use `ng help` or check out the [Angular CLI](https://angular.io/cli).