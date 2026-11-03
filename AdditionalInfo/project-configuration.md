# Gamestore CI/CD Template

## Description

A template for setting up a CI pipeline for the `Gamestore` project.

## Requirements

- Update Visual Studio to latest version (2022+).
- All test should be consolidated in one test project.
- Test project should reference `coverlet.msbuild` package.
- Solution must be located in `Gamestore` folder.

## Installation

1. Copy the [.gitlab-ci.yml](.gitlab-ci.yml) file to the repository root folder.
2. Copy the following files to the solution folder:
   - [Directory.Build.props](Directory.Build.props)
   - [.editorconfig](.editorconfig)

## Usage

- **IDE Integration**: Errors and suggestions will be highlighted based on the rule set from `.editorconfig`, which is automatically loaded.
- **CI/CD Pipeline**: The pipeline runs on every push.
- Results can be viewed in the Merge Request details page or in the `CI/CD` → `Pipelines` section of your project at [autocode.git.epam.com](https://autocode.git.epam.com/).
- The report includes total test coverage.
