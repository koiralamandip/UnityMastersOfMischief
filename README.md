# Unity Project Repository

Welcome to the repository for our Unity project! This document provides guidelines and instructions for contributing to and managing the project.

## Branching Model

We follow a Git branching model that allows for separate development and production environments. Here's an overview of the branches:

- **Main Branch**: The main branch is reserved for production-ready code. Any changes merged into this branch trigger a workflow to build the Windows64 executable.
  
- **Dev Branch**: The dev branch serves as the integration branch for ongoing development work. Developers should create feature branches from this branch.

## Workflow

1. **Clone the Repository**: Start by cloning the repository to your local machine.
   ```
   git clone <repository-url>
   ```
   
2. **Create a Feature Branch**: Create a new branch for your feature or bug fix based on the `dev` branch.
   ```
   git checkout -b feature/your-feature dev
   ```
   
3. **Work on Your Feature**: Make your changes on the feature branch locally.

4. **Push Your Branch**: Once your feature is ready, push your branch to the remote repository.
   ```
   git push origin feature/your-feature
   ```
   
5. **Create a Pull Request**: Go to the GitHub repository page and create a pull request to merge your feature branch into the `dev` branch. Ensure you provide a clear description of the changes made.
   
6. **Code Review**: Other team members will review your pull request. Address any feedback or issues raised during the review process.

7. **Merge to Dev**: Once your pull request is approved, it will be merged into the `dev` branch.

8. **Release to Main**: Periodically, the `dev` branch will be merged into the `main` branch to prepare for production releases.

## Building the Project

- **Main Branch Build**: Building from the `main` branch triggers a workflow to build the Windows64 executable automatically.

- **Development Builds**: For local testing and development purposes, you can build the project directly from your Unity editor.

## License

This project is a work of a group for submission in a graded project. Not for reproduction.
