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

## � 介面與功能詳細規格 (UI/UX Specifications)

本專案採用 View-based Navigation 架構，以下為各視圖的詳細規格分析：

### 1. 應用程式框架 (App Shell) - `MainWindow`
作為整個應用程式的容器，負責全域導航與狀態顯示。
- **標題列 (TitleBar)**:
  - 整合式視窗控制 (WindowChrome)，支援拖曳移動。
  - **Debug 工具**: 包含 "Simulate" 按鈕，可快速切換模擬掃描狀態。
- **側邊導覽列 (Sidebar)**:
  - 提供 `Home` (首頁)、`Workflow` (工作流 - 預留)、`Settings` (設定) 的導航切換。
  - 支援單選按鈕樣式 (RadioButton) 的視覺狀態管理。
- **底部狀態列 (StatusBar)**:
  - **連接狀態監控**: 顯示 Q30 掃描器的連線狀態 (Searching / Connected / Disconnected)。
  - **視覺回饋**: 透過不同顏色的呼吸燈號 (紅/綠/橙) 與圖標變化，提供即時硬體狀態指示。

### 2. 首頁 (Home View)
監控中心與歷史紀錄的儀表板。
- **即時掃描面板 (Live Session Panel)**:
  - **觸發機制**: 僅在 `IsScanning = True` 時自左側滑出。
  - **動態回饋**: 包含 "Scanning from Q30" 錄製指示燈 (紅點呼吸動畫)。
  - **進度顯示**: 中央顯示目前掃描頁數計數器，搭配 Indeterminate 旋轉動畫環。
- **最近掃描列表 (Recent Scans)**:
  - 使用 `DocCard` 組件呈現歷史批次。
  - 支援響應式排列 (WrapPanel)，顯示批次名稱 (Timestamp)、文件數量與日期。
  - **互動**: 點擊卡片觸發 `SelectBatchCommand` 並導航至詳情頁。

### 3. 批次詳情頁 (Detail View)
特定掃描批次的檢視與管理介面，採用側欄縮圖與主預覽區的布局。
- **互動式影像檢視器 (Interactive Viewer)**:
  - 核心邏輯基於 `ZoomBorder` 類別，支援流暢的 **縮放 (Zooming)** 與 **平移 (Panning)**。
  - **浮動控制列 (Floating Control Bar)**: 輕量化、半透明的圓角面板，提供常用功能：
    - **放大鏡圖示**: 直覺的放大 (+) 與縮小 (-) 操作。
    - **符合視窗 (Fit to Screen)**: 一鍵重置影像大小與位置。
    - **高級介面回饋**: 按鈕具備感性的懸停亮化與點擊縮放微動畫。
- **側欄縮圖清單 (Sidebar Gallery)**: 
  - 列出批次內所有影像，支援點選切換。
  - **智慧刪除機制**: 滑鼠移入縮圖時才顯現的「垃圾桶」按鈕，減少視覺雜訊。
- **自動化體驗 (Automation)**:
  - **首頁即看**: 進入詳情頁時自動選擇並顯示第一張影像。
  - **連續檢視**: 刪除目前檢視的影像後，系統會自動切換至下一張（或上一張），確保檢視流程不中斷。
- **快速操作**:
  - **鍵盤導覽**: 支援 **上下鍵 (Up/Down)** 快速切換預覽影像。
  - **整批刪除**: 頂部工具列提供標誌性的垃圾桶按鈕，可一鍵移除整個批次。
- **硬體最佳化**: 影像渲染採用 `BitmapScalingMode="Fant"`，確保縮放時依然保持細膩清晰。

### 4. 收件匣 (Inbox View)
分別檢視來自對應不同Workflow的文件，預設會有「Quick Scan」這個分類，其餘則是由使用者從Workflow頁面定義。

### 5. 工作流 (Workflow View)
定義工作流的掃描參數、IP參數、輸出路徑、OCR設定等。

### 6. 設定頁 (Settings View)
- **外觀設定 (Appearance)**:
  - **主題切換**: 提供 Light / Dark 雙色系切換開關，綁定 `IsDarkTheme` 屬性，即時刷新全域資源字典。

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
