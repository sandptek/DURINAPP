const SALT = "12101992";
var Sotec = {
    GetData: function (op, successFunc, errorFunc) {
        var options = op;
        var jqxhr;

        if (successFunc) {
            options = $.extend(options, { success: successFunc });
        }
        if (errorFunc) {
            options = $.extend(options, { error: errorFunc });
        }

        options = $.extend({}, { type: "GET", cache: false, timeout: 90000 }, options);

        if (!options.url) {
            console.log('id, name, url : tanımsız');
        } else {
            jqxhr = $.ajax(options);
        }
        return jqxhr;
    },
    TableOP: function (e, t1, t2) {
        var a = $.extend({}, { type: "POST", cache: false, timeout: 90000 }, { url: "/System/TableOperations", data: { jsonData: JSON.stringify(e) } });
        t1 && (a = $.extend(a, { success: t1, error: t2 })), $.ajax(a)
    },
    toMixedNumber: function (inputNumber, salt = "314159265358") {
        if (inputNumber == undefined)
            return "";

        const inputStr = inputNumber.toString().padStart(12, "0"); // Sayıyı 12 karaktere tamamla
        let encrypted = "";

        for (let i = 0; i < inputStr.length; i++) {
            // Her bir karakteri SECRET_KEY'e göre değiştir
            const encryptedChar = (parseInt(inputStr[i]) + parseInt(salt[i])) % 10;
            encrypted += encryptedChar.toString();
        }

        return encrypted;

    },
    toOriginalNumber: function (mixedNumber, salt = "314159265358") {
        let original = "";

        for (let i = 0; i < encryptedNumber.length; i++) {
            // SECRET_KEY'e göre orijinal sayıyı geri al
            const originalChar = (parseInt(encryptedNumber[i]) - parseInt(salt[i]) + 10) % 10;
            original += originalChar.toString();
        }

        return parseInt(original); // Orijinal numarayı geri döndür
    }
}