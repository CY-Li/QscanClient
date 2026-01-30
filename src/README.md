# Qscan Client

## Project Status
This project is initialized as a **Windows WPF** application targeting **.NET 8**.

## Fluent 2 / WinUI 3 Support
The original request was to include **Fluent 2 Design (via Wpf.Ui)**. 
However, during initialization, the local environment consistently failed to resolve the .NET Core/Standard versions of the `Wpf.Ui` NuGet package, falling back to incompatible .NET Framework versions (Warning `NU1701`).

To ensure the project compiles and runs, it has been reverted to **Standard WPF**.

### How to Enable Fluent 2 Later
Once the local NuGet/SDK environment is fixed (ensuring `net8.0-windows` packages are resolved correctly without fallback):

1. Add the package:
   ```bash
   dotnet add package Wpf.Ui
   ```
2. Update `App.xaml` to merge `Wpf.Ui` dictionaries.
3. Update `MainWindow.xaml` to use `ui:FluentWindow`.

## Build
```bash
cd src/QscanClient
dotnet build
dotnet run
```
