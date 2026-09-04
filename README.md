<p align="center">
  <img src="Documents/1.png" alt="Unita Banner" width="100%">
</p>
# Unita
Solution and bridge between Eitaa MiniApps and Unity programs

[![Unity Version](https://img.shields.io/badge/Unity-2021.3+-blue.svg)](https://unity.com)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE.md)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](CONTRIBUTING.md)

پروژه **Unita** یک پلاگین اختصاصی برای تبدیل بازی‌ها و نرم‌افزارهای توسعه‌یافته در موتور بازی Unity به برنامک‌های تعاملی تحت وب (MiniApps) در بستر پیام‌رسان ایتا است.

## 📦 نصب

### از طریق Package Manager (Git URL)
1. در Unity، پنجره **Window > Package Manager** را باز کنید.
2. روی **+** کلیک کرده و **Add package from git URL** را انتخاب کنید.
3. آدرس زیر را وارد کنید:
   ```
   https://github.com/SajadAhmadiNiat/Unita.git
   ```

### نصب دستی
1. مخزن را Clone یا Download کنید.
2. پوشه `Unita` را به پوشه `Assets` پروژه خود کپی کنید.
   
## 🚀 شروع سریع

1. در اولین Scene پروژه، یک GameObject خالی ایجاد کنید و نام آن را `UnitaManager` بگذارید.
2. اسکریپت `UnitaManager.cs` را روی آن Drag کنید.
3. در متد `Start`، برنامک را آماده کنید:

```csharp
private void Start()
{
    UnitaManager.Instance.Ready();
    UnitaManager.Instance.Expand();
}
```

## ⚙️ تنظیمات قالب (Template Configurator)

از منوی **Unita > Template Configurator** می‌توانید ظاهر صفحه بارگذاری را سفارشی کنید:

- **MiniApp Title** - عنوان برنامک
- **Loading Text** - متن بارگذاری
- **Font Name** - فونت دلخواه (یا انتخاب فایل `.woff2` از Asset)
- **Text Alignment** - تراز متن (چپ، راست، وسط)
- **Background Color / Image** - رنگ یا تصویر پس‌زمینه (انتخاب فایل `.png` از Asset)
- **Show Loading Text / Fullscreen Button** - نمایش یا مخفی کردن المان‌ها

## 📚 مستندات کامل API

| متد | توضیح |
|-----|--------|
| `Ready()` | اعلام آماده‌بودن برنامک به ایتا |
| `Expand()` | گسترش برنامک به حداکثر ارتفاع |
| `CloseWebApp()` | بستن برنامک |
| `RequestInitData()` | دریافت اطلاعات احراز هویت کاربر |
| `SetHeaderColor(string color)` | تغییر رنگ هدر |
| `SetBackgroundColor(string color)` | تغییر رنگ پس‌زمینه |
| `ShowAlert(string message)` | نمایش پیام هشدار |
| `ConfigureMainButton(...)` | تنظیم دکمه اصلی |
| `TriggerHaptic(string style)` | اجرای بازخورد لمسی |

## 🏗️ معماری پروژه

```
├── Assets/
│   ├── Plugins/
│   │   └── WebGL/
│   │       └── UnitaBridge.jslib
│   │
│   ├── Scripts/
│   │   └── UnitaManager.cs
│   │
│   └── WebGLTemplates/
│       └── EitaaTemplate/
│           └── index.html
│           └── unita-background.png
│           └── thumbnail.png
│           └── unita-loading-font.woff2
```

## 🔧 پیش‌نیازها
- Unity با پشتیبانی WebGL
- پروژه قابل Build در WebGL
- دامنه دارای گواهی SSL
- هاست مناسب برای فایل‌های WebGL
- حساب توسعه‌دهنده در پنل ایتایار

## 🤝 مشارکت

1. Fork کنید.
2. یک Branch جدید ایجاد کنید (`git checkout -b feature/amazing-feature`).
3. Commit کنید (`git commit -m 'Add amazing feature'`).
4. Push کنید (`git push origin feature/amazing-feature`).
5. Pull Request باز کنید.

## 📄 مجوز
این پروژه تحت مجوز **MIT** منتشر شده است. برای جزئیات بیشتر به فایل [LICENSE.md](LICENSE.md) مراجعه کنید.

## 👨‍💻 توسعه‌دهنده

**سجاد احمدی نیت** - صنایع خالق نسیم مهر، استودیو رز یخی
⭐ اگر این پروژه برای شما مفید بود، حتماً به آن ستاره دهید!
