# SEM5_PI_GRUPO44

## Starting the Backend API

To run the backend, open a terminal, navigate to the `Backend` directory, and execute the following file:

#### Linux/Mac Users

```sh
./run_backend.sh
```

#### Windows Users

Run script file ./run_backend.bat on repository root.

## Starting the Frontend

To start the backoffice, open a terminal, navigate to the `Frontend` directory, and execute the following file:

#### Linux/Mac Users

```sh
./run_frontend.sh
```

#### Windows Users

Run script file ./run_frontend.bat on repository root.

## Continuous Integration (CI) - GitHub Actions Workflows

#### Backend CI (.github/workflows/backend-ci.yml)

This workflow is triggered whenever there is a push or pull request in the `Backend` folder. It performs the following
steps:

- Check out the repository
- Set up the .NET SDK
- Restore dependencies
- Build the project
- Run tests

#### Frontend CI (.github/workflows/frontend-ci.yml)

This workflow is triggered whenever there is a push or pull request in the `Frontend` folder. It performs the following
steps:

- Check out the repository
- Set up Node.js
- Install dependencies
- Run tests
- Build the project
