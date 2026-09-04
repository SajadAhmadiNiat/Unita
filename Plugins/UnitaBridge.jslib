mergeInto(LibraryManager.library, {
    
    EitaaReady: function () {
        if (window.Eitaa && window.Eitaa.WebApp) {
            window.Eitaa.WebApp.ready();
        }
    },

    EitaaExpand: function () {
        if (window.EitaaWebApp) {
            window.EitaaWebApp.expand();
        }
    },

    EitaaClose: function () {
        if (window.EitaaWebApp) {
            window.EitaaWebApp.close();
        }
    },

    EitaaGetInitData: function (objectNamePtr, callbackNamePtr) {
        var objName = UTF8ToString(objectNamePtr);
        var cbName = UTF8ToString(callbackNamePtr);
        
        if (window.EitaaWebApp && window.EitaaWebApp.initData) {
            SendMessage(objName, cbName, window.EitaaWebApp.initData);
        } else {
            SendMessage(objName, cbName, "");
        }
    },

    EitaaGetVersion: function (objectNamePtr, callbackNamePtr) {
        var objName = UTF8ToString(objectNamePtr);
        var cbName = UTF8ToString(callbackNamePtr);
        
        if (window.EitaaWebApp && window.EitaaWebApp.version) {
            SendMessage(objName, cbName, window.EitaaWebApp.version);
        } else {
            SendMessage(objName, cbName, "unknown");
        }
    },

    EitaaSetHeaderColor: function (colorPtr) {
        var color = UTF8ToString(colorPtr);
        if (window.EitaaWebApp && window.EitaaWebApp.setHeaderColor) {
            window.EitaaWebApp.setHeaderColor(color);
        }
    },

    EitaaSetBackgroundColor: function (colorPtr) {
        var color = UTF8ToString(colorPtr);
        if (window.EitaaWebApp && window.EitaaWebApp.setBackgroundColor) {
            window.EitaaWebApp.setBackgroundColor(color);
        }
    },

    EitaaShowAlert: function (messagePtr) {
        var message = UTF8ToString(messagePtr);
        if (window.EitaaWebApp && window.EitaaWebApp.showAlert) {
            window.EitaaWebApp.showAlert(message);
        } else {
            alert(message);
        }
    },

    EitaaSetMainButton: function (textPtr, colorPtr, textColorPtr, isVisible, isActive) {
        var text = UTF8ToString(textPtr);
        var color = UTF8ToString(colorPtr);
        var textColor = UTF8ToString(textColorPtr);
        
        if (window.EitaaWebApp && window.EitaaWebApp.MainButton) {
            var mainBtn = window.EitaaWebApp.MainButton;
            mainBtn.setText(text);
            mainBtn.setParams({
                color: color,
                text_color: textColor
            });
            
            if (isVisible) mainBtn.show(); else mainBtn.hide();
            if (isActive) mainBtn.enable(); else mainBtn.disable();
        }
    },

    EitaaHapticImpact: function (stylePtr) {
        var style = UTF8ToString(stylePtr);
        if (window.EitaaWebApp && window.EitaaWebApp.HapticFeedback) {
            window.EitaaWebApp.HapticFeedback.impactOccurred(style);
        }
    }
});