const SALT = "12101992";
var CoreJS = {
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
    encodeNumber: function (number) {
        const saltedValue = `${SALT}${number}`; // Salt ile sayıyı birleştir
        const base36 = saltedValue.split('').reduce((acc, char) => acc + char.charCodeAt(0), 0).toString(36).toUpperCase();
        return base36.padStart(12, '0'); // 12 haneye uzat
    },
    decodeNumber: function (encodedNumber) {
        const base10 = parseInt(encoded, 36); // Base36'dan sayı çıkar
        const saltedNumber = base10 - SALT.split('').reduce((acc, char) => acc + char.charCodeAt(0), 0); // Salt etkisini kaldır
        return saltedNumber;
    }
}