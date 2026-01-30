# Qscan Client

一個基於 WPF (.NET 8) 開發的現代化文件掃描與管理客戶端應用程式。

## 🚀 專案概述

Qscan Client 旨在提供流暢且美觀的桌面體驗，用於接收、管理及查看掃描的文件批次。本專案採用 **Fluent 2 / WinUI 3** 的視覺風格，結合了現代化的深色模式與毛玻璃效果。

## 🛠️ 技術棧

- **框架**: .NET 8.0 WPF
- **目標系統**: Windows 10 / 11 (SDK 10.0.19041.0)
- **設計語言**: Fluent Design 2 (Mica / Acrylic 效果)
- **架構模式**: MVVM (預計導入)

## ✨ 核心特性

- **現代化 UI**: 自定義標題列 (`WindowChrome`)，深色主題，優化的側邊導航欄。
- **文件卡片設計**: 提供 `DocCard` 組件，直觀顯示文件縮圖、名稱、日期與大小。
- **高效導航**: 側邊欄支持首頁 (Home)、掃描器 (Scanner)、歷史紀錄 (History) 與設定 (Settings) 的快速切換。

## 📂 專案結構

- `src/QscanClient`: 核心專案原始碼。
- `App.xaml`: 應用程式資源與啟動設定。
- `MainWindow.xaml`: 主視窗佈局，包含自定義導航與工作區。
- `DocCard.xaml`: 獨立的文件展示組件。
- `wpf_uiux_spec（ai_friendly）.md`: 詳細的 UI/UX 設計規範文件。

## 📅 開發藍圖

1. **架構重構**: 建立 `ViewModels` 並實作 MVVM 模式。
2. **導覽功能**: 實作動態頁面切換機制。
3. **數據整合**: 建立掃描數據服務，模擬或串接真實掃描硬體。
4. **細節優化**: 增加微動畫與物理回饋效果。

## 🛠️ 如何運行

1. 確保已安裝 [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)。
2. 使用 Visual Studio 2022 或 VS Code 開啟專案。
3. 執行指令：
   ```powershell
   dotnet run --project src/QscanClient/QscanClient.csproj
   ```

---

*由 Antigravity AI 輔助開發*
