# WPF UI/UX Specification（AI-friendly）

> 本文件目的是 **同時給人類與 AI（設計 / 產生 UI / 產生 ViewModel / 產生 XAML）都能清楚理解** 的 UIUX 規格。
>
> 適用範圍：Windows WPF (.NET, MVVM)

---

## 1. 文件基本資訊 (Metadata)

- Product Name: **<ProductName>**
- Platform: **Windows Desktop (WinUI 3)**
- Architecture Pattern: **MVVM (Model / View / ViewModel)** (WinUI 3)
- Target OS: **Windows 10 / 11**
- Primary Input: **Mouse / Keyboard / Touch (Optional)**
- Language: **zh-TW (default), en-US (optional)**

---

## 2. UX 設計原則 (Design Principles)

### 2.1 核心原則

- **Clarity over cleverness**：介面清楚優先於炫技
- **Minimum cognitive load**：單一畫面只做一件事
- **Predictable layout**：固定區域、不隨意跳動
- **Immediate feedback**：所有操作都必須有回饋

### 2.2 AI 可理解規則（重要）

- 每個畫面必須定義：
  - Screen ID
  - UI 組件清單
  - 使用者行為 → 狀態改變
- 所有 UI 行為 **對應 ViewModel Command**
- 所有顯示內容 **對應 ViewModel Property**

---

## 3. 整體資訊架構 (Information Architecture)

### 3.1 App Shell Layout

```
+--------------------------------------------------+
| TopBar (AppTitle / Window Controls)              |
+----------------------+---------------------------+
| Sidebar (Navigation) | Main Content Area         |
|                      |                           |
| - Home               | 依 Navigation 切換頁面     |
| - Workflow           |                           |
| - Settings           |                           |
+----------------------+---------------------------+
| StatusBar (Optional)                             |
+--------------------------------------------------+
```

### 3.2 Navigation Model

- Navigation Type: **Left Sidebar Navigation**
- Navigation State:
  - SelectedItem (one)
  - DisabledItem (optional)

---

## 4. 視覺設計系統 (Design System)

### 4.1 Design Language

- Design System: **Fluent 2**
- Visual Style: **Modern Glass / Mica / Acrylic**
- Motion: **Subtle, physics-based**
- Depth: **Layered elevation (Z-axis)**

### 4.2 Surface & Material Tokens

```yaml
Material.Window: Mica
Material.Panel: Acrylic
Material.Card: AcrylicThin
Material.Dialog: Acrylic
```

### 4.3 色彩 (Fluent Tokens)

```yaml
Color.Accent: SystemAccent
Color.Text.Primary: #FFFFFF
Color.Text.Secondary: #C7C7C7
Color.Surface.Base: #1E1E1E
Color.Surface.Card: #252525
Color.Border.Subtle: rgba(255,255,255,0.08)
```

### 4.4 圓角與層級

```yaml
CornerRadius.Small: 4
CornerRadius.Medium: 8
CornerRadius.Large: 12

Elevation.Card: 2
Elevation.Flyout: 4
Elevation.Dialog: 8
```

------|------|--------|-------|
| H1 | 24 | SemiBold | Page Title |
| H2 | 18 | SemiBold | Section Title |
| Body | 14 | Regular | Normal text |
| Caption | 12 | Regular | Hint / Meta |

---

## 5. 通用 UI 元件規格 (WinUI 3 / Fluent 2 Component Spec)

### 5.1 Button

- Variants: Primary / Secondary / Text / Danger
- States: Normal / Hover / Pressed / Disabled / Loading

```yaml
Component: Button
Properties:
  Text: string
  Icon: optional
  IsEnabled: bool
  IsLoading: bool
Actions:
  OnClick -> ICommand
```

---

### 5.2 Text Input

```yaml
Component: TextBox
Properties:
  Text: string (TwoWay Binding)
  Placeholder: string
  IsReadOnly: bool
Validation:
  ErrorMessage: string
```

---

### 5.3 List / Data Grid

```yaml
Component: ListView / DataGrid
Properties:
  ItemsSource: ObservableCollection<T>
  SelectedItem: T
Actions:
  OnItemClick -> ICommand
```

---

## 6. 畫面級規格 (Screen Specification)

> ⚠️ **每一個畫面都必須獨立定義，AI 不能靠猜**

---

### Screen: Home / Recent

```yaml
ScreenID: HOME_001
Purpose: 即時顯示從掃描器接收到的影像批次（Recent Scans）
RefreshMode: Realtime (Event-driven)
```

#### Data Model (AI-readable)

```yaml
ScanBatch:
  BatchId: string
  Name: string
  ReceivedAt: datetime
  Thumbnails: List<Image>
```

#### Layout Structure

```
[CommandBar]
  - View Toggle (Grid / List)
  - Sort (Time desc / asc)
------------------------------------------------
[ScrollViewer]
  [ItemsRepeater]
    ┌──────────────────────────────────────┐
    │ [Thumbnail]  [Batch Name]            │
    │              [Received Time]         │
    └──────────────────────────────────────┘
```
[PageTitle: Recent Scans]
------------------------------------------------
[ItemsRepeater / GridView]
  ┌──────────────────────────┐
  │ Thumbnail (Preview)      │
  │ Batch Name               │
  │ Received Time            │
  └──────────────────────────┘
```

#### UI Elements

| ID | Type | Binding | Description |
|----|------|--------|-------------|
| PageTitle | TextBlock | "Recent Scans" | 固定標題 |
| BatchGrid | GridView | ScanBatches | 批次清單 |
| Thumb | Image | Thumbnail | 批次縮圖 |
| Name | TextBlock | Name | 批次名稱 |
| Time | TextBlock | ReceivedAt | 接收時間 |

#### Interaction Rules

```yaml
- On Scanner Image Received
  -> Append ScanBatch to ScanBatches
  -> UI auto refresh (ObservableCollection)

- On Batch Item Click
  -> Execute CmdOpenBatch
  -> Navigate to BatchDetail Screen
```

#### Visual Rules (Fluent 2)

```yaml
Card:
  Material: AcrylicThin
  CornerRadius: 12
  Elevation: 2

Thumbnail:
  AspectRatio: 1:1
  Clip: Rounded
```

------------------------------------------------
[Card] Recent Activity
[Card] Quick Actions
```

#### UI Elements

| ID | Type | Binding | Description |
|----|------|--------|-------------|
| Title | TextBlock | PageTitle | 畫面標題 |
| RecentList | ListView | RecentItems | 最近項目 |
| BtnQuick | Button | CmdQuickAction | 快速操作 |

#### User Interactions

```yaml
- User clicks BtnQuick
  -> Execute CmdQuickAction
  -> Navigate to TARGET_SCREEN
```

---

### Screen: Settings

```yaml
ScreenID: SETTINGS_001
Purpose: 系統設定與偏好設定
```

#### UI Elements

| ID | Type | Binding |
|----|------|---------|
| Language | ComboBox | SelectedLanguage |
| Save | Button | CmdSaveSettings |

---

## 7. 狀態管理 (State Management)

### 7.1 ViewModel 狀態

```yaml
State:
  IsLoading: bool
  HasError: bool
  ErrorMessage: string
```

### 7.2 Loading 規範

- 任一非同步操作 > 300ms 必須顯示 Loading
- Loading 狀態必須 disable 相關操作

---

## 8. 錯誤與回饋 (Error & Feedback)

### 8.1 錯誤顯示層級

1. Inline (欄位錯誤)
2. Page-level Message
3. Modal Dialog (Critical)

```yaml
Error:
  Code: string
  Message: string
  RecoveryAction: optional ICommand
```

---

## 9. 可存取性 (Accessibility)

- Keyboard Navigation: 必須支援
- Focus Visual: 明確
- Color Contrast: WCAG AA

---

## 10. AI / 開發約定 (非常重要)

### 10.1 命名規則

- View: `XxxView.xaml`
- ViewModel: `XxxViewModel.cs`
- Command: `CmdXxx`
- Property: PascalCase

### 10.2 AI 產生 UI 時的硬性規則

- 不可直接操作 Model
- 不可在 View 寫邏輯
- 所有行為必須 ICommand

---

## 11. 擴充附錄（選填）

- Wireframe
- Flow Diagram
- API Contract

---

**End of Spec**

