# Weekly Milestone Manager
This GitHub project provides an automated tool, written in C#, for creating new milestones every week and managing unfinished issues within them. The project is designed to help keep your GitHub repositories organized and efficient.

## Features
- Automatically creates new milestones every week with a name format like 2023w33, where 2023 is the current year, and 33 is the current week.
- Moves unfinished issues from old milestones to upcoming ones.
- Closes old milestones once all issues are resolved.
- For best results, run the application every week on the last day at 23:55.

## Setup
To set up this project, follow these steps:

- Clone the repository to your local machine.
- Open the solution file (.sln) in Visual Studio.
- Build the solution to generate the executable.
- Replace GitLab Url and Secret Key.
- Set up the program to run as a scheduled job in Windows Task Scheduler.

## Windows Task Scheduler
To set up the program as a scheduled job in Windows Task Scheduler, follow these steps:

- Open the Task Scheduler on your Windows machine.
- Create a new task with the desired schedule (e.g., weekly on the last day at 23:55).
- Set the action to start the program with the path to the executable and necessary arguments.
Save the task and ensure it runs as expected.

## Usage
To manually run the application, execute the built executable generated by Visual Studio:

Copy code
```csharp
GMS.exe
```
This will create a new milestone, move unfinished issues, and close old milestones as configured.

Contributing
Contributions are welcome! Feel free to submit issues and pull requests to help improve this project.

License
This project is licensed under the MIT License.