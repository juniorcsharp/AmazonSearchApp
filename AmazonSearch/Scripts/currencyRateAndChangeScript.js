$(function () {
    var urlForCurrencies = "https://openexchangerates.org/api/currencies.json";
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var result = this.responseText;
            result = JSON.parse(result);
            $.each(result, function (index, value) {
                $("#currencies").append("<option value=" + index + ">[" + index + "] " + value + "</option>")
            });
        }

    };
    xhttp.open("POST", urlForCurrencies, true);
    xhttp.send();

    $("#changeCurrency").click(function () {
        var YourAppID = "your_openexchangerates_App_Id";
        var currentCurrency = $("#currencyCode").text();
        var convertToCurrency = $("#currencies option:selected").val();
        var urlForConvertation = "https://openexchangerates.org/api/latest.json?app_id=" + YourAppID;
        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {

                var currentCurrencyRate;
                var convertToCurrencyRate;

                var res = this.responseText;
                res = JSON.parse(res);
                var result = res.rates;

                $.each(result, function (index, value) {
                    if (index == currentCurrency) {
                        currentCurrencyRate = parseFloat(value);
                    }
                    else if (index == convertToCurrency) {
                        convertToCurrencyRate = parseFloat(value);
                    }
                    else if (currentCurrency == "USD") {
                        currentCurrencyRate = 1;
                    }
                });

                $(".currencyCode").text(convertToCurrency);

                var rate = parseFloat(convertToCurrencyRate / currentCurrencyRate);

                var itemIdCount = $("div[class*='item']").length;

                for (var i = 0; i < itemIdCount; i++) {
                    var newPrice = parseFloat($("#newPrice"+i+"").text()) * rate;
                    $("#newPrice"+i+"").text(newPrice);

                    var usedPrice = parseFloat($("#usedPrice"+i+"").text()) * rate;
                    $("#usedPrice"+i+"").text(usedPrice);
                }
            }
        };
        xhttp.open("POST", urlForConvertation, true);
        xhttp.send();
    });
});