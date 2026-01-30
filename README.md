# Qscan Client

一個基於 WPF (.NET 8) 開發的現代化文件掃描與管理客戶端應用程式，採用現代化 Fluent Design 2.0 視覺規範。

## 🚀 專案現況與成果

目前專案已完成核心介面與導覽架構的實作，具備高品質的視覺質感與流暢的使用者體驗。

### ✨ 核心特性

- **高品質雙主題系統**: 支援即時切換的雙主題設計，適配不同使用場景。
  - **日落熔岩 (Sunset Lava - Dark)**: 深邃的沉浸式深色模式，搭配暖橙漸層與琥珀色發光效果。
  - **日出珍珠 (Sunrise Pearl - Light)**: 溫潤優雅的淺色模式，經過高對比優化，確保文字清晰易讀。
- **現代化導覽架構**: 
  - 基於 **View-based Navigation** 的內容動態切換，提供首頁 (Home) 與設定 (Settings) 視圖。
  - 精緻的側邊導航欄，具備流暢的選中動畫與懸停回饋。
- **精品化組件設計**: 
  - **DocCard**: 擬真化文件預覽卡片，支援動態縮放與高級陰影效果。
  - **Custom TitleBar**: 自定義視窗標題列，支援主題連動與自定義控制項。
- **全面動態綁定**: 介面所有元素（含外框、按鈕、圖標）均採用 `DynamicResource`，支援零延遲的主題切換。

## 🛠️ 技術棧

- **核心框架**: .NET 8.0 WPF
- **架構模式**: MVVM (採用 **CommunityToolkit.Mvvm**)
- **圖標系統**: Fluent System Icons (向量路徑實作)
- **設計語言**: Fluent Design 2.0 (微動畫、高級陰影與層次感)

## 📂 專案結構

- `src/QscanClient/Themes`: 存放主題資源字典 (`DarkTheme.xaml`, `LightTheme.xaml`)。
- `src/QscanClient/ViewModels`: 核心業務邏輯與介面狀態控制。
- `src/QscanClient/Views`: 獨立的視圖處理，包含 `HomeView` 與 `SettingsView`。
- `src/QscanClient/DocCard.xaml`: 高度封裝的文件展示組件。

## 📅 開發藍圖 (Next Steps)

1. **掃描器連接實作**:
   - 整合 **mDNS (DNS-SD)** 服務廣播，讓掃描器自動發現此客戶端。
   - 建立 **WebSocket 伺服器** 以接收來自 Q30 掃描器的影像串流。
2. **流程管理 (Workflow)**:
   - 實作 Workflow 視圖，讓使用者能設定文件自動處理規則。

## 🛠️ 如何編譯與運行

1. 確保已安裝 [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)。
2. 在專案根目錄執行：
   ```powershell
   dotnet run --project src/QscanClient/QscanClient.csproj
   ```

---

*由 Antigravity AI 輔助開發，致力於打造極致的 Windows 桌面體驗。*
