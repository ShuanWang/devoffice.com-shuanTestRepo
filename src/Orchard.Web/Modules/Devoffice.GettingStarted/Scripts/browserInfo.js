//browserInfo.js
//
//Utility functions to get info about the browser and OS

function os () {
    var ua = navigator.userAgent.toLowerCase();
    return {
        isWindows: /windows/.test(ua),
        isWin2K: /windows nt 5.0/.test(ua),
        isXP: /windows nt 5.1/.test(ua),
        isVista: /windows nt 6.0/.test(ua),
        isWin7: /windows nt 6.1/.test(ua),
        isWin8: /windows nt 6.2/.test(ua),
        isWin81: /windows nt 6.3/.test(ua),
        isMac: /MacOS/.test(ua),
        isUNIX: /UNIX/.test(ua),
        isLinux: /Linux/.test(ua)
    };
}
